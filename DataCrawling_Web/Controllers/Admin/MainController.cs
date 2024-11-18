using DataCrawling_Web.BSL.Admin;
using DataCrawling_Web.DSL.Account;
using DataCrawling_Web.DSL.Admin;
using DataCrawling_Web.Models.Admin;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace DataCrawling_Web.Controllers.Admin
{
    enum ContentCode
    {
        Main = 1,
        Main_Help = 2
    }

    public class MainController : Controller
    {
        public ActionResult Index()
        {
            return Redirect("/Admin/Main/item");
        }

        public ActionResult item()
        {
            if (Session["MenuInfo"] == null)
            {
                var temp = new Contents().USP_ADMIN_NOTICE_MENU_S();
                Session["MenuInfo"] = JsonConvert.SerializeObject(temp);
            }
            var vm = JsonConvert.DeserializeObject<IEnumerable<MenuModel>>(Session["MenuInfo"].ToString());

            //var vm = new Contents().GetContents().OrderByDescending(s => s.E_DATE);
            //Session["ContentInfoModel"] = JsonConvert.SerializeObject(vm);

            return View("~/Views/Admin/Main/item.cshtml", vm);
        }

        [HttpPost]
        [Route("Main/GetSector")]
        public string GetSector(int idx)
        {
            if (Session["MenuInfo"] == null)
            {
                var temp = new Contents().USP_ADMIN_NOTICE_MENU_S();
                Session["MenuInfo"] = JsonConvert.SerializeObject(temp);
            }
            var vm = JsonConvert.DeserializeObject<IEnumerable<MenuModel>>(Session["MenuInfo"].ToString());
            vm = vm.Where(p => p.Menu_Idx == idx);

            return JsonConvert.SerializeObject(vm);
        }

        [HttpPost]
        [Route("Main/GetContent")]
        public ActionResult GetContent(int m_idx, int code)
        {
            var vm = new Contents().GetContents().OrderByDescending(s => s.E_DATE);
            Session["ContentInfoModel"] = JsonConvert.SerializeObject(vm);

            return PartialView("~/Views/Admin/Shared/_UserList.cshtml", vm);
        }
    }
}