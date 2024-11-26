using DataCrawling_Web.BSL.Common;
using DataCrawling_Web.BSL.Extentions;
using DataCrawling_Web.DSL.Data;
using DataCrawling_Web.Models.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace DataCrawling_Web.Controllers.Admin
{
    public class ServiceController : Controller
    {
        // GET: Service
        public ActionResult API()
        {
            var vm = new DataSvc().USP_DATALIST_S();
            Session["DataInfoView_Model"] = JsonConvert.SerializeObject(vm);
            return View("~/Views/Admin/Service/API.cshtml", vm);
        }

        public ActionResult Automation()
        {
            return View("~/Views/Admin/Service/Automation.cshtml");
        }

        public ActionResult item(string type, string view, string idx)
        {
            ViewBag.View = view;
            DataInfoEditView vm = new DataInfoEditView();
            vm.Item_Sector = new List<comboboxMpdel>()
            {
                new comboboxMpdel { Idx = 1, Name = "교통및물류 - 교통" },
                new comboboxMpdel { Idx = 2, Name = "이베이" }
            };
            vm.Item_D_TYPE = new List<comboboxMpdel>()
            {
                new comboboxMpdel { Idx = 1, Name = "API" },
                new comboboxMpdel { Idx = 2, Name = "File" }
            };
            vm.Item_R_TYPE = new List<comboboxMpdel>()
            {
                new comboboxMpdel { Idx = 1, Name = "JSON" },
                new comboboxMpdel { Idx = 2, Name = "XML" }
            };
            vm.Item_Data_TYPE = new List<comboboxMpdel>()
            {
                new comboboxMpdel { Idx = 1, Name = "-" },
                new comboboxMpdel { Idx = 2, Name = "String" },
                new comboboxMpdel { Idx = 3, Name = "Integer" },
                new comboboxMpdel { Idx = 4, Name = "Bool" },
                new comboboxMpdel { Idx = 5, Name = "DateTime" }
            };
            vm.Item_Currect_TYPE = new List<comboboxMpdel>()
            {
                new comboboxMpdel { Idx = 1, Name = "Y" },
                new comboboxMpdel { Idx = 2, Name = "N" }
            };

            if (Session["DataInfoView_Model"] != null && Session["DataInfoView_Model"].ToString() != "")
            {
                vm.DataInfo = JsonConvert.DeserializeObject<IEnumerable<DataInfoModel>>(Session["DataInfoView_Model"].ToString());
            }
            else vm.DataInfo = new DataSvc().USP_DATALIST_S();

            if (string.IsNullOrEmpty(idx) || !vm.DataInfo.IsAny())
            {
                string url = "";
                if (type == "api") url = "/Admin/Service/API";
                else url = "/Admin/Service/Automation";

                return Content(Commons.AlertMessage("잘못된 접근입니다.", url));
            }
            else
            {
                vm.SelInfo = vm.DataInfo.Where(s => s.DATA_IDX.ToString() == idx).FirstOrDefault();
                vm.Item_Sector_Idx = vm.Item_Sector.FindIndex(s => s.Name == vm.SelInfo.SECTOR) + 1;
            }

            var api_Models = new DataSvc().USP_DATALIST_API_S(idx);
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

            return View("~/Views/Admin/Service/item.cshtml", vm);
        }
    }
}