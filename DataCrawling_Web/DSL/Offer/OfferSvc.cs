using Dapper;
using DataCrawling_Web.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using DataCrawling_Web.Models.Offer;
using DataCrawling_Web.Models.Files;

namespace DataCrawling_Web.DSL.Offer
{
    public class OfferSvc
    {
        protected readonly string CONN_STR;

        public OfferSvc(bool readOnly = false)
        {
            CONN_STR = "Data Source=211.233.51.65;Initial Catalog=mkapi_godohosting_com;User ID=mkapi;Password=akvmfzh1!@;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        }

        public IEnumerable<SelCode> GetSelectData(string pageCode)
        {
            var param = new DynamicParameters();
            param.Add("@PAGE_CODE", pageCode);
            using (IDbConnection conn = new SqlConnection(CONN_STR))
            {
                return conn.Query<SelCode>(param: param, commandType: CommandType.StoredProcedure
                    , sql: "DBO.USP_SelectData_S");
            }
        }

        public IEnumerable<OfferDBModel> USP_RegistOffer_S(string pageCode, string tab, string uid)
        {
            var param = new DynamicParameters();
            param.Add("@pageCode", pageCode);
            param.Add("@tab", tab);
            param.Add("@uid", uid);
            using (IDbConnection conn = new SqlConnection(CONN_STR))
            {
                return conn.Query<OfferDBModel>(param: param, commandType: CommandType.StoredProcedure
                    , sql: "DBO.USP_RegistOffer_S");
            }
        }

        public IEnumerable<ResultSvc> USP_RegistOffer_I(string uid, RegDbModel regDb)
        {
            var param = new DynamicParameters();
            param.Add("@uid", uid);
            param.Add("@offerType", regDb.offerType);
            param.Add("@hidStatType", regDb.hidStatType);
            param.Add("@hidPlanType", regDb.hidPlanType);
            param.Add("@hidPeriodType", regDb.hidPeriodType);
            param.Add("@lb_Url", regDb.lb_Url);
            param.Add("@txtContent", regDb.txtContent);
            param.Add("@etcContent", regDb.etcContent);
            param.Add("@FileIdx", regDb.FileIdx == null ? "" : string.Join(",", regDb.FileIdx));
            using (IDbConnection conn = new SqlConnection(CONN_STR))
            {
                return conn.Query<ResultSvc>(param: param, commandType: CommandType.StoredProcedure
                    , sql: "DBO.USP_RegistOffer_I");
            }
        }
    }
}