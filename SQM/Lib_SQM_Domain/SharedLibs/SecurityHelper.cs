using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Lib_SQM_Domain.SharedLibs
{
    public static class SecurityHelper
    {
        public static bool PasswordIsOK(string Password)
        {
            string[] CharTypeList = new string[] { "ABCDEFGHIJKLMNOPQRSTUVWXYZ", "ABCDEFGHIJKLMNOPQRSTUVWXYZ", "1234567890" };
            string sPWD = Password.Trim();
            if ((sPWD.Length < 8) || (sPWD.Length > 20)) return false;
            bool[] CharTypeExist = new bool[] { false, false, false };
            for (int iCnt1 = 0; iCnt1 < sPWD.Length; iCnt1++)
                for (int iCnt2 = 0; iCnt2 < 3; iCnt2++)
                    if (CharTypeList[iCnt2].IndexOf(sPWD.Substring(iCnt1, 1)) > -1)
                        CharTypeExist[iCnt2] = true;
            if ((CharTypeExist[0] == false) || (CharTypeExist[1] == false) || (CharTypeExist[2] == false)) return false;
            return true;
        }

        public static string CheckActivationData(string PWD1, string PWD2)
        {
            string r = "";
            if ((PWD1 == "") || (PWD2 == "")) r += "<br />Must provide password.";
            if (PWD1 != PWD2) r += "<br />Both password must identical.";
            if (r == "")
            {
                if (!PasswordIsOK(PWD1))
                {
                    r = "Password should follow the rules(security policy) below:";
                    r += "<br />1. Password should contain at least 8 characters.";
                    r += "<br />2. Password can only contain capital letters, unicase, or number digit.";
                    r += "<br />3. Password should contain at least one contain capital letters, at least one unicase, and at least one number digit.";
                    r += "<br />Please check/correct it, and then submit again.";
                }
            }
            else
            {
                r = r.Substring(6);
            }
            return r;
        }

        public static string MD5Hash(string text)
        {
            MD5 md5 = new MD5CryptoServiceProvider();

            //compute hash from the bytes of text
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(text));

            //get hash result after compute it
            byte[] result = md5.Hash;

            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                //change it into 2 hexadecimal digits
                //for each byte
                strBuilder.Append(result[i].ToString("x2"));
            }

            return strBuilder.ToString();
        }
    }

    public enum EncoderType
    {
        //可逆編碼(對稱金鑰)
        AES,
        DES,
        RC2,
        TripleDES,

        //可逆編碼(非對稱金鑰)
        RSA,

        //不可逆編碼(雜湊值)
        MD5,
        SHA1,
        SHA256,
        SHA384,
        SHA512
    }

    public class EncodeHelper
    {
        public string Key { get; set; }
        public string IV { get; set; }

        public EncodeHelper()
        {
            Key = ConfigurationManager.AppSettings["EncodeKey"].ToString();
            IV = ConfigurationManager.AppSettings["EncodeIV"].ToString();
        }

        public void GenerateKey(EncoderType type)
        {
            switch (type)
            {
                //可逆編碼(對稱金鑰)
                case EncoderType.AES:
                    using (AesCryptoServiceProvider csp = new AesCryptoServiceProvider())
                    {
                        csp.GenerateIV();
                        IV = Convert.ToBase64String(csp.IV);
                        csp.GenerateKey();
                        Key = Convert.ToBase64String(csp.Key);
                    }
                    break;
                case EncoderType.DES:
                    using (DESCryptoServiceProvider csp = new DESCryptoServiceProvider())
                    {
                        csp.GenerateIV();
                        IV = Convert.ToBase64String(csp.IV);
                        csp.GenerateKey();
                        Key = Convert.ToBase64String(csp.Key);
                    }
                    break;
                case EncoderType.RC2:
                    using (RC2CryptoServiceProvider csp = new RC2CryptoServiceProvider())
                    {
                        csp.GenerateIV();
                        IV = Convert.ToBase64String(csp.IV);
                        csp.GenerateKey();
                        Key = Convert.ToBase64String(csp.Key);
                    }
                    break;
                case EncoderType.TripleDES:
                    using (TripleDESCryptoServiceProvider csp = new TripleDESCryptoServiceProvider())
                    {
                        csp.GenerateIV();
                        IV = Convert.ToBase64String(csp.IV);
                        csp.GenerateKey();
                        Key = Convert.ToBase64String(csp.Key);
                    }
                    break;
                //可逆編碼(非對稱金鑰)
                case EncoderType.RSA:
                    using (RSACryptoServiceProvider csp = new RSACryptoServiceProvider())
                    {
                        IV = "";
                        Key = csp.ToXmlString(true);
                        string publicKey = csp.ToXmlString(false);
                    }
                    break;
            }
        }

        #region Byte method
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="type">編碼器種類</param>  
        /// <param name="encrypt">加密前Binary</param>
        /// <returns>加密後字串</returns>
        public byte[] EncryptByte(EncoderType type, byte[] encrypt)
        {
            byte[] ret = null;
            byte[] inputByteArray = encrypt;

            switch (type)
            {
                //可逆編碼(對稱金鑰)
                case EncoderType.AES:
                    using (AesCryptoServiceProvider csp = new AesCryptoServiceProvider())
                    {
                        byte[] rgbKey = Convert.FromBase64String(Key);
                        byte[] rgbIV = Convert.FromBase64String(IV);

                        using (MemoryStream ms = new MemoryStream())
                        {
                            using (CryptoStream cs = new CryptoStream(ms, csp.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write))
                            {
                                cs.Write(encrypt, 0, encrypt.Length);
                                cs.FlushFinalBlock();
                                ret = ms.ToArray();
                            }
                        }
                    }
                    break;
                case EncoderType.DES:
                    using (DESCryptoServiceProvider csp = new DESCryptoServiceProvider())
                    {
                        byte[] rgbKey = Convert.FromBase64String(Key);
                        byte[] rgbIV = Convert.FromBase64String(IV);

                        using (MemoryStream ms = new MemoryStream())
                        {
                            using (CryptoStream cs = new CryptoStream(ms, csp.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write))
                            {
                                cs.Write(inputByteArray, 0, inputByteArray.Length);
                                cs.FlushFinalBlock();
                                ret = ms.ToArray();
                            }
                        }
                    }
                    break;
                case EncoderType.RC2:
                    using (RC2CryptoServiceProvider csp = new RC2CryptoServiceProvider())
                    {
                        byte[] rgbKey = Convert.FromBase64String(Key);
                        byte[] rgbIV = Convert.FromBase64String(IV);

                        using (MemoryStream ms = new MemoryStream())
                        {
                            using (CryptoStream cs = new CryptoStream(ms, csp.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write))
                            {
                                cs.Write(inputByteArray, 0, inputByteArray.Length);
                                cs.FlushFinalBlock();
                                ret = ms.ToArray();
                            }
                        }
                    }
                    break;
                case EncoderType.TripleDES:
                    using (TripleDESCryptoServiceProvider csp = new TripleDESCryptoServiceProvider())
                    {
                        byte[] rgbKey = Convert.FromBase64String(Key);
                        byte[] rgbIV = Convert.FromBase64String(IV);

                        using (MemoryStream ms = new MemoryStream())
                        {
                            using (CryptoStream cs = new CryptoStream(ms, csp.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write))
                            {
                                cs.Write(inputByteArray, 0, inputByteArray.Length);
                                cs.FlushFinalBlock();
                                ret = ms.ToArray();
                            }
                        }
                    }
                    break;
                //可逆編碼(非對稱金鑰)
                case EncoderType.RSA:
                    using (RSACryptoServiceProvider csp = new RSACryptoServiceProvider())
                    {
                        csp.FromXmlString(Key);
                        ret = csp.Encrypt(inputByteArray, false);
                    }
                    break;
                //不可逆編碼(雜湊值)
                case EncoderType.MD5:
                    using (MD5CryptoServiceProvider csp = new MD5CryptoServiceProvider())
                    {
                        ret = csp.ComputeHash(inputByteArray);
                    }
                    break;
                case EncoderType.SHA1:
                    using (SHA1CryptoServiceProvider csp = new SHA1CryptoServiceProvider())
                    {
                        ret = csp.ComputeHash(inputByteArray);
                    }
                    break;
                case EncoderType.SHA256:
                    using (SHA256CryptoServiceProvider csp = new SHA256CryptoServiceProvider())
                    {
                        ret = csp.ComputeHash(inputByteArray);
                    }
                    break;
                case EncoderType.SHA384:
                    using (SHA384CryptoServiceProvider csp = new SHA384CryptoServiceProvider())
                    {
                        ret = csp.ComputeHash(inputByteArray);
                    }
                    break;
                case EncoderType.SHA512:
                    using (SHA512CryptoServiceProvider csp = new SHA512CryptoServiceProvider())
                    {
                        ret = csp.ComputeHash(inputByteArray);
                    }
                    break;
            }
            return ret;
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="type">編碼器種類</param>  
        /// <param name="decrypt">解密前Binary</param>
        /// <returns>解密後字串</returns>
        public byte[] DecryptByte(EncoderType type, byte[] decrypt)
        {
            byte[] ret = null;
            byte[] inputByteArray = decrypt;

            switch (type)
            {
                //可逆編碼(對稱金鑰)
                case EncoderType.AES:
                    using (AesCryptoServiceProvider csp = new AesCryptoServiceProvider())
                    {
                        byte[] rgbKey = Convert.FromBase64String(Key);
                        byte[] rgbIV = Convert.FromBase64String(IV);

                        using (MemoryStream ms = new MemoryStream())
                        {
                            using (CryptoStream cs = new CryptoStream(ms, csp.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write))
                            {
                                cs.Write(inputByteArray, 0, inputByteArray.Length);
                                cs.FlushFinalBlock();
                                ret = ms.ToArray();
                            }
                        }
                    }
                    break;
                case EncoderType.DES:
                    using (DESCryptoServiceProvider csp = new DESCryptoServiceProvider())
                    {
                        byte[] rgbKey = Convert.FromBase64String(Key);
                        byte[] rgbIV = Convert.FromBase64String(IV);

                        using (MemoryStream ms = new MemoryStream())
                        {
                            using (CryptoStream cs = new CryptoStream(ms, csp.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write))
                            {
                                cs.Write(inputByteArray, 0, inputByteArray.Length);
                                cs.FlushFinalBlock();
                                ret = ms.ToArray();
                            }
                        }
                    }
                    break;
                case EncoderType.RC2:
                    using (RC2CryptoServiceProvider csp = new RC2CryptoServiceProvider())
                    {
                        byte[] rgbKey = Convert.FromBase64String(Key);
                        byte[] rgbIV = Convert.FromBase64String(IV);

                        using (MemoryStream ms = new MemoryStream())
                        {
                            using (CryptoStream cs = new CryptoStream(ms, csp.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write))
                            {
                                cs.Write(inputByteArray, 0, inputByteArray.Length);
                                cs.FlushFinalBlock();
                                ret = ms.ToArray();
                            }
                        }
                    }
                    break;
                case EncoderType.TripleDES:
                    using (TripleDESCryptoServiceProvider csp = new TripleDESCryptoServiceProvider())
                    {
                        byte[] rgbKey = Convert.FromBase64String(Key);
                        byte[] rgbIV = Convert.FromBase64String(IV);

                        using (MemoryStream ms = new MemoryStream())
                        {
                            using (CryptoStream cs = new CryptoStream(ms, csp.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write))
                            {
                                cs.Write(inputByteArray, 0, inputByteArray.Length);
                                cs.FlushFinalBlock();
                                ret = ms.ToArray();
                            }
                        }
                    }
                    break;
                //可逆編碼(非對稱金鑰)
                case EncoderType.RSA:
                    using (RSACryptoServiceProvider csp = new RSACryptoServiceProvider())
                    {
                        csp.FromXmlString(Key);
                        ret = csp.Decrypt(inputByteArray, false);
                    }
                    break;
            }
            return ret;
        }
        #endregion
    }
}
