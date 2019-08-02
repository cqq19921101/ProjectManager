using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Lib_Portal_Domain.SupportClasses;
using Lib_Portal_Domain.Abstract;
using Lib_Portal_Domain.PortalLogicsForGeneralControllerAction;
using Lib_Portal_Domain.Concrete;
using Lib_Portal_Domain.Model;
using Portal_Web.SupportClasses;
using Lib_Portal_Domain.SharedLibs;
using System.Configuration;
using Lib_VMI.SharedLibs;
using System.Web.Configuration;
using Lib_Portal_MUIResources;
using Lib_VMI_Domain.SharedLibs;
using System.Text;
using System.Data.SqlClient;
using Lib_Portal_Domain_MUIResources;
using System.Threading;
using Lib_VMI_Domain.Model;
using System.Security.Cryptography;
using System.IO;
using Newtonsoft.Json;

namespace Portal_Web.Controllers
{
    [OverrideMemberCulture]
    public class RegistController : Controller
    {
        static String sKEY;
        static String sIV;
        static String sKEY2;
        static String sIV2;
        static String sKEY3;
        static String sIV3;


        private string UserName
        {
            get
            {
                return this.Request.Form["txtName"];
            }
        }
        private string PassWord
        {
            get
            {
                return this.Request.Form["txtPassWord"];
            }
        }

        [AllowAnonymous]
        // GET: Regist
        public ActionResult Regist()
        {
            return View();
        }

        //public string Index()
        //{
        //    //var VoderCode = Request.QueryString["VoderCode"];
        //    //var Name = Request.QueryString["Name"];
        //    //return VoderCode.ToString() + ":" + Name.ToString();

        //}
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
        //[AllowAnonymous]
        //public ActionResult Regist(string returnUrl)
        //{
        //    RegistModel lm = new RegistModel();
        //    return this.pc_Account.Login_HttpGet(lm, _LoginAuthProvider, Session, HttpContext, Request, Response, Url) ? PartialView(lm) : RedirectToLocal(returnUrl);
        //}

    }
}