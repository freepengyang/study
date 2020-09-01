using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FBPkgGen
{
   public static class Tools
    {
        public static byte[] GetBytes(string s)
        {
            return Encoding.GetEncoding("UTF-8").GetBytes(s);
        }

        public static string GetString(byte[] bytes)
        {
            return Encoding.GetEncoding("UTF-8").GetString(bytes);
        }
    }
}
