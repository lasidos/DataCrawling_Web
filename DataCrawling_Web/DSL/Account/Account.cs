using Dapper;
using DataCrawling_Web.BSL.Common;
using DataCrawling_Web.BSL.File;
using DataCrawling_Web.Models;
using DataCrawling_Web.Models.DSL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.ConstrainedExecution;
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
            param.Add("@EMAIL", email, DbType.String, size: 100);
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

        /// <summary>
        /// 회원가입 인증코드 발송
        /// </summary>
        /// <param name="email">수신이메일</param>
        /// <param name="code">인증번호</param>
        /// <returns></returns>
        public IEnumerable<CertModel> PushCode(string email, string code)
        {
            var param = new DynamicParameters();
            param.Add("@EMAIL", email, DbType.String, size: 100);
            param.Add("@CERT_NUM", code, DbType.String, size: 100);
            using (IDbConnection conn = new SqlConnection(CONN_STR))
            {
                return conn.Query<CertModel>(param: param, commandType: CommandType.StoredProcedure
                    , sql: "DBO.USP_AuthUser_CertNuM_I");
            }
        }

        /// <summary>
        /// 회원가입 인증코드 확인
        /// </summary>
        /// <param name="email">수신이메일</param>
        /// <param name="cert">사용자입력 인증코드</param>
        /// <returns></returns>
        public IEnumerable<CertModel> ConfirmPushCode(string email, string cert)
        {
            var param = new DynamicParameters();
            param.Add("@EMAIL", email, DbType.String, size: 100);
            param.Add("@CERT_NUM", cert, DbType.String, size: 100);
            using (IDbConnection conn = new SqlConnection(CONN_STR))
            {
                return conn.Query<CertModel>(param: param, commandType: CommandType.StoredProcedure
                    , sql: "DBO.USP_AuthUser_CertNuM_S");
            }
        }

        public string CheckExist(string email)
        {
            return null;
        }

        public IEnumerable<UserInfo> RegisterMember(UserInfo user)
        {
            var param = new DynamicParameters();
            param.Add("@EMAIL", Utility.Encrypt_AES(user.User_ID));
            param.Add("@User_PW", Utility.Encrypt_SHA(user.User_PW));
            param.Add("@User_Name", Utility.Encrypt_AES(user.User_Name));
            param.Add("@Phone", Utility.Encrypt_AES(user.Phone));
            param.Add("@Gender", Utility.Encrypt_AES(user.Gender));
            param.Add("@TermAgree", Utility.Encrypt_AES(user.TermAgree.ToString()));
            param.Add("@MemberType", Utility.Encrypt_AES(user.MemberType));
            using (IDbConnection conn = new SqlConnection(CONN_STR))
            {
                return conn.Query<UserInfo>(param: param, commandType: CommandType.StoredProcedure
                    , sql: "DBO.USP_AuthUser_I");
            }
        }

        #endregion
    }
}