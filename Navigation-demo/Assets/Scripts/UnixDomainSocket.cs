// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Microsoft.CodeAnalysis.CompilerServer
{
    internal sealed class UnixDomainSocket : IDisposable
    {
        private readonly Socket _serverSocket;
        private readonly string _pipePath;

        private UnixDomainSocket(Socket serverSocket, string pipePath)
        {
            _serverSocket = serverSocket;
            _pipePath = pipePath;
        }

        public void Dispose()
        {
            _serverSocket.Dispose();
            File.Delete(_pipePath);
        }

        // Internal type from corefx:src/System.IO.Pipes/src/System/Net/Sockets/UnixDomainSocketEndPoint.cs
        private static EndPoint CreateUnixDomainSocketEndPoint(string path)
        {
            var type = Assembly.Load(new AssemblyName("System.IO.Pipes"))
                .GetType("System.Net.Sockets.UnixDomainSocketEndPoint");
            return (EndPoint)Activator.CreateInstance(type, path);
        }

        // Copied from corefx:src/Common/src/Interop/Unix/System.Native/Interop.GetPeerID.cs
        [DllImport("System.Native", EntryPoint = "SystemNative_GetPeerID", SetLastError = true)]
        private static extern int GetPeerID(IntPtr socket, out uint euid);

        private static uint GetPeerID(Socket socket)
        {
            var handle = socket.Handle;
            var result = GetPeerID(handle, out var euid);
            if (result != 0)
            {
                throw new Exception($"getsockopt(SO_PEERCRED) or getpeereid failed ({result})");
            }
            return euid;
        }

        // Copied from corefx:src/Common/src/Interop/Unix/System.Native/Interop.GetEUid.cs
        [DllImport("System.Native", EntryPoint = "SystemNative_GetEUid")]
        internal static extern uint GetEUid();

        private static string NameToPath(string pipeName)
        {
            return Path.Combine(Path.GetTempPath(), pipeName);
        }

        private static bool Check(Socket connection)
        {
            var myId = GetEUid();
            var theirId = GetPeerID(connection);
            return myId == theirId;
        }

        public static UnixDomainSocket CreateServer(string pipeName)
        {
            var path = NameToPath(pipeName);
            File.Delete(path);
            var server = new Socket(AddressFamily.Unix, SocketType.Stream, ProtocolType.Unspecified);
            server.Bind(CreateUnixDomainSocketEndPoint(path));
            server.Listen(2);
            return new UnixDomainSocket(server, path);
        }

        public Socket WaitOne()
        {
            while (true)
            {
                var acceptedSocket = _serverSocket.Accept();
                if (!Check(acceptedSocket))
                {
                    acceptedSocket.Dispose();
                    continue;
                }
                return acceptedSocket;
            }
        }

        public static Socket CreateClient(string pipeName)
        {
            var path = NameToPath(pipeName);
            var socket = new Socket(AddressFamily.Unix, SocketType.Stream, ProtocolType.Unspecified);
            try
            {
                socket.Connect(CreateUnixDomainSocketEndPoint(path));
            }
            catch
            {
                return null;
            }
            if (!Check(socket))
            {
                return null;
            }
            return socket;
        }
    }
}
