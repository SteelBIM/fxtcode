using System;
using System.Text;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace FXT.VQ.UserService.Model
{
    [Serializable]
    public class SecurityInfo
    {
        //private string _appKey;
        private string _functionname;

        public SecurityInfo(string functionname)
        {
            _functionname = functionname;
        }

        public string appid
        {
            get
            {
                return ConfigSettings.mUserCenterAppid;
            }
        }
        public string apppwd
        {
            get
            {
                return ConfigSettings.mUserCenterApppwd;
            }
        }
        public string signname
        {
            get
            {
                return ConfigSettings.mUserCenterSignname;
            }
        }
        public string time
        {
            get
            {
                return DateTime.Now.ToString("yyyyMMddHHmmss");
            }
        }
        public string functionname
        {
            get
            {
                return _functionname;
            }
        }

        private string[] _securityArray = new string[5];
        public string code
        {
            get
            {
                _securityArray[0] = this.appid;
                _securityArray[1] = this.apppwd;
                _securityArray[2] = this.signname;
                _securityArray[3] = this.time;
                _securityArray[4] = this.functionname;
                Array.Sort(_securityArray);
                return GetMd5(string.Join("", _securityArray), ConfigSettings.mUserCenterAppkey).ToLower();
            }
        }
        public string GetMd5(string strmd5, string key)
        {
            if (!string.IsNullOrEmpty(key))
            {
                strmd5 += key;
            }
            byte[] md5Bytes = ASCIIEncoding.Default.GetBytes(strmd5);
            byte[] encodedBytes;
            MD5 md5;
            md5 = new MD5CryptoServiceProvider();
            //FileStream fs= new FileStream(filepath,FileMode.Open,FileAccess.Read);
            encodedBytes = md5.ComputeHash(md5Bytes);
            string nn = BitConverter.ToString(encodedBytes);
            nn = Regex.Replace(nn, "-", "");//因为转化完的都是34-2d这样的，所以替换掉- 
            nn = nn.ToLower();//根据需要转化成小写
            //fs.Close();
            return nn;
        }
    }
}
