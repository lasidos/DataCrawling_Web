using DataCrawling_Web.BSL.Authentication;
using DataCrawling_Web.BSL.Code;
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
            GroupUserViewModel vm = this.GetGroup();

            return View("~/Views/Admin/Member/Management.cshtml", vm);
        }

        [HttpPost]
        [Route("Member/GetGroupUser")]
        public ActionResult GetGroupUser(int GROUP_ID, int Page = 1, string SearchTxt = "")
        {
            ViewBag.TypePage = "Management";
            GroupUserViewModel vm = this.GetGroupAndUser(GROUP_ID, Page, SearchTxt);

            return PartialView("~/Views/Admin/Shared/_UserList.cshtml", vm);
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
            GroupUserViewModel vm = this.GetGroup();
            return View("~/Views/Admin/Member/Groups.cshtml", vm);
        }

        [HttpPost]
        [Route("Member/ManageGroup")]
        public JsonResult ManageGroup(string Action, string G_NAME, string G_DESC, int G_IDX = 0)
        {
            string msg = string.Empty;
            switch (Action)
            {
                case "Add":
                    new Member().USP_GROUP_INFO_IU(G_NAME, G_DESC, AuthUser.M_ID);
                    msg = "그룹이 생성되었습니다.";
                    break;
                case "Edit":
                    new Member().USP_GROUP_INFO_IU(G_NAME, G_DESC, AuthUser.M_ID, G_IDX);
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
            GroupUserViewModel vm = GroupUserViewModel(-1);

            return View("~/Views/Admin/Member/Authority.cshtml", vm);
        }

        [HttpPost]
        [Route("Member/SaveUsers")]
        public JsonResult SaveUsers(string[] users)
        {
            string msg = "success";

            string userInfo = string.Join(",", users);
            new Member().USP_Individual_Authority_I(userInfo);

            return Json(new { success = true, msg });
        }

        [HttpPost]
        [Route("Member/AuthorityDelete")]
        public JsonResult AuthorityDelete(int Order)
        {
            string msg = "success";
            new Member().USP_AuthorityDelete(Order);

            return Json(new { success = true, msg });
        }

        #endregion

        private GroupUserViewModel GroupUserAuthorityViewModel(int Category, int MenuIdx, int Page = 1, string SearchTxt = "")
        {
            GroupUserViewModel groupUser = new GroupUserViewModel
            {
                GroupInfo = this.GetGroupInfo()
            };

            IEnumerable<GroupAuthorityUserModel> vm = new Member().USP_GROUP_USER_Authority_S(MenuIdx);
            groupUser.GroupAuthorityUsers = vm.Select(s => new GroupAuthorityUserModel()
            {
                OrderNo = s.OrderNo,
                User_ID = Utility.Decrypt_AES(s.User_ID),
                User_Name = Utility.SetMask(Utility.Decrypt_AES(s.User_Name), 1),
                ROLE_ID = s.ROLE_ID,
                Menu_Idx = s.Menu_Idx,
                Menu_Name = s.Menu_Name,
                Visible_Stat = s.Visible_Stat,
                Select_Stat = s.Select_Stat,
                Edit_Authority = s.Edit_Authority,
                Menu_Type = s.Menu_Type
            }).Where(w => w.Menu_Type == Category);

            #region 페이징 필수

            int pageSize = 50;
            var totalItems = groupUser.GroupAuthorityUsers.Count(); // 예시: 총 아이템 수
            groupUser.GroupAuthorityUsers = groupUser.GroupAuthorityUsers
                .Where(i => i.User_Name.Contains(SearchTxt) || i.User_ID.Contains(SearchTxt))
                          .OrderBy(i => i.Menu_Idx)
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

        private GroupUserViewModel GroupUserViewModel(int GROUP_ID, int Page = 1, string SearchTxt = "")
        {
            IEnumerable<GroupUserModel> vm = new Member().USP_GROUP_USER_S(GROUP_ID);
            GroupUserViewModel groupUser = new GroupUserViewModel
            {
                GroupUsers = vm.Select(s => new GroupUserModel()
                {
                    OrderNo = s.OrderNo,
                    IDX = s.IDX,
                    User_ID = Utility.Decrypt_AES(s.User_ID),
                    User_Name = Utility.Decrypt_AES(s.User_Name),
                    Phone = Utility.Decrypt_AES(s.Phone),
                    Gender = Utility.Decrypt_AES(s.Gender) == "Male" ? "남" : "여",
                    GROUP_ID = s.GROUP_ID,
                    GROUP_NAME = s.GROUP_NAME,
                    DESCRIPTION = s.DESCRIPTION,
                    LastLoginDateST = s.LastLoginDate.ToString("yyyy년 MM월 dd일"),
                    RegistDateST = s.RegistDate.ToString("yyyy년 MM월 dd일")
                }),
                GroupInfo = new Member().ADMIN_GROUP_S()
            };

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

            IEnumerable<IndividualAuthorityModel> vm = new Member().USP_Individual_Authority_S(-1);
            groupUser.Individuals = vm.Select(s => new IndividualAuthorityModel()
            {
                IDX = s.IDX,
                OrderNo = s.OrderNo,
                Menu_Name = s.Menu_Name,
                User_ID = Utility.Decrypt_AES(s.User_ID),
                User_Name = Utility.SetMask(Utility.Decrypt_AES(s.User_Name), 1),
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

        #region 그룹 / 사용자 권한 변경

        public ActionResult GroupAuthority(string m_type, int idx)
        {
            GroupUserViewModel vm = new GroupUserViewModel();

            ViewBag.idx = idx;
            switch (m_type)
            {
                case "group":
                    ViewBag.TypePage = "Group";
                    vm.GroupInfo = new Member().USP_GROUP_INFO_S().Where(s => s.GROUP_ID == idx);
                    break;
                case "auth":
                    ViewBag.TypePage = "Authority";
                    vm.Individuals = new Member().USP_Individual_Authority_S(idx);
                    break;
            }

            return View("~/Views/Admin/Member/GroupAuthority.cshtml", vm);
        }

        [HttpPost]
        [Route("Member/GroupAuthorityJsonUpdate")]
        public JsonResult GroupAuthorityJsonUpdate(string data)
        {
            string mid = AuthUser.M_ID;
            string msg = "success";
            new Member().USP_GroupAuthorityUpdate(data);
            return Json(new { success = true, msg });
        }

        #endregion

        #region BSL

        // 그룹 정보만 가져오기
        private GroupUserViewModel GetGroup()
        {
            GroupUserViewModel vm = new GroupUserViewModel
            {
                GroupInfo = new Member().ADMIN_GROUP_S()
            };

            return vm;
        }

        // 그룹 정보와 사용자 정보 가져오기
        private GroupUserViewModel GetGroupAndUser(int GROUP_ID, int Page = 1, string SearchTxt = "")
        {
            IEnumerable<GroupUserModel> vm = new Member().USP_GROUP_USER_S(GROUP_ID);
            GroupUserViewModel groupUser = new GroupUserViewModel
            {
                GroupUsers = vm.Select(s => new GroupUserModel()
                {
                    OrderNo = s.OrderNo,
                    IDX = s.IDX,
                    User_ID = Utility.Decrypt_AES(s.User_ID),
                    User_Name = Utility.Decrypt_AES(s.User_Name),
                    Phone = Utility.Decrypt_AES(s.Phone),
                    Gender = Utility.Decrypt_AES(s.Gender) == "Male" ? "남" : "여",
                    GROUP_ID = s.GROUP_ID,
                    GROUP_NAME = s.GROUP_NAME,
                    DESCRIPTION = s.DESCRIPTION,
                    LastLoginDateST = s.LastLoginDate.ToString("yyyy년 MM월 dd일"),
                    RegistDateST = s.RegistDate.ToString("yyyy년 MM월 dd일")
                }),
                GroupInfo = new Member().ADMIN_GROUP_S()
            };

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

        #endregion
    }
}