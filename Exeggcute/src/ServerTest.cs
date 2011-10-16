using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace Exeggcute.src
{
    class ServerTest
    {
        int portmin = 9030;
        int portmax = 9039;
        Random rng = new Random();
        public int getPort()
        {
            int port = portmin + rng.NextInt(9);
            return 9030;
            return port;
        }
        public void Test()
        {

            IPEndPoint ip = new IPEndPoint(IPAddress.Parse("128.143.69.241"), getPort());

            Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                server.Connect(ip);
            }
            catch (SocketException e)
            {
                Console.WriteLine("Unable to connect to server.");
                return;
            }
            Console.WriteLine("Requesting data from server");
            server.Send(Encoding.ASCII.GetBytes("GetLogCount\r\n"));
            byte[] data = new byte[1024];
            int receivedDataLength = server.Receive(data);
            string stringData = Encoding.ASCII.GetString(data, 0, receivedDataLength);
            Console.WriteLine(stringData);
            Console.WriteLine("Data successful");
            server.Shutdown(SocketShutdown.Both);
            server.Close();
            Console.WriteLine("The server shut down correctly");
        }
    }
}
