using DataCrawling_Web.BSL.Attributes;
using DataCrawling_Web.BSL.Authentication;
using DataCrawling_Web.BSL.Code;
using DataCrawling_Web.BSL.Common;
using DataCrawling_Web.DSL.Offer;
using DataCrawling_Web.Models.Files;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Utility = DataCrawling_Web.BSL.Common.Utility;

namespace DataCrawling_Web.Controllers
{
    public class OfferController : Controller
    {
        #region 리스트 페이지

        [LoginCheck(Url =  "/Offer/List?menu=scrap")]
        public ActionResult List(string menu)
        {
            if (W_Menu.GetMenuIdx(Request.Url.PathAndQuery).Login_Stat == 1 && string.IsNullOrEmpty(AuthUser.M_ID))
            {
                return Content(JSBuilder.ConfirmMoveCancel(@"로그인이 필요한 서비스입니다..\n로그인페이지로 이동하시겠습니까?", 
                    "/Auth/Login/Nid_Login", "/"));
            }

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

            List<OfferViewModel> vm = GetData(type, "0,1");

            ViewBag.Subject = subject;
            ViewBag.type = type;
            return View(vm);
        }

        [HttpPost]
        public ActionResult GetTabData(string menu, string Tab)
        {
            string state = string.Empty;
            string _tab = string.Empty;
            switch (Tab)
            {
                case "0":
                    state = "접수대기";
                    _tab = "0,1";
                    break;
                case "1":
                    state = "진행중";
                    _tab = "2";
                    break;
                case "2":
                    state = "완료";
                    _tab = "3";
                    break;
            }

            List<OfferViewModel> vm = GetData(menu == "scrap" ? "S" : "P", _tab);
            ViewBag.State = state;
            return PartialView("~/Views/Offer/_PartialView/_ListItem.cshtml", vm);
        }

        private List<OfferViewModel> GetData(string type, string tab)
        {
            var result = new OfferSvc().USP_RegistOffer_S(type, tab, AuthUser.M_ID);
            List<string> idxs = result.GroupBy(s => s.IDX).Select(p => p.Key).ToList();
            List<OfferViewModel> vm = new List<OfferViewModel>();
            foreach (var item in result)
            {
                if (vm.Where(s => s.IDX == item.IDX).Count() > 0)
                {
                    if (!string.IsNullOrEmpty(item.Origin_FileName))
                    {
                        vm.Where(s => s.IDX == item.IDX).First().FileList.Add(new FileListModel
                        {
                            DOC_TYPE = item.DOC_TYPE,
                            Origin_FileName = item.Origin_FileName
                        });
                    }
                }
                else
                {
                    var stat = "";
                    switch (item.PROGRESS)
                    {
                        case 0:
                            stat = "접수대기";
                            break;
                        case 1:
                            stat = "검토중";
                            break;
                        case 2:
                            stat = "개발진행중";
                            break;
                        case 3:
                            stat = "개발완료";
                            break;
                    }

                    FileListModel fileListModel = null;
                    if (!string.IsNullOrEmpty(item.Origin_FileName))
                    {
                        fileListModel = new FileListModel
                        {
                            DOC_TYPE = item.DOC_TYPE,
                            Origin_FileName = item.Origin_FileName
                        };
                    }

                    vm.Add(new OfferViewModel()
                    {
                        IDX = item.IDX,
                        O_TYPE = item.O_TYPE,
                        STAT_TYPE = item.STAT_TYPE,
                        PLAN_TYPE = item.PLAN_TYPE,
                        PERIOD_TYPE = item.PERIOD_TYPE,
                        U_URL = item.U_URL,
                        CONTENT = item.CONTENT,
                        ETC = item.ETC,
                        PROGRESS_Stat = stat,
                        FileList = fileListModel != null ? new List<FileListModel>() { fileListModel } : new List<FileListModel>()
                    });
                }
            }

            return vm;
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

        public new ActionResult RequestOffer()
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