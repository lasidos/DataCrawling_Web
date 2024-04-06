using DataCrawling_Web.BSL.Common;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Web.Mvc;
using DataCrawling_Web.BSL.Smtp;
using DataCrawling_Web.DSL.Account;
using DataCrawling_Web.BSL.Extentions;
using System.Linq;
using System.Runtime.Remoting;
using DataCrawling_Web.BSL.Authentication;
using System;

namespace DataCrawling_Web.Controllers
{
    public class JoinController : Controller
    {
        // GET: Join
        public ActionResult Regist()
        {
            return View();
        }

        public ActionResult Step(string Step)
        {
            string page = string.Empty, title = string.Empty;
            switch (Step)
            {
                case "1":
                    title = "약관동의";
                    page = "_Regist_Step1";
                    break;
                case "2":
                    title = "이메일 인증";
                    page = "_Regist_Step2";
                    break;
                case "3":
                    title = "비밀번호 설정";
                    page = "_Regist_Step3";
                    break;
                case "4":
                    title = "프로필 설정";
                    page = "_Regist_Step4";
                    break;
                case "5":
                    title = "가입하기 완료";
                    page = "_Regist_Step5";
                    break;
            }
            ViewBag.Title = title;
            return PartialView(page);
        }

        #region Step_1 - 약관동의

        [HttpPost]
        public JsonResult Terms(string accept)
        {
            AuthUser.JoinMember = new Models.UserInfo();
            AuthUser.JoinMember.TermAgree = Convert.ToBoolean(accept);
            string Code = Utility.Encrypt_AES(accept);
            string Step = "2";
            return Json(new { Code, Step });
        }

        #endregion

        #region Step_2 - 이메일 인증
        [HttpPost]
        // 이메일 발송
        public JsonResult PushCode(string email)
        {
            // 기존 회원인지 체크


            // 인증코드 생성
            string passCode = Utility.RndString8();

            //발신자 및 수신자 메일 설정
            string title = "마이플랫폼 | 회원가입";
            string content = string.Join("\n", Utility.ReadAllText(Server.MapPath("~/Resource/Text/PassCode.txt")));
            content = string.Format(content, email, passCode);

            string resultCode = "", Message = "";
            string result = new Smtp().SendMail(title, content, email);
            if (result == "ok")
            {
                var db_result = new Account().PushCode(Utility.Encrypt_AES(email), passCode);
                if (db_result.IsAny()) resultCode = "0";
            }
            else
            {
                resultCode = "-1";
                Message = result;
            }
            return Json(new { resultCode, Message });
        }

        [HttpPost]
        // 이메일 인증
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

        #endregion

        #region Step_3 - 비밀번호 설정

        [HttpPost]
        public JsonResult UserPassword(string pw)
        {
            AuthUser.JoinMember.User_PW = pw;
            return Json(new { resultCode = "0", Message = "4" });
        }

        #endregion

        #region Step_4 - 프로필설정

        [HttpPost]
        public JsonResult UserProfile(string name, string tel, string gender)
        {
            AuthUser.JoinMember.User_Name = name;
            AuthUser.JoinMember.Phone = tel;
            AuthUser.JoinMember.Gender = gender;
            AuthUser.JoinMember.MemberType = "C";

            // 회원가입
            var result = new Account().RegisterMember(AuthUser.JoinMember);
            if (result.IsAny())
            {
                AuthUser.JoinMember = null;
            }

            ViewBag.Email = AuthUser.JoinMember.User_ID;
            ViewBag.Name = AuthUser.JoinMember.User_Name;
            return Json(new { resultCode = "0", Message = "5" });
        }

        #endregion
    }
}