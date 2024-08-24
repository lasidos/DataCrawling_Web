using DataCrawling_Web.BSL.Authentication;
using DataCrawling_Web.BSL.Code;
using DataCrawling_Web.DSL.Account;
using DataCrawling_Web.DSL.Admin;
using DataCrawling_Web.Models.Admin;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DataCrawling_Web.Controllers.Admin
{
    public class CodeController : Controller
    {
        public ActionResult CodeMng(int MenuLvl = 0)
        {
            var vm = new Code().USP_MENU_CODE_S();
            return View("~/Views/Admin/Code/CodeMng.cshtml", vm);
        }

        [HttpPost]
        [Route("Code/GetMenu")]
        public string GetMenu(int Menu_Type, int Parent_Id)
        {
            IEnumerable<W_MenuModel> vm = W_Menu.GetMenu(Parent_Id, Menu_Type);
            return JsonConvert.SerializeObject(vm);
        }

        [HttpPost]
        [Route("Code/AddMenu")]
        public JsonResult AddCodes(int menuType, int idx, int parent_Id, string title, string url, int order, int login, int visible)
        {
            new Code().USP_MENU_IU(new W_MenuModel()
            {
                Menu_Idx = idx,
                Menu_Name = title,
                Menu_URL = url,
                Order_No = order,
                Login_Stat = login,
                Display_Stat = visible,
                M_Id = AuthUser.M_ID,
                Parent_Id = parent_Id,
                Menu_Type = menuType
            });

            string code = "0", msg = "ok";
            return Json(new { code, msg });
        }

        [HttpPost]
        [Route("Code/DeleteMenu")]
        public JsonResult DeleteMenu(int idx)
        {
            new Code().USP_MENU_D(idx);

            string code = "0", msg = "ok";
            return Json(new { code, msg });
        }
    }
}