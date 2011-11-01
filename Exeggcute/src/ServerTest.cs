using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Exeggcute.src
{
    class ServerTest
    {
        int portmin = 9030;
        int portmax = 9039;
        Random rng = new Random();
        public int getPort()
        {
            int port = portmin + rng.NextInt(10);
            if (port < portmin || port > portmax) throw new ExeggcuteError("port out of range");
            return 9030;
            //return port;
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
                Console.WriteLine("{0}\nUnable to connect to server.", e.Message);
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
            Console.WriteLine("The client connection was shut down correctly");
        }
    }
}
