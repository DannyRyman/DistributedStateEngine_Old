using System;
using DistributedStateEngine;
using log4net.Config;

namespace RaftSpike
{
    class Program
    {
        static void Main(string[] args)
        {
            XmlConfigurator.Configure();

            /*
            var server = new Server();
            server.Start();*/

            Console.ReadLine();
        }
    }
}
