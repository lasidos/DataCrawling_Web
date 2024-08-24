using DataCrawling_Web.BSL.Attributes;
using DataCrawling_Web.DSL.Admin;
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
                vm = new Contents().GetContents().OrderByDescending(s => s.E_DATE);
                Session["ContentInfoModel"] = JsonConvert.SerializeObject(vm);
            }
            ContentInfoModel item = vm.Where(p => p.IDX.ToString() == idx).FirstOrDefault();

            string state = "";
            switch (status.ToLower())
            {
                case "view":
                    state = "상단컨텐츠(module_ty) - 미리보기";
                    break;
                case "new":
                    state = "상단컨텐츠(module_ty) - 신규등록";
                    break;
                case "edit":
                    state = "상단컨텐츠(module_ty) - 수정";
                    break;
            }
            ViewBag.State = state;
            return View(item);
        }
    }
}