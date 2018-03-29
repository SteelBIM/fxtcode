using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Net.Mail;

namespace CAS.Common
{
    public class Validator
    {
        public static bool HasInvalidCtrlChar(string input)
        {
            Regex rex = new Regex(@"[\x01-\x08\x0B-\x0C\x0E-\x1F\x7F-\x84\x86-\x9F]");
            return rex.IsMatch(input);
        }

        /// <summary>
        /// Regex:^[\da-zA-Z_@ ]{1,100}$
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsValidChar(string value)
        {
            if (string.IsNullOrEmpty(value)) value = "";
            bool result = Regex.IsMatch(value, @"^[\da-zA-Z_@ ]{1,100}$");

            return result;
        }

        /// <summary>
        /// Regex:^[\d]{7,12}$
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsPhoneNumber(string value, bool isRequired)
        {
            bool result = Regex.IsMatch(value, @"^[\d]{7,12}$", RegexOptions.IgnoreCase);
            if (!isRequired)
            {
                result = string.IsNullOrEmpty(value) || result;
            }
            return result;
        }
        /// <summary>
        /// Regex:^0|1$
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsFlagNum(string value)
        {
            return Regex.IsMatch(value, @"^0|1$");
        }

        /// <summary>
        /// by int.TryParse
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsInt(string value)
        {
            bool result = false;
            int i = 0;
            result = int.TryParse(value, out i);
            return result;
        }

        /// <summary>
        /// Regex:^\d{1,9}$
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsPrice(string value)
        {
            return Regex.IsMatch(value, @"^\d{1,9}$");
        }
        public static bool IsPrice(string value, bool isRequired)
        {
            bool result = Regex.IsMatch(value, @"^\d{1,9}$");
            if (!isRequired)
            {
                result = string.IsNullOrEmpty(value) || result;
            }
            return result;
        }
        
        /// <summary>
        /// by int.TryParse
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsInt(string value, bool isRequired)
        {
            bool result = IsInt(value);
            if (!isRequired)
            {
                result = string.IsNullOrEmpty(value) || result;
            }
            return result;
        }
        /// <summary>
        /// by float.TryParse
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsFloat(string value)
        {
            bool result = false;
            float f = 0;
            result = float.TryParse(value, out f);
            return result;
        }

        /// <summary>
        /// IsInt(value) or IsFloat(value)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNumeric(string value)
        {
            return IsInt(value) || IsFloat(value);
        }

        public static bool IsTime(string value, bool isRequired)
        {
            bool result = Regex.IsMatch(value, @"^((2[0123])|([01]\d)|\d):(([0-5]\d)|\d)(:(([0-5]\d)|\d))(\s?[AM,PM]{0,2})?$", RegexOptions.IgnoreCase);
            if (!isRequired)
            {
                result = string.IsNullOrEmpty(value) || result;
            }
            return result;
        }

        public static bool IsShortTime(string value, bool isRequired)
        {
            bool result = Regex.IsMatch(value, @"^((2[0123])|([01]\d)|\d):(([0-5]\d)|\d)(\s?[AM,PM]{0,2})?$", RegexOptions.IgnoreCase);
            if (!isRequired)
            {
                result = string.IsNullOrEmpty(value) || result;
            }
            return result;
        }

        public static bool IsDateTime(string value, bool isRequired)
        {
            bool result = Regex.IsMatch(value, @"^((((19|20)(([02468][048])|([13579][26]))-02-29))|((20[0-9][0-9])|(19[0-9][0-9]))-((((0[1-9])|(1[0-2]))-((0[1-9])|(1\d)|(2[0-8])))|((((0[13578])|(1[02]))-31)|(((01,3-9])|(1[0-2]))-(29|30)))))\s((2[0123])|([01]\d)|\d):(([0-5]\d)|\d)(:(([0-5]\d)|\d))(\s?[AM,PM]{0,2})?$", RegexOptions.IgnoreCase);
            if (!isRequired)
            {
                result = string.IsNullOrEmpty(value) || result;
            }
            return result;
        }

        /// <summary>
        /// 是否为日期型字符串
        /// </summary>
        /// <param name="StrSource">日期字符串(2008-05-08)</param>
        /// <returns></returns>
        public static bool IsDate(string value, bool isRequired)
        {
            bool result = Regex.IsMatch(value, @"^((?!0000)[0-9]{4}-((0[1-9]|1[0-2])-(0[1-9]|1[0-9]|2[0-8])|(0[13-9]|1[0-2])-(29|30)|(0[13578]|1[02])-31)|([0-9]{2}(0[48]|[2468][048]|[13579][26])|(0[48]|[2468][048]|[13579][26])00)-02-29)$", RegexOptions.IgnoreCase);
            if (!isRequired)
            {
                result = string.IsNullOrEmpty(value) || result;
            }
            return result;            
        }
 
        public static bool IsIP(string value, bool isRequired)
        {
            bool result = Regex.IsMatch(value, @"^(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9])\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[0-9])$", RegexOptions.IgnoreCase);
            if (!isRequired)
            {
                result = string.IsNullOrEmpty(value) || result;
            }
            return result;
        }

        public static bool IsEmail(string value, bool isRequired)
        {
            bool result = true;
            bool isEmpty = string.IsNullOrEmpty(value);
            if (!isEmpty)
            {
                try
                {
                    string e = (new MailAddress(value)).Address;
                    result = true && value.Length < 51;
                }
                catch (FormatException)
                {
                    result = false;
                }
            }
            if (!isRequired)
            {
                result = isEmpty || result;
            }

            return result;
        }
    }
}
