using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CDI.Client
{
    class MacHelper
    {
        /// <summary>
        /// 获取本地Mac地址
        /// </summary>
        public static string GetLocalMac()
        {
            string mac = null;
            var result = GetLocalMacs();
            if (result != null && result.Count > 0)
            {
                mac = result[0];
                char c = '-';
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < mac.Length; i++)
                {
                    sb.Append(mac[i]);
                    if (i % 2 == 1)
                    {
                        sb.Append(c);
                    }
                }
                if (sb.Length > 0 && sb[sb.Length - 1] == c)
                {
                    sb.Remove(sb.Length - 1, 1);
                }
                mac = sb.ToString();
            }
            return mac;
        }

        ///<summary>
        /// 通过NetworkInterface读取网卡Mac
        ///</summary>
        ///<returns></returns>
        public static IList<string> GetLocalMacs()
        {
          IList<string> macs =new List<string>();
          NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();
          foreach (NetworkInterface ni in interfaces)
          {
              if (ni.OperationalStatus == OperationalStatus.Up && ni.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
              {
                  PhysicalAddress pd = ni.GetPhysicalAddress();
                  string addr = pd.ToString();
                  if (!string.IsNullOrWhiteSpace(addr))
                  {
                      macs.Add(addr);
                  }
              }
          }
          return macs;
        }

    }
}
