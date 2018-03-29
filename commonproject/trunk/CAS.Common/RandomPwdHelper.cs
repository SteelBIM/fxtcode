using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.Common
{
    public class RandomPwdHelper
    {
        private const string randomChars = "BCDFGHJKMPQRTVWXY2346789";
        public static string GetRandomPassword(int passwordLen)
        {
            string password = string.Empty;
            int randomNum;
            Random random = new Random();
            for (int i = 0; i < passwordLen; i++)
            {
                randomNum = random.Next(randomChars.Length);
                password += randomChars[randomNum];
            }
            return password;
        }
    }
}
