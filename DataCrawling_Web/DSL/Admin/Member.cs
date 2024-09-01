using Dapper;
using DataCrawling_Web.BSL.Common;
using DataCrawling_Web.Models.Admin;
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

        public IEnumerable<GroupInfoModel> USP_GROUP_INFO_S(int GROUP_ID = -1)
        {
            using (IDbConnection conn = new SqlConnection(CONN_STR))
            {
                return conn.Query<GroupInfoModel>(commandType: CommandType.StoredProcedure
                    , sql: "DBO.USP_GROUP_INFO_S");
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
    }
}