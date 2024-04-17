using DataCrawling_Web.BSL.Authentication;
using DataCrawling_Web.BSL.Common;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using static System.Windows.Forms.AxHost;

namespace DataCrawling_Web.Controllers
{
    public class OfferController : Controller
    {
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
            if (subject == "") return Content(Commons.AlertMessage("잘못된 접근입니다."));

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
            ViewBag.title = title;
            ViewBag.termsofservice = termsofservice;
            ViewBag.privacy = privacy;
            return View();
        }

        public new ActionResult Request()
        {
            if (System.Web.HttpContext.Current.Session["Term"] == null)
            {
                return Redirect("/Offer/List");
            }
            return View();
        }

        [HttpPost]
        public string functionname(string Term)
        {
            System.Web.HttpContext.Current.Session["Term"] = Term;
            return "ok";
        }
    }
}