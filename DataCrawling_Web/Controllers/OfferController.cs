using DataCrawling_Web.BSL.Authentication;
using DataCrawling_Web.BSL.Common;
using DataCrawling_Web.DSL.Account;
using DataCrawling_Web.DSL.Offer;
using DataCrawling_Web.Models.Files;
using Microsoft.IdentityModel.Tokens;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using static System.Windows.Forms.AxHost;
using Utility = DataCrawling_Web.BSL.Common.Utility;

namespace DataCrawling_Web.Controllers
{
    public class OfferController : Controller
    {
        #region 리스트 페이지

        public ActionResult List(string menu)
        {
            string subject = string.Empty, type = string.Empty;
            switch (menu)
            {
                case "scrap":
                    type = "S";
                    subject = "스크랩핑";
                    break;
                case "prs":
                    type = "P";
                    subject = "웹·업무 자동화";
                    break;
            }
            if (string.IsNullOrEmpty(subject)) return Content(Commons.AlertMessage("잘못된 접근입니다."));

            ViewBag.State = "접수대기";
            ViewBag.Subject = subject;
            ViewBag.type = type;
            return View();
        }

        [HttpPost]
        public ActionResult GetTabData(string Tab)
        {
            string state = string.Empty;
            switch (Tab)
            {
                case "0":
                    state = "접수대기";
                    break;
                case "1":
                    state = "진행중";
                    break;
                case "2":
                    state = "완료";
                    break;
            }
            ViewBag.State = state;
            return PartialView("~/Views/Offer/_PartialView/_ListItem.cshtml");
        }

        #endregion

        #region 요청서 페이지

        public ActionResult Terms(string type)
        {
            string title = string.Empty;
            switch (type)
            {
                case "S":
                    title = "스크랩핑";
                    break;
                case "P":
                    title = "웹·업무 자동화";
                    break;
            }
            string termsofservice = string.Join("\n", Utility.ReadAllText(Server.MapPath("~/Resource/Text/termsofservice.txt")));
            string privacy = string.Join("\n", Utility.ReadAllText(Server.MapPath("~/Resource/Text/privacy.txt")));
            ViewBag.type = type;
            ViewBag.title = title;
            ViewBag.termsofservice = termsofservice;
            ViewBag.privacy = privacy;

            return View();
        }

        public new ActionResult Request()
        {
            FilePathGenerate.FileUploadList.Clear();

            if (System.Web.HttpContext.Current.Session["Term"] == null || System.Web.HttpContext.Current.Session["type"] == null)
            {
                return Redirect("/Offer/List");
            }

            ViewBag.type = System.Web.HttpContext.Current.Session["type"];
            var result = new OfferSvc().GetSelectData(ViewBag.type);
            
            return View(result);
        }

        [HttpPost]
        public string functionname(string Term, string type)
        {
            System.Web.HttpContext.Current.Session["Term"] = Term;
            System.Web.HttpContext.Current.Session["type"] = type;
            return "ok";
        }

        public ActionResult GetViewData()
        {
            return PartialView("~/Views/Offer/_PartialView/_OrderItem.cshtml", FilePathGenerate.FileUploadList);
        }

        #endregion

        #region 요청완료 페이지

        public ActionResult Completed()
        {
            ViewBag.type = System.Web.HttpContext.Current.Session["type"];
            if (string.IsNullOrEmpty(ViewBag.type)) return Content(Commons.AlertMessage("잘못된 접근입니다."));
            System.Web.HttpContext.Current.Session["Term"] = null;
            System.Web.HttpContext.Current.Session["type"] = null;
            return View();
        }

        #endregion
    }
}