using System;
using System.IO;
using System.IO.Pipes;
using System.Net.Sockets;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.CodeAnalysis.CompilerServer;
public class test : MonoBehaviour
{
    private const string PipeName = "test_pipe";


    private static void RunClient()
    {
        using (var client = UnixDomainSocket.CreateClient(PipeName))
        {
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

    }
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
    // Start is called before the first frame update
    void Start()
    {
        Console.WriteLine("Go!");
       // Debug.Log("Go");
        RunClient();
      //  RunServer();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
  

}
