﻿using DataCrawling_Web.Models.Commons;
using System;
using System.Collections.Generic;

namespace DataCrawling_Web.Models.Admin
{
    public class GroupUserViewModel
    {
        // 그룹정보
        public IEnumerable<GroupInfoModel> GroupInfo { get; set; }

        // 그룹 사용자
        public IEnumerable<GroupUserModel> GroupUsers { get; set; }
        public IEnumerable<GroupAuthorityUserModel> GroupAuthorityUsers { get; set; }
        public IEnumerable<IndividualAuthorityModel> Individuals { get; set; }
        public PagingInfo PagingInfo { get; set; } = null;
    }

    public class GroupInfoModel
    {
        public int OrderNo { get; set; }
        public int GROUP_ID { get; set; }
        public string GROUP_NAME { get; set; }
        public string DESCRIPTION { get; set; }
        public int ROLE_ID { get; set; }
        public int Menu_Idx { get; set; }
        public string Menu_Name { get; set; }
        public int Visible_Stat { get; set; }
        public int Select_Stat { get; set; }
        public int Edit_Authority { get; set; }
        public DateTime REG_DT { get; set; }
        public DateTime EDIT_DT { get; set; }
        public string REGISTER { get; set; }
    }

    public class GroupAuthorityUserModel
    {
        public int OrderNo { get; set; }
        public string User_ID { get; set; }
        public string User_Name { get; set; }
        public int ROLE_ID { get; set; }
        public int Menu_Idx { get; set; }
        public string Menu_Name { get; set; }
        public int Visible_Stat { get; set; }
        public int Select_Stat { get; set; }
        public int Edit_Authority { get; set; }
        public int Menu_Type { get; set; }
    }

    public class GroupUserModel
    {
        public int IDX { get; set; }
        public int OrderNo { get; set; }
        public string User_ID { get; set; }
        public string User_Name { get; set; }
        public string Phone { get; set; }
        public string Gender { get; set; }
        public DateTime LastLoginDate { get; set; }
        public DateTime RegistDate { get; set; }
        public string LastLoginDateST { get; set; }
        public string RegistDateST { get; set; }
        public int GROUP_ID { get; set; }
        public string GROUP_NAME { get; set; }
        public string DESCRIPTION { get; set; }
    }

    public class IndividualAuthorityModel
    {
        public int IDX { get; set; }
        public int OrderNo { get; set; }
        public string Menu_Name { get; set; }
        public string User_ID { get; set; }
        public string User_Name { get; set; }
        public int ROLE_ID { get; set; }
        public int Visible_Stat { get; set; }
        public int Select_Stat { get; set; }
        public int Edit_Authority { get; set; }
    }

    public class RoleModel
    {
        public string role { get; set; }
        public string view { get; set; }
        public string sel { get; set; }
        public string edit { get; set; }
    }
}