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
        #region 회원관리

        public ActionResult Management()
        {
            GroupUserViewModel vm = new GroupUserViewModel
            {
                GroupInfo = this.GetGroupInfo(),
                PagingInfo = new PagingInfo()
            };

            return View("~/Views/Admin/Member/Management.cshtml", vm);
        }

        [HttpPost]
        [Route("Member/GetGroupUser")]
        public ActionResult GetGroupUser(int GROUP_ID, int Page = 1, string SearchTxt = "")
        {
            ViewBag.TypePage = "Management";
            GroupUserViewModel groupUser = GroupUserViewModel(GROUP_ID, Page, SearchTxt);

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

        #endregion

        #region 그룹 권한

        public ActionResult Groups()
        {
            var vm = this.GetGroupInfo();
            return View("~/Views/Admin/Member/Groups.cshtml", vm);
        }

        public ActionResult GroupSet(int GROUP_ID)
        {
            ViewBag.GROUP_ID = GROUP_ID;
            var vm = new Member().USP_GROUP_INFO_S();
            return View("~/Views/Admin/Member/GroupSet.cshtml", vm);
        }

        [HttpPost]
        [Route("Member/ManageGroup")]
        public JsonResult ManageGroup(string Action, string G_NAME, string G_DESC, int G_IDX = 0)
        {
            string msg = string.Empty;
            switch (Action)
            {
                case "Add":
                    new Member().USP_GROUP_INFO_IU(G_NAME, G_DESC);
                    msg = "그룹이 생성되었습니다.";
                    break;
                case "Edit":
                    new Member().USP_GROUP_INFO_IU(G_NAME, G_DESC, G_IDX);
                    msg = "그룹이 수정되었습니다.";
                    break;
                case "Delete":
                    new Member().USP_GROUP_INFO_D(G_IDX);
                    msg = "그룹이 삭제되었습니다.";
                    break;
            }

            return Json(new { success = true, msg });
        }

        private IEnumerable<GroupInfoModel> GetGroupInfo()
        {
            IEnumerable<GroupInfoModel> groupInfos;
            if (Session["GroupInfo"] == null)
            {
                groupInfos = new Member().USP_GROUP_INFO_S();
                Session["GroupInfo"] = groupInfos;
                Session.Timeout = 30;
            }
            else groupInfos = (IEnumerable<GroupInfoModel>)Session["GroupInfo"];
            return groupInfos;
        }

        #endregion

        #region 사용자별 개별권한

        public ActionResult Authority()
        {
            GroupUserViewModel vm = new GroupUserViewModel
            {
                GroupInfo = this.GetGroupInfo(),
                PagingInfo = new PagingInfo()
            };

            return View("~/Views/Admin/Member/Authority.cshtml", vm);
        }

        [HttpPost]
        [Route("Member/GetGroupPersonAuthority")]
        public ActionResult GetGroupPersonAuthority(int Page = 1, string SearchTxt = "")
        {
            ViewBag.TypePage = "Authority";
            GroupUserViewModel groupUser = Individual_Authority_ViewModel(Page, SearchTxt);

            return PartialView("~/Views/Admin/Shared/_UserList.cshtml", groupUser);
        }

        #endregion

        private GroupUserViewModel GroupUserViewModel(int GROUP_ID, int Page = 1, string SearchTxt = "")
        {
            GroupUserViewModel groupUser = new GroupUserViewModel
            {
                GroupInfo = this.GetGroupInfo()
            };

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
                .Where(i => i.User_Name.Contains(SearchTxt) || i.User_ID.Contains(SearchTxt) || i.Phone.Contains(SearchTxt))
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

            return groupUser;
        }

        private GroupUserViewModel Individual_Authority_ViewModel(int Page = 1, string SearchTxt = "")
        {
            GroupUserViewModel groupUser = new GroupUserViewModel
            {
                GroupInfo = this.GetGroupInfo()
            };

            IEnumerable<IndividualAuthorityModel> vm = new Member().USP_Individual_Authority_S();
            groupUser.Individuals = vm.Select(s => new IndividualAuthorityModel()
            {
                OrderNo = s.OrderNo,
                User_ID = Utility.Decrypt_AES(s.User_ID),
                ROLE_ID = s.ROLE_ID,
                Visible_Stat = s.Visible_Stat,
                Select_Stat = s.Select_Stat, 
                Edit_Authority = s.Edit_Authority
            });

            #region 페이징 필수

            int pageSize = 50;
            var totalItems = groupUser.Individuals.Count(); // 예시: 총 아이템 수
            groupUser.Individuals = groupUser.Individuals
                .Where(i => i.User_ID.Contains(SearchTxt))
                          .OrderBy(i => i.ROLE_ID)
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

            return groupUser;
        }
    }
}