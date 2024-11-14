using Dapper;
using DataCrawling_Web.BSL.Common;
using DataCrawling_Web.Models.Admin;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DataCrawling_Web.DSL.Admin
{
    public class Member
    {
        protected readonly string CONN_STR;

        public Member(bool readOnly = false)
        {
            CONN_STR = "Data Source=211.233.51.65;Initial Catalog=mkapi_godohosting_com;User ID=mkapi;Password=akvmfzh1!@;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        }

        /// <summary>
        /// 그룹정보 가져오기
        /// </summary>
        /// <param name="All">1 = 비회원 포함 가져오기</param>
        /// <returns></returns>
        public IEnumerable<GroupInfoModel> ADMIN_GROUP_S(int All = -1)
        {
            var param = new DynamicParameters();
            param.Add("@@All", All);
            using (IDbConnection conn = new SqlConnection(CONN_STR))
            {
                return conn.Query<GroupInfoModel>(param: param, commandType: CommandType.StoredProcedure
                    , sql: "DBO.ADMIN_GROUP_S");
            }
        }





        public IEnumerable<GroupInfoModel> USP_GROUP_INFO_S(int M_Idx = -1)
        {
            var param = new DynamicParameters();
            param.Add("@M_Idx", M_Idx);
            using (IDbConnection conn = new SqlConnection(CONN_STR))
            {
                return conn.Query<GroupInfoModel>(param: param, commandType: CommandType.StoredProcedure
                    , sql: "DBO.USP_GROUP_INFO_S");
            }
        }

        public IEnumerable<GroupInfoModel> USP_GROUP_INFO_IU(string G_NAME, string G_DESC, string M_ID, int G_IDX = -1)
        {
            var param = new DynamicParameters();
            param.Add("@G_IDX", G_IDX);
            param.Add("@G_NAME", G_NAME);
            param.Add("@G_DESC", G_DESC);
            param.Add("@M_ID", M_ID);
            using (IDbConnection conn = new SqlConnection(CONN_STR))
            {
                return conn.Query<GroupInfoModel>(param: param, commandType: CommandType.StoredProcedure
                    , sql: "DBO.USP_GROUP_INFO_IU");
            }
        }

        public IEnumerable<GroupInfoModel> USP_GROUP_INFO_D(int G_IDX)
        {
            var param = new DynamicParameters();
            param.Add("@G_IDX", G_IDX);
            using (IDbConnection conn = new SqlConnection(CONN_STR))
            {
                return conn.Query<GroupInfoModel>(param: param, commandType: CommandType.StoredProcedure
                    , sql: "DBO.USP_GROUP_INFO_D");
            }
        }

        public int USP_USER_GROUP_INFO_S(string M_ID)
        {
            var param = new DynamicParameters();
            param.Add("@M_ID", M_ID);
            using (IDbConnection conn = new SqlConnection(CONN_STR))
            {
                return conn.QuerySingle<int>(param: param, commandType: CommandType.StoredProcedure
                    , sql: "DBO.USP_USER_GROUP_INFO_S");
            }
        }

        public IEnumerable<GroupUserModel> USP_GROUP_USER_S(int GROUP_ID = -1)
        {
            var param = new DynamicParameters();
            param.Add("@GROUP_ID", GROUP_ID);
            using (IDbConnection conn = new SqlConnection(CONN_STR))
            {
                return conn.Query<GroupUserModel>(param: param, commandType: CommandType.StoredProcedure
                    , sql: "DBO.USP_GROUP_USER_S");
            }
        }

        public IEnumerable<GroupAuthorityUserModel> USP_GROUP_USER_Authority_S(int M_Idx = -1)
        {
            var param = new DynamicParameters();
            param.Add("@M_Idx", M_Idx);
            using (IDbConnection conn = new SqlConnection(CONN_STR))
            {
                return conn.Query<GroupAuthorityUserModel>(param: param, commandType: CommandType.StoredProcedure
                    , sql: "DBO.USP_GROUP_USER_Authority_S");
            }
        }

        public int USP_GROUP_USER_IU(string IDX, string GROUP_ID, string M_IDX)
        {
            var param = new DynamicParameters();
            param.Add("@IDX", IDX);
            param.Add("@GROUP_ID", GROUP_ID);
            param.Add("@M_IDX", M_IDX);
            using (IDbConnection conn = new SqlConnection(CONN_STR))
            {
                return conn.QuerySingle<int>(param: param, commandType: CommandType.StoredProcedure
                    , sql: "DBO.USP_GROUP_USER_IU");
            }
        }

        public void USP_ReSetPW(string M_IDX)
        {
            var param = new DynamicParameters();
            param.Add("@M_IDX", M_IDX);
            param.Add("@PW", Utility.Encrypt_SHA("qwer1234!"));
            using (IDbConnection conn = new SqlConnection(CONN_STR))
            {
                conn.Execute(param: param, commandType: CommandType.StoredProcedure, sql: "DBO.USP_ReSetPW");
            }
        }

        public IEnumerable<IndividualAuthorityModel> USP_Individual_Authority_S(int IDX)
        {
            var param = new DynamicParameters();
            param.Add("@IDX", IDX);
            using (IDbConnection conn = new SqlConnection(CONN_STR))
            {
                return conn.Query<IndividualAuthorityModel>(param: param, commandType: CommandType.StoredProcedure
                    , sql: "DBO.USP_Individual_Authority_S");
            }
        }

        public void USP_Individual_Authority_I(string Users)
        {
            var param = new DynamicParameters();
            param.Add("@Users", Users);
            using (IDbConnection conn = new SqlConnection(CONN_STR))
            {
                conn.Execute(param: param, commandType: CommandType.StoredProcedure
                    , sql: "DBO.USP_Individual_Authority_I");
            }
        }

        public IEnumerable<IndividualAuthorityModel> USP_GroupAuthorityUpdate(string json)
        {
            var param = new DynamicParameters();
            param.Add("@json", json);
            using (IDbConnection conn = new SqlConnection(CONN_STR))
            {
                return conn.Query<IndividualAuthorityModel>(param: param, commandType: CommandType.StoredProcedure
                                       , sql: "DBO.USP_GroupAuthorityUpdate");
            }
        }

        public void USP_AuthorityDelete(int ROLE_ID)
        {
            var param = new DynamicParameters();
            param.Add("@ROLE_ID", ROLE_ID);
            using (IDbConnection conn = new SqlConnection(CONN_STR))
            {
                conn.Execute(param: param, commandType: CommandType.StoredProcedure, sql: "DBO.USP_AuthorityDelete");
            }
        }
    }
}