using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IOSPush
{
    class Program
    {
        static void Main(string[] args)
        {
            string dsa = "43b2c28039905615b3246798e1b4bccee6d319e9f824e59b23bbcaed6d6c3346";
          // dsa = "466595f18db67760c2cfdab035932794eebcc25d6390d599a6f8a5461100703c";
          //  dsa = "43b2c28039905615b3246798e1b4bccee6d319e9f824e59b23bbcaed6d6c3346";
           // dsa = "466595f18db67760c2cfdab035932794eebcc25d6390d599a6f8a5461100703c";
           // dsa = "0a48b56f7b3bdc685ce4d7b5218fa5782dd4f1926ffed0844e1a9bcc3c760984";
          // dsa = "d93a84e962fdbb680e2da2967d78dbe8fbec74465168ee8d6359f5e7529eccc6";
           // dsa = "43b2c28039905615b3246798e1b4bccee6d319e9f824e59b23bbcaed6d6c3346";//
           dsa = "466595f18db67760c2cfdab035932794eebcc25d6390d599a6f8a5461100703c";
           int issucess = 0;
           string result=PushNotificationForIos.pushNotifications(dsa, "你好222 ", "yck.p12", ref issucess,"");

           
        }
    }
}
