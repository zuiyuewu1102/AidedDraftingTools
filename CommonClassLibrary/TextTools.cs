using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonClassLibrary
{
    public static class TextTools
    {
        /// <summary>
        /// 将字符串中的数字和小数点转换出来
        /// </summary>
        /// <param name="str">需转换的字符串</param>
        /// <returns></returns>
        public static string IntegerString(this string str)
        {
            string b = string.Empty;
            for (int i = 0; i < str.Length; i++)
            {
                if (Char.IsDigit(str[i]) || str[i] == '.')
                    b += str[i];
            }
            return b;
        }


        public static string Dzxwb(this string str,int zl)
        {
            string xstr = "";            
            int n=0;
            string b = string.Empty;
            str = str.Reverse().ToString();
            if (char.IsDigit(str.First()))
            {
                for (int i = 0; i < str.Length; i++)
                {
                    if (Char.IsDigit(str[i]))
                        b += str[i];
                    else
                    {
                        n = int.Parse(b.Reverse().ToString()) + zl;
                        str = str.Substring(i, str.Length).Reverse().ToString();
                        break;
                    }
                }
                xstr = str + n.ToString();
                return xstr;
            }
            else
                return xstr;
        }
    }
}
