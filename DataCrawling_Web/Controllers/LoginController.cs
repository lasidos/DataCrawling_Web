using DataCrawling_Web.BSL.Authentication;
using DataCrawling_Web.BSL.Common;
using DataCrawling_Web.BSL.Extentions;
using DataCrawling_Web.DSL.Account;
using DataCrawling_Web.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Web.Mvc;

namespace DataCrawling_Web.Controllers
{
    public class LoginController : BaseController
    {
        public static string acceptConditions = null;
        public static string certEmail = null;
        public static string userPw = null;
        public static string userName = null;

        public static string findEmail = null;

        #region 로그인

        public ActionResult Nid_Login(string redirectUrl = "https://www.myplatformkorea.com")
        {
            ViewBag.redirect = redirectUrl;
            return View();
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

        #region 계정찾기

        public ActionResult find_account(string step = "")
        {
            ViewBag.step = step;
            if (step == "find-completed") ViewBag.findEmail = findEmail;

            return View();
        }

        public string find_userId(string name, string contact)
        {
            string result = new Account().FindAccount(AuthUser.Encrypt_AES(name), AuthUser.Encrypt_AES(contact));
            if (result.Split('|')[0] == "0")
            {
                JArray jarr = JArray.Parse(result.Split('|')[1]);

                string email = null;
                foreach (JObject jobj in jarr)
                {
                    email = AuthUser.Decrypt_AES(jobj["이메일"].ToString());
                }

                if (email != null) findEmail = email;

                return "0";
            }
            else
            {
                if (result.Split('|').Length > 1) return string.Format("-1|{0}", result.Split('|')[1]);
                else return string.Format("-1|{0}", result);
            }
        }

        #endregion

        #region 비밀번호 찾기

        public ActionResult find_password(string step = "")
        {
            ViewBag.step = step;
            if (step == "cert-email" || step == "changePw") ViewBag.userEmail = findEmail;

            return View();
        }

        public string findPassword(string email)
        {
            int cnt = 0;
            string result = new Account().CheckExist(AuthUser.Encrypt_AES(email));
            if (result.Split('|')[0] == "0")
            {
                JArray jarr = JArray.Parse(result.Split('|')[1]);
                foreach (JObject jobj in jarr)
                {
                    cnt = Convert.ToInt32(jobj["cnt"].ToString());
                }

                if (cnt > 0)
                {
                    string passCode = null;
                    for (int i = 0; i < 8; i++)
                    {
                        passCode += Utility.GetRandomNumber(0, 9);
                    }

                    //발신자 및 수신자 메일 설정
                    string title = "MyPlatform 인증메일입니다.";
                    string content = string.Join("\n", Utility.ReadAllText(Server.MapPath("~/Resource/Text/Account/PassCode.txt")));
                    content = string.Format(content, email, passCode);

                    result = new Account().PushPasscode(AuthUser.Encrypt_AES(title), AuthUser.Encrypt_AES(content),
                        AuthUser.Encrypt_AES(email), AuthUser.Encrypt_AES(passCode));
                    if (result.Split('|')[0] == "0")
                    {
                        findEmail = email;
                        return "ok";
                    }
                    else
                    {
                        if (result.Split('|').Length > 1) return string.Format("-1|{0}", result.Split('|')[1]);
                        else return string.Format("-1|{0}", result);
                    }
                }
                else return "fail";
            }
            else
            {
                if (result.Split('|').Length > 1) return string.Format("-1|{0}", result.Split('|')[1]);
                else return string.Format("-1|{0}", result);
            }
        }

        public string ChangePassword(string pw)
        {
            int cnt = 0;
            string result = new Account().ChangePassword(AuthUser.Encrypt_AES(findEmail), AuthUser.Encrypt_SHA(pw));
            if (result.Split('|')[0] == "0")
            {
                JArray jarr = JArray.Parse(result.Split('|')[1]);
                foreach (JObject jobj in jarr)
                {
                    cnt = Convert.ToInt32(jobj["Cnt"].ToString());
                }

                if (cnt > 0) return "ok";
                else return "fail";
            }
            else
            {
                if (result.Split('|').Length > 1) return string.Format("-1|{0}", result.Split('|')[1]);
                else return string.Format("-1|{0}", result);
            }
        }

        #endregion

        #region 회원가입

        public ActionResult Create_account(string step = "")
        {
            ViewBag.subpage = step;
            if (step == "requestVerifyEmail")
            {
                if (acceptConditions != null) return View();
                else return View("WrongWay");
            }
            if (step == "userPw")
            {
                if (certEmail != null)
                {
                    ViewBag.userEmail = certEmail;
                    return View();
                }
                else return View("WrongWay");
            }
            else if (step == "userProfile")
            {
                if (certEmail != null && userPw != null)
                {
                    ViewBag.userEmail = certEmail;
                    return View();
                }
                else return View("WrongWay");
            }
            else if (step == "completed")
            {
                if (certEmail != null && userName != null)
                {
                    ViewBag.userEmail = certEmail;
                    ViewBag.userName = userName;

                    certEmail = null;
                    userPw = null;
                    userName = null;
                    return View();
                }
                else return View("WrongWay");
            }
            else certEmail = null;

            return View();

            //return PartialView(subpage);
        }

        public string AcceptConditions(bool accept)
        {
            if (accept) acceptConditions = "동의";
            else acceptConditions = "동의안함";
            return "ok";
        }

        public string CheckExist(string email)
        {
            string result = new Account().CheckExist(AuthUser.Encrypt_AES(email));
            if (result.Split('|')[0] == "0") return string.Format("0|{0}", result.Split('|')[1]);
            else
            {
                if (result.Split('|').Length > 1) return string.Format("-1|{0}", result.Split('|')[1]);
                else return string.Format("-1|{0}", result);
            }
        }

        public string PushPasscode(string email)
        {
            string passCode = null;
            for (int i = 0; i < 8; i++)
            {
                passCode += Utility.GetRandomNumber(0, 9);
            }

            //발신자 및 수신자 메일 설정
            string title = "MyPlatform 인증메일입니다.";
            string content = string.Join("\n", Utility.ReadAllText(Server.MapPath("~/Resource/Text/Account/PassCode.txt")));
            content = string.Format(content, email, passCode);

            string result = new Account().PushPasscode(AuthUser.Encrypt_AES(title), AuthUser.Encrypt_AES(content),
                AuthUser.Encrypt_AES(email), AuthUser.Encrypt_AES(passCode));
            if (result.Split('|')[0] == "0") return string.Format("0|{0}", result.Split('|')[1]);
            else
            {
                if (result.Split('|').Length > 1) return string.Format("-1|{0}", result.Split('|')[1]);
                else return string.Format("-1|{0}", result);
            }
        }
        public string ConfirmPasscode(string email)
        {
            certEmail = email;
            string result = new Account().ConfirmPasscode(AuthUser.Encrypt_AES(email));
            if (result.Split('|')[0] == "0") return string.Format("0|{0}", result.Split('|')[1]);
            else
            {
                if (result.Split('|').Length > 1) return string.Format("-1|{0}", result.Split('|')[1]);
                else return string.Format("-1|{0}", result);
            }
        }

        public string UserPassword(string pw)
        {
            userPw = pw;
            return "ok";
        }

        public string UserProfile(string name, string tel, string gender)
        {
            userName = name;
            if (gender == "Male") gender = "남성";
            else if (gender == "Female") gender = "여성";
            else gender = "선택안함";

            string result = new Account().RegisterMember(
                                        AuthUser.Encrypt_AES(certEmail)
                                        , AuthUser.Encrypt_SHA(userPw)
                                        , AuthUser.Encrypt_AES(name)
                                        , AuthUser.Encrypt_AES(tel)
                                        , AuthUser.Encrypt_AES(gender)
                                        , AuthUser.Encrypt_AES(acceptConditions)
                                        , AuthUser.Encrypt_AES("0"));
            if (result.Split('|')[0] == "0")
            {
                JArray jarr = JArray.Parse(result.Split('|')[1]);
                if (Convert.ToInt32(jarr[0]["Cnt"].ToString()) > 0) return "ok";
                else return "not exist";
            }
            else
            {
                if (result.Split('|').Length > 1) return string.Format("-1|{0}", result.Split('|')[1]);
                else return string.Format("-1|{0}", result);
            }
        }

        #endregion
    }
}