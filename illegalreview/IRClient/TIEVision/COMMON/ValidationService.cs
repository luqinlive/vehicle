using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace TIEVision.COMMON
{
    class ValidationService
    {
        /// <summary>
        /// 判断字符是否为空 为空返回 true
        /// </summary>
        /// <param name="temp"></param>
        /// <returns>为空返回 true </returns>
        public static bool CheckNull(string temp)
        {
            if (temp == string.Empty || temp == null)
            {
                return true;
            }
            else
            {

                if (temp.Trim() == "")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

        }
        /// <summary>
        /// 判断两个字符是否相同
        /// </summary>
        /// <param name="temp"></param>
        /// <returns></returns>
        public static bool CheckEqual(string temp, string temp1)
        {
            if (temp.Equals(temp1))
            {

                return true;
            }
            else
            {
                return false;
            }

        }

        public static bool IsValidIP(string ip)
        {
            Regex a = new Regex(@"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){2}((2[0-4]\d|25[0-5]|[01]?\d\d?|\*)\.)(2[0-4]\d|25[0-5]|[01]?\d\d?|\*)$");
            if (a.IsMatch(ip.Trim()))
            {
                return true;

            }
            else
            {
                return false;
            }
        }
        public static bool IsRegexText(string text, string pattern)
        {
            Regex a = new Regex(pattern);
            if (a.IsMatch(text.Trim()))
            {
                return true;

            }
            else
            {
                return false;
            }
        }
    }
}
