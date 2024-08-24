using DataCrawling_Web.BSL.Common;
using DataCrawling_Web.DSL.Admin;
using DataCrawling_Web.Models.Admin;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace DataCrawling_Web.Controllers.Admin
{
    public class MemberController : Controller
    {
        public ActionResult Management()
        {
            var vm = new Member().USP_GROUP_INFO_S();
            return View("~/Views/Admin/Member/Management.cshtml", vm);
        }

        public ActionResult Authority()
        {
            return View("~/Views/Admin/Member/Authority.cshtml");
        }

        public ActionResult Groups()
        {
            return View("~/Views/Admin/Member/Groups.cshtml");
        }

        [HttpPost]
        [Route("Member/GetGroupUser")]
        public string GetGroupUser(int GROUP_ID)
        {
            IEnumerable<GroupUserModel> vm = new Member().USP_GROUP_USER_S(GROUP_ID);
            var ViewModel = vm.Select(s => new GroupUserModel()
            {
                OrderNo = s.OrderNo,
                IDX = s.IDX,
                User_ID = Utility.Decrypt_AES(s.User_ID),
                User_Name = Utility.SetMask(Utility.Decrypt_AES(s.User_Name), 1),
                Phone = Utility.SetPhoneNumMask(Utility.Decrypt_AES(s.Phone)),
                Gender = Utility.Decrypt_AES(s.Gender) == "Male" ? "남" : "여",
                GROUP_NAME = s.GROUP_NAME,
                DESCRIPTION = s.DESCRIPTION,
                LastLoginDateST = s.LastLoginDate.ToString("yyyy년 MM월 dd일"),
                RegistDateST = s.RegistDate.ToString("yyyy년 MM월 dd일")
            });
            return JsonConvert.SerializeObject(ViewModel);
        }

        public ActionResult GroupUser()
        {
            return View("~/Views/Admin/Member/GroupUser.cshtml");
        }
    }
}