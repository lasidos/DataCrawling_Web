using Dapper;
using DataCrawling_Web.Models.Offer;
using DataCrawling_Web.Models.Param;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;

namespace DataCrawling_Web.DSL.Files
{
    public class GGRpSvc
    {
        protected readonly string CONN_STR;

        public GGRpSvc()
        {
            CONN_STR = "Data Source=211.233.51.65;Initial Catalog=mkapi_godohosting_com;User ID=mkapi;Password=akvmfzh1!@;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        }

        public int USP_UserFileDB_I(string id, string docType, string fullFileName)
        {
            var param = new DynamicParameters();
            param.Add("@UID", id);
            param.Add("@DOC_TYPE", docType);
            param.Add("@FILE_NAME", fullFileName);
            using (IDbConnection conn = new SqlConnection(CONN_STR))
            {
                var result =  conn.QuerySingle(param: param, commandType: CommandType.StoredProcedure
                    , sql: "DBO.USP_UserFileDB_I");
                
                return Convert.ToInt32(result.IDX);
            }
        }

        public void USP_FileDownLoad_Error_Log_I(USP_FileDownLoad_Error_Log_I_param p)
        {

        }

        public string USP_JKM_UserFileDB_FileInfo_S(string id, int fileNo)
        {
            return null;
        }

        public string USP_JKM_UserFileDB_SU(string id, int fileNo)
        {
            return null;
        }

        public void USP_AAA_WebServerErrorLog_I(USP_AAA_WebServerErrorLog_I_param p)
        {

        }

        public string SelectUserFileDB(string id, int fileNo)
        {
            return null;
        }

        public int UpdateOnPassUserFile(byte File_Type, string fullFileNamePath, string fileContentLength, string id, int fileNo)
        {
            return 0;
        }

        
    }
}