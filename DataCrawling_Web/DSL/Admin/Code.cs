using Dapper;
using DataCrawling_Web.Models.Admin;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Razor.Tokenizer.Symbols;
using System.Configuration;
using DataCrawling_Web.BSL.Authentication;

namespace DataCrawling_Web.DSL.Admin
{
    public class Code
    {
        protected readonly string CONN_STR;

        public Code(bool readOnly = false)
        {
            CONN_STR = "Data Source=211.233.51.65;Initial Catalog=mkapi_godohosting_com;User ID=mkapi;Password=akvmfzh1!@;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        }

        public IEnumerable<MenuCode> USP_MENU_CODE_S()
        {
            using (IDbConnection conn = new SqlConnection(CONN_STR))
            {
                return conn.Query<MenuCode>(commandType: CommandType.StoredProcedure
                    , sql: "DBO.USP_MENU_CODE_S");
            }
        }

        public IEnumerable<W_MenuModel> USP_MENU_IU(W_MenuModel model)
        {
            var param = new DynamicParameters();
            param.Add("@Idx", model.Menu_Idx);
            param.Add("@Parent_Id", model.Parent_Id);
            param.Add("@Title", model.Menu_Name);
            param.Add("@Url", model.Menu_URL);
            param.Add("@Order", model.Order_No);
            param.Add("@Login", model.Login_Stat);
            param.Add("@Visible", model.Display_Stat);
            param.Add("@M_Id", model.M_Id);
            param.Add("@MenuType", model.Menu_Type);
            using (IDbConnection conn = new SqlConnection(CONN_STR))
            {
                return conn.Query<W_MenuModel>(param: param, commandType: CommandType.StoredProcedure
                    , sql: "DBO.USP_MENU_IU");
            }
        }

        public IEnumerable<W_MenuModel> USP_MENU_D(int idx)
        {
            var param = new DynamicParameters();
            param.Add("@IDX", idx);
            using (IDbConnection conn = new SqlConnection(CONN_STR))
            {
                return conn.Query<W_MenuModel>(param: param, commandType: CommandType.StoredProcedure
                    , sql: "DBO.USP_MENU_D");
            }
        }
    }
}