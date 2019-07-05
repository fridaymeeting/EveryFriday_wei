using System;
using System.IO;
using System.IO.Pipes;
using System.Net.Sockets;
using System.Threading;
using Microsoft.CodeAnalysis.CompilerServer;

namespace UnixDomainSocketTest
{
    public static class Program
    {
        private const string PipeName = "test_pipe";

        private static void RunServer()
        {
            using (var server = UnixDomainSocket.CreateServer(PipeName))
            {
                while (true)
                {
                    using (var accepted = server.WaitOne())
                    using (var stream = new NetworkStream(accepted))
                    using (var reader = new StreamReader(stream))
                    using (var writer = new StreamWriter(stream))
                    {
                        Console.WriteLine("Server accepted");
                        writer.WriteLine("Server -> Client");
                        writer.Flush();
                        Console.WriteLine("Server written");
                        Console.WriteLine(reader.ReadLine());
                        Console.WriteLine("Server done");
                    }
                }
            }
        }

        private static void RunClient()
        {
            using (var client = UnixDomainSocket.CreateClient(PipeName))
            using (var stream = new NetworkStream(client))
            using (var reader = new StreamReader(stream))
            using (var writer = new StreamWriter(stream))
            {
                Console.WriteLine("Client connected");
                writer.WriteLine("Server -> Client");
                writer.Flush();
                Console.WriteLine("Client written");
                Console.WriteLine(reader.ReadLine());
                Console.WriteLine("Client done");
            }
        }

        private static void WaitForConnectionClose()
        {
            var server = new NamedPipeServerStream(
                PipeName,
                PipeDirection.InOut,
                NamedPipeServerStream.MaxAllowedServerInstances,
                PipeTransmissionMode.Byte,
                PipeOptions.Asynchronous);
            var thread = new Thread(() =>
            {
                Console.WriteLine("Begin wait for connection");
                try
                {
                    server.WaitForConnection();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception!");
                    Console.WriteLine(e);
                }
                Console.WriteLine("End wait for connection");
            });
            thread.Start();
            Thread.Sleep(1000);
            Console.WriteLine("Started thread, closing stream");
            server.Close();
            Console.WriteLine("Closed stream, Join()'ing thread");
            thread.Join();
            Console.WriteLine("Joined thread, done");
        }

        public static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Invalid usage");
                return;
            }
            Console.WriteLine("Go!");
            switch (args[0])
            {
                case "server":
                    RunServer();
                    break;
                case "client":
                    RunClient();
                    break;
                case "closeserver":
                    WaitForConnectionClose();
                    break;
            }
        }
    }
}
