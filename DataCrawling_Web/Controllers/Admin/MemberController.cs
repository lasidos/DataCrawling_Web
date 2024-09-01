using DataCrawling_Web.BSL.Authentication;
using DataCrawling_Web.BSL.Common;
using DataCrawling_Web.DSL.Admin;
using DataCrawling_Web.Models.Admin;
using DataCrawling_Web.Models.Commons;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Web.Mvc;

namespace DataCrawling_Web.Controllers.Admin
{
    public class MemberController : Controller
    {
        public ActionResult Management()
        {
            GroupUserViewModel vm = new GroupUserViewModel();
            vm.GroupInfo = new Member().USP_GROUP_INFO_S();
            Session["GroupInfo"] = vm.GroupInfo;
            Session.Timeout = 30;

            vm.PagingInfo = new PagingInfo();
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
        public ActionResult GetGroupUser(int GROUP_ID, int Page = 1)
        {
            GroupUserViewModel groupUser = new GroupUserViewModel();
            groupUser.GroupInfo = (IEnumerable<GroupInfoModel>)Session["GroupInfo"];
            IEnumerable<GroupUserModel> vm = new Member().USP_GROUP_USER_S(GROUP_ID);
            groupUser.GroupUsers = vm.Select(s => new GroupUserModel()
            {
                OrderNo = s.OrderNo,
                IDX = s.IDX,
                User_ID = Utility.Decrypt_AES(s.User_ID),
                User_Name = Utility.SetMask(Utility.Decrypt_AES(s.User_Name), 1),
                Phone = Utility.SetPhoneNumMask(Utility.Decrypt_AES(s.Phone)),
                Gender = Utility.Decrypt_AES(s.Gender) == "Male" ? "남" : "여",
                GROUP_ID = s.GROUP_ID,
                GROUP_NAME = s.GROUP_NAME,
                DESCRIPTION = s.DESCRIPTION,
                LastLoginDateST = s.LastLoginDate.ToString("yyyy년 MM월 dd일"),
                RegistDateST = s.RegistDate.ToString("yyyy년 MM월 dd일")
            });

            #region 페이징 필수

            int pageSize = 50;
            var totalItems = groupUser.GroupUsers.Count(); // 예시: 총 아이템 수
            groupUser.GroupUsers = groupUser.GroupUsers
                          .OrderBy(i => i.GROUP_ID).ThenBy(i => i.IDX)
                          .Skip((Page - 1) * pageSize)
                          .Take(pageSize)
                          .ToList();

            groupUser.PagingInfo = new PagingInfo
            {
                CurrentPage = Page,
                ItemsPerPage = pageSize,
                TotalItems = totalItems
            };

            #endregion

            return PartialView("~/Views/Admin/Shared/_UserList.cshtml", groupUser);
        }

        [HttpPost]
        [Route("Member/SetAuthority")]
        public JsonResult SetAuthority(string IDX, string GROUP_ID, string M_IDX)
        {
            var vm = new Member().USP_GROUP_USER_IU(IDX, GROUP_ID, M_IDX);
            return Json(new { success = true, data = vm.ToString() });
        }

        [HttpPost]
        [Route("Member/ReSetPW")]
        public JsonResult ReSetPW(string M_IDX)
        {
            new Member().USP_ReSetPW(M_IDX);
            return Json(new { success = true });
        }

        public ActionResult GroupSet(int GROUP_ID)
        {
            ViewBag.GROUP_ID = GROUP_ID;
            var vm = new Member().USP_GROUP_INFO_S();
            return View("~/Views/Admin/Member/GroupSet.cshtml", vm);
        }
    }
}