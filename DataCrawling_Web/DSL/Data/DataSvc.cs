using Dapper;
using DataCrawling_Web.Models.Data;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DataCrawling_Web.DSL.Data
{
    public class DataSvc
    {
        protected readonly string CONN_STR;

        public DataSvc()
        {
            CONN_STR = "Data Source=211.233.51.65;Initial Catalog=mkapi_godohosting_com;User ID=mkapi;Password=akvmfzh1!@;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        }

        public IEnumerable<DataInfoModel> USP_DATALIST_S(string A_No = "", string keyword = "")
        {
            var param = new DynamicParameters();
            param.Add("@A_No", A_No);
            param.Add("@KEYWORD", keyword);
            using (IDbConnection conn = new SqlConnection(CONN_STR))
            {
                return conn.Query<DataInfoModel>(param: param, commandType: CommandType.StoredProcedure
                    , sql: "DBO.USP_DATALIST_S");
            }
        }

        public IEnumerable<Api_Model> USP_DATALIST_API_S(string A_No = "")
        {
            var param = new DynamicParameters();
            param.Add("@A_No", A_No);
            using (IDbConnection conn = new SqlConnection(CONN_STR))
            {
                return conn.Query<Api_Model>(param: param, commandType: CommandType.StoredProcedure
                    , sql: "DBO.USP_DATALIST_API_S");
            }
        }

        public IEnumerable<Authentication_Key_Model> USP_Authentication_Key_S(string Auth_ID)
        {
            var param = new DynamicParameters();
            param.Add("@Auth_ID", Auth_ID);
            using (IDbConnection conn = new SqlConnection(CONN_STR))
            {
                return conn.Query<Authentication_Key_Model>(param: param, commandType: CommandType.StoredProcedure
                    , sql: "DBO.USP_Authentication_Key_S");
            }
        }

        public IEnumerable<Authentication_Key_Model> USP_Authentication_Key_I(string Auth_ID, string Authentication_Key)
        {
            var param = new DynamicParameters();
            param.Add("@Auth_ID", Auth_ID);
            param.Add("@Authentication_Key", Authentication_Key);
            using (IDbConnection conn = new SqlConnection(CONN_STR))
            {
                return conn.Query<Authentication_Key_Model>(param: param, commandType: CommandType.StoredProcedure
                    , sql: "DBO.USP_Authentication_Key_I");
            }
        }
    }
}