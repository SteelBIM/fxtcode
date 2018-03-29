using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace OpenPlatform.Api.TCPServer
{
    public class Server
    {
        string ServerIP = System.Configuration.ConfigurationManager.AppSettings["ServerIP"];
        string ServerPort = System.Configuration.ConfigurationManager.AppSettings["ServerPort"];
        TcpListener listener;
        public Server()
        {
            listener = new TcpListener(new IPEndPoint(IPAddress.Parse(ServerIP), int.Parse(ServerPort)));

            listener.Start();
        }
    }
}
