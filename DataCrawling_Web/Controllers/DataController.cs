using DataCrawling_Web.BSL.Attributes;
using DataCrawling_Web.BSL.Authentication;
using DataCrawling_Web.BSL.Code;
using DataCrawling_Web.BSL.Common;
using DataCrawling_Web.BSL.Extentions;
using DataCrawling_Web.DSL.Data;
using DataCrawling_Web.Models;
using DataCrawling_Web.Models.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace DataCrawling_Web.Controllers
{
    public class DataController : Controller
    {
        //[CustomOutputCache(Duration = 60 * 20)]
        [LoginCheck(Url = "/Data/ApiList")]
        public ActionResult ApiList()
        {

            DataInfoViewModel model = new DataInfoViewModel();
            model.Tab = 1;
            model.DataInfo = new DataSvc().USP_DATALIST_S();
            Session["DataInfo"] = model.DataInfo;

            UserInfo userInfo = new UserInfo()
            {
                User_ID = "lasidos",
                MemberType = "M",
                User_Name = "유지혁"
            };

            return View(model);
        }

        public ActionResult FileData(string A_No)
        {
            IEnumerable<Authentication_Key_Model> AuthKey = new List<Authentication_Key_Model>();
            if (!string.IsNullOrEmpty(AuthUser.M_ID))
            {
                AuthKey = new DataSvc().USP_Authentication_Key_S(AuthUser.M_ID);
            }

            var result = new DataSvc().USP_DATALIST_S(A_No: A_No);
            if (string.IsNullOrEmpty(A_No) || !result.IsAny()) return Content(Commons.AlertMessage("잘못된 접근입니다.", "/Data/ApiList"));

            DataInfoView_Model vm = new DataInfoView_Model();
            vm.DataInfo = result.FirstOrDefault();

            var api_Models = new DataSvc().USP_DATALIST_API_S(A_No);
            vm.RequestList = api_Models.GroupBy(g => new { g.P_IDX, g.P_NAME_E, g.P_TYPE, g.P_NEED, g.SAMPLE_TXT, g.P_EXPLANE })
                .Select(p => new RequestParameter_Model()
                {
                    P_IDX = p.Key.P_IDX,
                    P_NAME_E = p.Key.P_NAME_E,
                    P_TYPE = p.Key.P_TYPE,
                    P_NEED = p.Key.P_NEED,
                    SAMPLE_TXT = p.Key.SAMPLE_TXT,
                    P_EXPLANE = p.Key.P_EXPLANE
                }).ToList();

            vm.ResponseList = api_Models.GroupBy(g => new { g.R_IDX, g.R_NAME_K, g.R_NAME_E, g.R_TYPE, g.R_EXPLANE })
                .Select(p => new ResponseElement_Model()
                {
                    R_IDX = p.Key.R_IDX,
                    R_NAME_K = p.Key.R_NAME_K,
                    R_NAME_E = p.Key.R_NAME_E,
                    R_TYPE = p.Key.R_TYPE,
                    R_EXPLANE = p.Key.R_EXPLANE
                }).ToList();

            Session["DataInfoView_Model"] = JsonConvert.SerializeObject(vm);
            return View(vm);
        }

        public ActionResult SelTabView(string tab)
        {
            DataInfoView_Model vm = null;
            if (Session["DataInfoView_Model"] != null && Session["DataInfoView_Model"].ToString() != "")
            {
                vm = JsonConvert.DeserializeObject<DataInfoView_Model>(Session["DataInfoView_Model"].ToString());
            }

            ViewBag.tab = tab;
            return PartialView("~/Views/Data/_PartialView/_tabView.cshtml", vm);
        }

        public ActionResult Info(string A_No)
        {
            ViewBag.A_No = A_No;
            return View();
        }
    }
}