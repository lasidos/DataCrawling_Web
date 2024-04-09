using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;
using System;
using System.Web.Mvc;
using DataCrawling_Web.BSL.Authentication;
using DataCrawling_Web.DSL.Account;
using DataCrawling_Web.BSL.Common;
using DataCrawling_Web.BSL.Extentions;
using System.Linq;
using System.Xml.Linq;
using DataCrawling_Web.BSL.Smtp;

namespace DataCrawling_Web.Controllers.Auth
{
    public class AuthController : Controller
    {
        #region 계정찾기
        [Route("Auth/Find_Id")]
        public ActionResult Find_Id(string Step = "")
        {
            return View();
        }

        [Route("Auth/Step")]
        public ActionResult Step(string Step)
        {
            string page = string.Empty;
            switch (Step)
            {
                case "1":
                    page = "~/Views/Auth/_PartialView/Account/_FindWay.cshtml";
                    break;
                case "2":
                    page = "~/Views/Auth/_PartialView/Account/_UserInfo.cshtml";
                    break;
                case "3":
                    ViewBag.User_ID = AuthUser.JoinMember.User_ID;
                    AuthUser.JoinMember = null;
                    page = "~/Views/Auth/_PartialView/Account/_Completed.cshtml";
                    break;
            }
            return PartialView(page);
        }

        [HttpPost]
        [Route("Auth/FindID_Info")]
        public JsonResult FindID_Info(string name, string contact)
        {
            string resultCode = "", Message = "";

            var result = new Account().FindAccount(Utility.Encrypt_AES(name), Utility.Encrypt_AES(contact));
            if (result.FirstOrDefault().MSG == "OK")
            {
                resultCode = "0";
                AuthUser.JoinMember = new Models.UserInfo()
                {
                    User_ID = Utility.Decrypt_AES(result.FirstOrDefault().User_ID)
                };
            }
            else
            {
                resultCode = "-1";
                Message = result.FirstOrDefault().MSG;
            }
            return Json(new { resultCode, Message });
        }
        #endregion

        #region 비밀번호 찾기

        [Route("Auth/Find_Pwd")]
        public ActionResult Find_Pwd()
        {
            return View();
        }

        [Route("Auth/PwdStep")]
        public ActionResult PwdStep(string Step)
        {
            string page = string.Empty;
            switch (Step)
            {
                case "1":
                    page = "~/Views/Auth/_PartialView/PW/_UserInfo.cshtml";
                    break;
                case "2":
                    page = "~/Views/Auth/_PartialView/PW/_CertEmail.cshtml";
                    break;
                case "3":
                    page = "~/Views/Auth/_PartialView/PW/_SubmitPw.cshtml";
                    break;
                case "4":
                    page = "~/Views/Auth/_PartialView/PW/_Completed.cshtml";
                    break;
            }
            return PartialView(page);
        }

        [HttpPost]
        [Route("Auth/CheckEmail")]
        public JsonResult CheckEmail(string email)
        {
            string resultCode = "", Message = "";

            if (new Account().CheckExist(Utility.Encrypt_AES(email)).IsAny())
            {
                // 인증코드 생성
                string passCode = Utility.RndString8();

                //발신자 및 수신자 메일 설정
                string title = "마이플랫폼 | 회원가입";
                string content = string.Join("\n", Utility.ReadAllText(Server.MapPath("~/Resource/Text/PassCode.txt")));
                content = string.Format(content, email, passCode);


                string result = new Smtp().SendMail(title, content, email);
                if (result == "ok")
                {
                    var db_result = new Account().PushCode(Utility.Encrypt_AES(email), passCode);
                    if (db_result.IsAny())
                    {
                        resultCode = "0";
                        Message = "2";
                        AuthUser.JoinMember = new Models.UserInfo()
                        {
                            User_ID = email
                        };
                    }
                    else
                    {
                        resultCode = "-1";
                        Message = "인증코드 발송에 실패하였습니다.\n다시 시도해주시기 바랍니다.";
                    }
                }
                else
                {
                    resultCode = "-1";
                    Message = result;
                }
            }
            else
            {
                resultCode = "-1";
                Message = "회원정보가 없습니다.\n회원가입 후 다시 이용해주시기 바랍니다.";
            }

            return Json(new { resultCode, Message });
        }

        [HttpPost]
        [Route("Auth/CheckCode")]
        public JsonResult CheckCode(string email, string passCode)
        {
            string resultCode = "", Message = "";
            var result = new Account().ConfirmPushCode(Utility.Encrypt_AES(email), passCode);
            if (result.IsAny())
            {
                AuthUser.JoinMember.User_ID = email;
                resultCode = "0";
                Message = "3";
            }
            else
            {
                resultCode = "-1";
                Message = "인증번호를 잘못 입력하였습니다.\n인증코드를 다시 입력해주세요.";
            }
            return Json(new { resultCode, Message });
        }

        [HttpPost]
        [Route("Auth/ChangePassword")]
        public JsonResult ChangePassword(string pw)
        {
            string resultCode = "", Message = "";
            var result = new Account().ConfirmPasscode(Utility.Encrypt_AES(AuthUser.JoinMember.User_ID), Utility.Encrypt_SHA(pw));
            if (result.IsAny())
            {
                resultCode = "0";
                Message = "4";
            }
            else
            {
                resultCode = "-1";
                Message = "서버접속에 장애가 발생하였습니다.\n잠시후 다시 시도해주시기 바랍니다.";
            }
            return Json(new { resultCode, Message });
        }
        #endregion
    }
}