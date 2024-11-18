using DataCrawling_Web.BSL.Attributes;
using DataCrawling_Web.BSL.Authentication;
using DataCrawling_Web.DSL.Admin;
using DataCrawling_Web.Models;
using DataCrawling_Web.Models.Admin;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace DataCrawling_Web.Controllers.Admin
{
    public class AdminController : Controller
    {
        [CustomOutputCache(Duration = 60 * 20)]
        public ActionResult Index()
        {
            //if (string.IsNullOrEmpty(AuthUser.M_ID) || Utility.Decrypt_AES(AuthUser.M_ID) != "lasidos@naver.com") return Content(Commons.AlertMessage("접근권한이 없습니다.", "/"));
            return Redirect("/Admin/Code/CodeMng");
        }

        [Route("Admin/Content")]
        [HttpGet]
        public new ActionResult Content(string idx = "0", string status = "new")
        {
            IEnumerable<ContentInfoModel> vm = null;
            if (Session["ContentInfoModel"] != null && Session["ContentInfoModel"].ToString() != "")
            {
                vm = JsonConvert.DeserializeObject<IEnumerable<ContentInfoModel>>(Session["ContentInfoModel"].ToString());
            }
            if (vm == null)
            {
                vm = new Contents().GetContents();
                Session["ContentInfoModel"] = JsonConvert.SerializeObject(vm);
            }
            ContentInfoModel item = vm.Where(p => p.IDX.ToString() == idx).FirstOrDefault();

            string title = "";
            switch (status.ToLower())
            {
                case "view":
                    title = "상단컨텐츠(module_ty) - 미리보기";
                    break;
                case "new":
                    title = "상단컨텐츠(module_ty) - 신규등록";
                    break;
                case "edit":
                    title = "상단컨텐츠(module_ty) - 수정";
                    break;
            }
            ViewBag.Status = status;
            ViewBag.Title = title;
            return View(item);
        }

        [Route("Admin/Reg")]
        [HttpPost]
        public JsonResult Reg(string cate, string title, string content)
        {
            new Contents().USP_ADMIN_CONTENTS_IU(-1, 0, 0, "", title, content, 1, AuthUser.M_ID);
            return Json(new { success = true });
        }
    }
}