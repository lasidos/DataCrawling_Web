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

        public IEnumerable<MenuModel> USP_ADMIN_NOTICE_MENU_S()
        {
            var param = new DynamicParameters();
            using (IDbConnection conn = new SqlConnection(CONN_STR))
            {
                return conn.Query<MenuModel>(commandType: CommandType.StoredProcedure
                    , sql: "DBO.USP_ADMIN_NOTICE_MENU_S");
            }
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

        /// <summary>
        /// 컨텐츠 등록/수정
        /// </summary>
        /// <param name="IDX"></param>
        /// <param name="AREA_CODE"></param>
        /// <param name="SECTOR_CODE"></param>
        /// <param name="PORT_IMG"></param>
        /// <param name="TITLE"></param>
        /// <param name="CONTENT_BODY"></param>
        /// <param name="VISIBLE"></param>
        /// <param name="M_ID"></param>
        public void USP_ADMIN_CONTENTS_IU(int IDX, int AREA_CODE, int SECTOR_CODE, string PORT_IMG, 
            string TITLE, string CONTENT_BODY, int VISIBLE, string M_ID)
        {
            var param = new DynamicParameters();
            param.Add("@IDX", IDX);
            param.Add("@AREA_CODE", AREA_CODE);
            param.Add("@SECTOR_CODE", SECTOR_CODE);
            param.Add("@PORT_IMG", PORT_IMG);
            param.Add("@TITLE", TITLE);
            param.Add("@CONTENT_BODY", CONTENT_BODY);
            param.Add("@VISIBLE", VISIBLE);
            param.Add("@M_ID", M_ID);
            using (IDbConnection conn = new SqlConnection(CONN_STR))
            {
                conn.Execute(param: param, commandType: CommandType.StoredProcedure
                                       , sql: "DBO.USP_ADMIN_CONTENTS_IU");
            }
        }
    }
}