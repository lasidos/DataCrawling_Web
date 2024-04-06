using Dapper;
using DataCrawling_Web.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace DataCrawling_Web.DSL.Account
{
    public class Account
    {
        protected readonly string CONN_STR;

        public Account(bool readOnly = false)
        {
            CONN_STR = "Data Source=211.233.51.65;Initial Catalog=mkapi_godohosting_com;User ID=mkapi;Password=akvmfzh1!@;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        }

        #region 로그인

        #endregion

        public IEnumerable<UserInfo> UserLogin(string email, string pw)
        {
            var param = new DynamicParameters();
            param.Add("@EMAIL", email, DbType.String, size:100);
            param.Add("@PW", pw, DbType.String, size: 100);
            using (IDbConnection conn = new SqlConnection(CONN_STR))
            {
                return conn.Query<UserInfo>(param: param, commandType: CommandType.StoredProcedure
                    , sql: "DBO.USP_AuthUser_S");
            }
        }

        #region 계정정보 찾기

        public string FindAccount(string name, string contact)
        {
            return null;
        }

        public string ChangePassword(string findEmail, string pw)
        {
            return null;
        }

        #endregion

        #region 비밀번호 찾기

        public string PushPasscode(string title, string content, string email, string passCode)
        {
            return null;
        }

        public string ConfirmPasscode(string email)
        {
            return null;
        }

        #endregion

        #region 회원가입

        public string CheckExist(string email)
        {
            return null;
        }

        public string RegisterMember(string email, string pw, string name, string tel, string gender, string acceptConditions, string lv)
        {
            return null;
        }

        #endregion
    }
}