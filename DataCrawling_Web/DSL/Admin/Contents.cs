using Dapper;
using DataCrawling_Web.Models.Admin;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DataCrawling_Web.DSL.Admin
{
    public class Contents
    {
        protected readonly string CONN_STR;

        public Contents(bool readOnly = false)
        {
            CONN_STR = "Data Source=211.233.51.65;Initial Catalog=mkapi_godohosting_com;User ID=mkapi;Password=akvmfzh1!@;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        }

        public IEnumerable<ContentInfoModel> GetContents(int MENU_IDX = -1, int CODE = -1)
        {
            var param = new DynamicParameters();
            param.Add("@MENU_IDX", MENU_IDX, DbType.Int32, size: 100);
            param.Add("@CODE", CODE, DbType.Int32, size: 100);
            using (IDbConnection conn = new SqlConnection(CONN_STR))
            {
                return conn.Query<ContentInfoModel>(param: param, commandType: CommandType.StoredProcedure
                    , sql: "DBO.USP_ADMIN_CONTENTS_S");
            }
        }
    }
}