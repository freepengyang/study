using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using System;
namespace ExtendEditor
{
    public static class MD5Utility
    {
        public static string GetMD5HashFromString(string str)
        {

            System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();

            byte[] bytValue, bytHash;

            bytValue = System.Text.Encoding.UTF8.GetBytes(str);

            bytHash = md5.ComputeHash(bytValue);

            md5.Clear();

            string sTemp = "";

            for (int i = 0; i < bytHash.Length; i++)
            {

                sTemp += bytHash[i].ToString("X").PadLeft(2, '0');

            }

            return sTemp.ToUpper();

        }

        public static string GetMD5HashFromFile(string filePath)
        {

            try
            {

                FileStream file = new FileStream(filePath, FileMode.Open);

                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();

                byte[] retVal = md5.ComputeHash(file);

                file.Close();

                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < retVal.Length; i++)
                {

                    sb.Append(retVal[i].ToString("x2"));

                }

                return sb.ToString().ToUpper();

            }

            catch (Exception ex)
            {

                throw new Exception("GetMD5HashFromFile() fail,error:" + filePath+" "+ex.Message);

            }

        }
    }
}

