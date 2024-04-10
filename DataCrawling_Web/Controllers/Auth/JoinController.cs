using DataCrawling_Web.BSL.Authentication;
using DataCrawling_Web.BSL.Common;
using DataCrawling_Web.BSL.Extentions;
using DataCrawling_Web.BSL.Smtp;
using DataCrawling_Web.DSL.Account;
using System;
using System.Linq;
using System.Web.Mvc;

namespace DataCrawling_Web.Controllers.Auth
{
    public class JoinController : BaseController
    {
        public ActionResult Regist()
        {
            ViewBag.Title = "회원가입";
            return View("~/Views/Auth/Join/Regist.cshtml");
        }

        public ActionResult Step(string Step)
        {
            string page = string.Empty;
            switch (Step)
            {
                case "1":
                    page = "~/Views/Auth/Join/_Regist_Step1.cshtml";
                    break;
                case "2":
                    page = "~/Views/Auth/Join/_Regist_Step2.cshtml";
                    break;
                case "3":
                    page = "~/Views/Auth/Join/_Regist_Step3.cshtml";
                    break;
                case "4":
                    page = "~/Views/Auth/Join/_Regist_Step4.cshtml";
                    break;
                case "5":
                    page = "~/Views/Auth/Join/_Regist_Step5.cshtml";
                    ViewBag.Email = AuthUser.JoinMember.User_ID;
                    ViewBag.Name = AuthUser.JoinMember.User_Name;
                    AuthUser.JoinMember = null;
                    break;
            }
            ViewBag.Step = Step;
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
            string resultCode = "", Message = "";

            // 기존 회원인지 체크
            if (new Account().CheckExist(Utility.Encrypt_AES(email)).IsAny())
            {
                return Json(new { resultCode = "-1", Message = "이미 가입된 이메일 계쩡입니다.\n비밀번호 찾기를 이용해주세요." });
            }

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
            string resultCode = "", Message = "";
            AuthUser.JoinMember.User_Name = name;
            AuthUser.JoinMember.Phone = tel;
            AuthUser.JoinMember.Gender = gender;
            AuthUser.JoinMember.MemberType = "C";

            // 회원가입
            var result = new Account().RegisterMember(AuthUser.JoinMember);
            if (result.IsAny())
            {
                resultCode = "0";
                Message = "5";
            }
            else
            {
                resultCode = "-1";
                Message = "회원가입에 실패하였습니다.\n문제가 지속될시 관리자에게 문의바랍니다.";
            }

            return Json(new { resultCode, Message });
        }

        #endregion
    }
}