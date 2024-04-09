using DataCrawling_Web.BSL.Authentication;
using DataCrawling_Web.BSL.Common;
using DataCrawling_Web.BSL.Extentions;
using DataCrawling_Web.DSL.Account;
using DataCrawling_Web.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Web.Mvc;

namespace DataCrawling_Web.Controllers.Auth
{
    public class LoginController : BaseController
    {
        public static string acceptConditions = null;
        public static string certEmail = null;
        public static string userPw = null;
        public static string userName = null;

        public static string findEmail = null;

        #region 로그인

        public ActionResult Nid_Login(string redirectUrl = "/")
        {
            ViewBag.redirect = redirectUrl;
            return View("~/Views/Auth/Login/Nid_Login.cshtml");
        }

        [HttpPost]
        public JsonResult UserLogin(string email, string pw, string redirect = "/")
        {
            string msg = "";
            var result = new Account().UserLogin(Utility.Encrypt_AES(email), Utility.Encrypt_SHA(pw));
            if (result.IsAny() && result.FirstOrDefault().MSG == "OK")
            {
                UserInfo us = result.FirstOrDefault();
                string token = new ClsJWT().GenerateToken(
                                    redirect,
                                    Utility.Decrypt_AES(us.User_ID),
                                    Utility.Decrypt_AES(us.MemberType),
                                    Utility.Decrypt_AES(us.User_Name),
                                    Utility.Decrypt_AES(us.Phone),
                                    Utility.Decrypt_AES(us.Gender),
                                    30
                                );
                MKCtx.Session.Set("M_ID", us.User_ID);
                MKCtx.Session.Set("M_User", token);
                msg = us.MSG;
            }
            else msg = result.FirstOrDefault().MSG;
            return Json(new { msg, redirect });
        }

        #endregion

        #region 로그아웃

        public ActionResult Logout()
        {
            MKCtx.Session.Remove("M_ID");
            MKCtx.Session.Remove("M_User");
            return View("~/Views/Auth/Login/Logout.cshtml");
        }

        #endregion
    }
}