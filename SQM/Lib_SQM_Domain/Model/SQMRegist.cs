using Lib_Portal_Domain.SharedLibs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Collections;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Web;
using System.IO;



namespace Lib_SQM_Domain.Model
{


    public static class BASE
    {
        static String sKEY;
        static String sIV;
        static String sKEY2;
        static String sIV2;
        static String sKEY3;
        static String sIV3;
        #region 加密解密
        public static string desEncryptBase64(string source)
        {
            string encrypt = "";
            {
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                byte[] key = Encoding.ASCII.GetBytes(sKEY);
                byte[] iv = Encoding.ASCII.GetBytes(sIV);
                byte[] dataByteArray = Encoding.UTF8.GetBytes(source);

                des.Key = key;
                des.IV = iv;

                using (MemoryStream ms = new MemoryStream())
                using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(dataByteArray, 0, dataByteArray.Length);
                    cs.FlushFinalBlock();
                    encrypt = Convert.ToBase64String(ms.ToArray());
                    StringBuilder sInsertEncrypt = new StringBuilder();
                    for (int i = 0; i < encrypt.Length; i++)
                    {
                        sInsertEncrypt.Append(encrypt[i] + ".");
                    }
                    encrypt = sInsertEncrypt.ToString();
                    encrypt = encrypt.Replace(".+.", ".ADD.");
                    encrypt = encrypt.Replace(".=", "=");
                    encrypt = encrypt.Replace("=.", "=");
                }
                encrypt = System.Web.HttpUtility.HtmlEncode(encrypt);
                //encrypt = Page.Server.UrlEncode(encrypt);
            }
            if (encrypt.ToLower().Contains("porn") || encrypt.ToLower().Contains("gcd"))
            {
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                byte[] key = Encoding.ASCII.GetBytes(sKEY2);
                byte[] iv = Encoding.ASCII.GetBytes(sIV2);
                byte[] dataByteArray = Encoding.UTF8.GetBytes(source);

                des.Key = key;
                des.IV = iv;

                using (MemoryStream ms = new MemoryStream())
                using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(dataByteArray, 0, dataByteArray.Length);
                    cs.FlushFinalBlock();
                    encrypt = Convert.ToBase64String(ms.ToArray());
                }
                encrypt = System.Web.HttpUtility.HtmlEncode(encrypt);
            }
            return encrypt;
        }

        public static string desDecryptBase64(string encrypt)
        {
            try
            {
                encrypt = HttpUtility.HtmlDecode(encrypt);
                //encrypt = Server.UrlDecode(encrypt);
                encrypt = encrypt.Replace(".ADD.", ".+.");
                encrypt = encrypt.Replace(" ", "+");
                encrypt = encrypt.Replace(".", "");
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                byte[] key = Encoding.ASCII.GetBytes(sKEY);
                byte[] iv = Encoding.ASCII.GetBytes(sIV);
                des.Key = key;
                des.IV = iv;

                byte[] dataByteArray = Convert.FromBase64String(encrypt);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(dataByteArray, 0, dataByteArray.Length);
                        cs.FlushFinalBlock();
                        return Encoding.UTF8.GetString(ms.ToArray());
                    }
                }
            }
            catch (Exception ex)
            {

            }
            try
            {
                encrypt = HttpUtility.HtmlDecode(encrypt);
                //encrypt = Server.UrlDecode(encrypt);
                encrypt = encrypt.Replace(" ", "+");
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                byte[] key = Encoding.ASCII.GetBytes(sKEY2);
                byte[] iv = Encoding.ASCII.GetBytes(sIV2);
                des.Key = key;
                des.IV = iv;

                byte[] dataByteArray = Convert.FromBase64String(encrypt);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(dataByteArray, 0, dataByteArray.Length);
                        cs.FlushFinalBlock();
                        return Encoding.UTF8.GetString(ms.ToArray());
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return "";
        }
        #endregion


    }




}


