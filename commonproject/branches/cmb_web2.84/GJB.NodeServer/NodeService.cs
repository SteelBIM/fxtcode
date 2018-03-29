using System;using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Common;
using SocketIOClient;
using SocketIOClient.Messages;
using System.Configuration;

namespace GJB.NodeServer
{
    /// <summary>
    /// 推送消息的实例
    /// <remarks>byte 2013-11-19</remarks>
    /// </summary>
    internal class NodeBase
    {
        private static Client _socket = null;
        public static string NodeServiceAddress
        {
            get
            {
                return ConfigurationManager.AppSettings["NodeServiceAddress"];
            }
        }
        public static Client Instance()
        {
            if (null == _socket)
            {
                _socket = new Client(NodeServiceAddress);
                _socket.SocketConnectionClosed += SocketConnectionClosed;
                _socket.Error += SocketError;
                _socket.Connect();
            }
            return _socket;
        }

        public static void Close()
        {
            if (_socket != null)
            {
                _socket.SocketConnectionClosed -= SocketConnectionClosed;
                _socket.Error -= SocketError;
                _socket.Dispose(); // close & dispose of socket client;
            }
        }

        private static void SocketError(object sender, ErrorEventArgs e)
        {
            LogHelper.Info("socket client error:" + e.Message);
            _socket = null;
        }

        private static void SocketConnectionClosed(object sender, EventArgs e)
        {
            LogHelper.Info("WebSocketConnection was terminated!");
            _socket = null;
        }
    }

    /// <summary>
    /// 推送消息的服务
    /// <remarks>byte 2013-11-19</remarks>
    /// </summary>
    public class NodeService
    {
        public static void Emit(dynamic paylode, EnumHelper.SendType type)
        {
            Client socket = NodeBase.Instance();
            if (null != socket)
            {
                if (EnumHelper.SendType.用户消息 == type)
                    socket.Emit("sendtousers", paylode);
                else if (EnumHelper.SendType.部门消息 == type)
                    socket.Emit("sendtodepartment", paylode);
                else
                    socket.Emit("sendtocompany", paylode);
            }
        }

        public static void Close()
        {
            NodeBase.Close();
        }

        public static string NodeServiceDomain
        {
            get
            {
                return ConfigurationManager.AppSettings["NodeServiceDomain"];
            }
        }
    }
}
