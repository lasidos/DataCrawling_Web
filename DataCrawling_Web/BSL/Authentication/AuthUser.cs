using DataCrawling_Web.BSL.Common;
using DataCrawling_Web.BSL.Core;
using DataCrawling_Web.Models;
using DataCrawling_Web.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace DataCrawling_Web.BSL.Authentication
{
    public class AuthUser
    {
        private static readonly string _keyCode = "gtm56km412#$%inb5040sr!@#$%&^&(I";

        /// <summary>
        /// 사용 세션명 정의
        /// </summary>
        private static List<AuthUserEntity> getSessionName
        {
            get
            {
                List<AuthUserEntity> objSessionN = new List<AuthUserEntity>() { };

                objSessionN.Add(new AuthUserEntity { Session_Name = "M_ID", Session_Type_Code = "M" });

                return objSessionN;
            }
        }

        #region Encrypt_SHA : SHA256Bit 암호화 함수
        // 용도 : 비밀번호용. 복호화가 필요 없이 분실시 새로 생성해야 하는 경우 사용
        public static string Encrypt_SHA(string Data)
        {
            SHA256 sha = new SHA256Managed();
            byte[] hash = sha.ComputeHash(Encoding.ASCII.GetBytes(Data));

            StringBuilder stringBuilder = new StringBuilder();
            foreach (byte b in hash)
            {
                stringBuilder.AppendFormat("{0:x2}", b);
            }
            return stringBuilder.ToString();
        }
        #endregion

        #region Encrypt_AES : Decrypt_AES : AES 256Bit 암복호화 함수

        /// <summary>
        /// 암호화
        /// 용도 : 주민번호, 신용카드번호 등 복호화 해야 하는 경우 처리
        /// </summary>
        /// <param name="Input">대상 문자열</param>
        /// <param name="key">키</param>
        /// <returns></returns>
        public static string Encrypt_AES(string Input, string key = "")
        {
            if (string.IsNullOrEmpty(key)) key = _keyCode;

            string Output = "";

            if (Input != "")
            {
                RijndaelManaged aes = new RijndaelManaged();
                aes.KeySize = 256;
                aes.BlockSize = 128;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

                var encrypt = aes.CreateEncryptor(aes.Key, aes.IV);
                byte[] xBuff = null;
                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, encrypt, CryptoStreamMode.Write))
                    {
                        byte[] xXml = Encoding.UTF8.GetBytes(Input);
                        cs.Write(xXml, 0, xXml.Length);
                    }

                    xBuff = ms.ToArray();
                }

                Output = Convert.ToBase64String(xBuff);
            }

            return Output;
        }

        /// <summary>
        /// 복호화
        /// </summary>
        /// <param name="Input">암호화 문자열</param>
        /// <param name="key">키</param>
        /// <returns></returns>
        public static string Decrypt_AES(string Input, string key = "")
        {
            if (string.IsNullOrEmpty(key)) key = _keyCode;

            string Output = "";

            if (Input != "")
            {
                RijndaelManaged aes = new RijndaelManaged();
                aes.KeySize = 256;
                aes.BlockSize = 128;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

                var decrypt = aes.CreateDecryptor();
                byte[] xBuff = null;
                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, decrypt, CryptoStreamMode.Write))
                    {
                        byte[] xXml = Convert.FromBase64String(Input);
                        cs.Write(xXml, 0, xXml.Length);
                    }

                    xBuff = ms.ToArray();
                }

                Output = Encoding.UTF8.GetString(xBuff);
            }

            return Patchnull(Output);
        }

        #endregion

        #region patchnull : null 문자 패치 ( 통신데이터에서 ASCII 코드가 00 인것을 공백으로 치환 )
        public static string Patchnull(string tmp)
        {
            string retVal = "";
            retVal = HttpUtility.UrlDecode(HttpUtility.UrlEncode(tmp).Replace("%00", ""));

            return retVal;
        }
        #endregion

        /// <summary>
        /// 세션 쿠키 시간 증가
        /// </summary>
        /// <param name="cookieID"></param>
        /// <param name="dbName"></param>
        /// <param name="key"></param>
        private static void SetUpdateCookie(String cookieID, String key = "")
        {
            HttpContext objContext = HttpContext.Current;
            HttpCookie objCookie = objContext.Request.Cookies["M%5FUser"];

            if (objCookie != null)
            {
                if (objCookie.HasKeys)
                {
                    if (objCookie.Values["LoginTime"] != null)
                    {
                        if (DateTime.Now.Subtract(Convert.ToDateTime(HttpUtility.UrlDecode(objCookie.Values["LoginTime"]))).Minutes > 15)
                        {
                            if (objCookie.Values["LoginStat"] != null)
                            {
                                if (!String.IsNullOrEmpty(objCookie.Values["LoginStat"]))
                                {
                                    if (Decrypt_AES(objCookie.Values["LoginStat"]) == "YES")
                                    {
                                        objCookie.Domain = "jobkorea.co.kr";
                                        objCookie.Values["LoginStat"] = Encrypt_AES("YES").Trim();
                                        objCookie.Values["LoginTime"] = HttpUtility.UrlEncode(DateTime.Now.ToString("yyyy-MM-dd HH:mm:00"));

                                        objCookie.Values["M_ID"] = Encrypt_AES(cookieID, key).Trim();
                                        objContext.Response.Cookies.Set(objCookie);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 세션 정의
        /// </summary>
        /// <param name="clientIP"></param>
        public static void SetSessionCookie()
        {
            HttpContext objContext = HttpContext.Current;
            HttpCookie objCookie = objContext.Request.Cookies["M%5FUser"];
            bool isLogin = false;

            if (objCookie != null && objCookie.HasKeys)
            {
                if (objCookie.Values["LoginStat"] != null)
                {
                    if (Decrypt_AES(objCookie.Values["LoginStat"]) == "YES")
                    {
                        String objDescryption = String.Empty;
                        String objItem = String.Empty;
                        foreach (var item in getSessionName)
                        {
                            objItem = item.Session_Name;

                            if (objCookie.Values[objItem.Replace("_", "%5F")] != null)
                            {
                                if (!String.IsNullOrEmpty(objCookie.Values[objItem.Replace("_", "%5F")]))
                                {
                                    objContext.Session[objItem] = Decrypt_AES(objCookie.Values[objItem.Replace("_", "%5F")]);
                                    objContext.Session["Mem_Type_Code"] = item.Session_Type_Code;
                                    SetUpdateCookie(objContext.Session[objItem].ToString());

                                    isLogin = true;

                                    // 사용되지 않는 세션 초기화
                                    foreach (var name in getSessionName)
                                    {
                                        if (name.Session_Name.Equals(objItem))
                                            continue;

                                        objContext.Session[name.Session_Name] = "";
                                    }
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            if (isLogin == false)
            {
                // 세션 초기화
                objContext.Session["Mem_Type_Code"] = "";

                foreach (var item in getSessionName)
                    objContext.Session[item.Session_Name] = "";
            }
        }

        public static void SetSessionAdminCookie()
        {
            HttpContext objContext = HttpContext.Current;
            HttpCookie objCookie = objContext.Request.Cookies["Emp%5Fcode"];

            if (objCookie == null)
            {
                objContext.Session["Emp_code"] = "";
            }
            else
            {
                Encoding encoding = Encoding.GetEncoding("euc-kr");
                objContext.Session["Emp_code"] = encoding.GetString(HttpUtility.UrlDecodeToBytes(objCookie.Value));
            }
        }

        /// <summary>
        /// 신규 쿠키 할당
        /// </summary>
        /// <param name="Mem_Id">회원 아이디</param>
        /// <param name="Mem_Type_Code">회원 구분 코드</param>
        public static void SetNewCookie(String Mem_Id, String Mem_Type_Code, String redirect, String Token = null)
        {
            //new SimpleCookie().Set("M%5ID", Utility.Encrypt_AES(Mem_Id).Trim());
            HttpContext objContext = HttpContext.Current;
            HttpCookie objCookie = objContext.Response.Cookies["MK%5FUser"];
            objCookie.Domain = redirect;
            
            objContext.Response.Cookies["MK%5FUser"]["M%5ID"] = Utility.Encrypt_AES(Mem_Id).Trim();
            objContext.Response.Cookies["MK%5FUser"]["Mem%5Type%5Code"] = Utility.Encrypt_AES(Mem_Type_Code).Trim();
            if (!String.IsNullOrEmpty(Token)) objContext.Response.Cookies["MK%5FUser"]["Mem%5DI"] = Utility.Encrypt_AES(Token).Trim();
            objContext.Response.Cookies["MK%5FUser"]["LoginStat"] = Utility.Encrypt_AES("YES").Trim();
            objContext.Response.Cookies["MK%5FUser"]["LoginTime"] = HttpUtility.UrlEncode(DateTime.Now.ToString("yyyy-MM-dd HH:mm:00"));
        }

        public static void RemoveCookie(String Mem_Type_Code)
        {
            HttpContext objContext = HttpContext.Current;
            HttpCookie objCookie = objContext.Response.Cookies["JK%5FUser"];
            objCookie.Domain = "jobkorea.co.kr";

            switch (Mem_Type_Code.ToUpper())
            {
                case "C":
                    objContext.Response.Cookies["JK%5FUser"]["C%5FID"] = String.Empty;
                    break;
                case "H":
                    objContext.Response.Cookies["JK%5FUser"]["H%5FID"] = String.Empty;
                    break;
                case "E":
                    objContext.Response.Cookies["JK%5FUser"]["E%5FID"] = String.Empty;
                    break;
                case "S":
                    objContext.Response.Cookies["JK%5FUser"]["S%5FID"] = String.Empty;
                    break;
                default:
                    objContext.Response.Cookies["JK%5FUser"]["M%5FID"] = String.Empty;
                    break;
            }
            objContext.Response.Cookies["JK%5FUser"]["DS%5FID"] = "";
            objContext.Response.Cookies["JK%5FUser"]["MemSysNo"] = "";
            objCookie.Expires = DateTime.Now.AddDays(-100);

            HttpCookie maintainCookie = objContext.Response.Cookies["LoginMaintain"];
            objContext.Response.Cookies["LoginMaintain"]["CookieKey"] = "";
            maintainCookie.Domain = "jobkorea.co.kr";
            maintainCookie.Expires = DateTime.Now.AddDays(-100);
        }



        /// <summary>
        /// 로그인 화면 노출용 쿠키 할당
        /// </summary>
        /// <param name="Mem_Id">회원 아이디</param>
        /// <param name="Mem_Type_Code">회원 구분 코드</param>
        public static void setLoginCookie(string Mem_Id, string Mem_Type_Code)
        {
            HttpContext objContext = HttpContext.Current;
            HttpCookie objCookie = objContext.Response.Cookies["C%5FUser"];
            objCookie.Domain = "myplatformkorea.co.kr";

            objCookie.Values["AAA"] = Utility.Encrypt_AES(Mem_Id).Trim();
            objCookie.Expires = DateTime.Now.AddDays(14);

            objContext.Response.Cookies.Set(objCookie);
        }

        /// <summary>
        /// 휴면회원 처리용 쿠키 할당
        /// </summary>
        /// <param name="Mem_Id">회원 아이디</param>
        /// <param name="Mem_Type_Code">회원 구분 코드</param>
        public static void setSleepSession(string Mem_Id, string Mem_Type_Code)
        {
            HttpContext objContext = HttpContext.Current;
            objContext.Session["Sleep_Mem_Id"] = Mem_Id;
            objContext.Session["Sleep_Mem_Type_Code"] = Mem_Type_Code;

            HttpCookie objCookie = objContext.Response.Cookies["JK%5FSleep"];
            objCookie.Domain = "jobkorea.co.kr";
            objContext.Response.Cookies["JK%5FSleep"]["Mem%5FId"] = Utility.Encrypt_AES(Mem_Id).Trim();
            objContext.Response.Cookies["JK%5FSleep"]["Mem%5FType%5FCode"] = Mem_Type_Code;
        }

        /// <summary>
        /// 회원정보에 접근하기 위한 비밀번호 인증 상태 저장
        /// </summary>
        /// <param name="val"></param>
        public static void SetMemPwdChkSession(object val)
        {
            HttpContext objContext = HttpContext.Current;
            objContext.Session["MemPwdChk"] = val;
        }

        public static bool CheckLastUserId()
        {
            bool isChangedUser = (HttpContext.Current.Session["LastLoginUserId"] as string) != AuthUser.M_ID;
            HttpContext.Current.Session["LastLoginUserId"] = AuthUser.M_ID;

            return isChangedUser;
        }

        public static String M_ID
        {
            get
            {
                return HttpContext.Current.Session["M_ID"] != null ? HttpContext.Current.Session["M_ID"].ToString() : "";
            }
        }

        public static UserInfo UserInfo
        {
            get
            {
                if (HttpContext.Current.Session["M_User"] != null)
                {
                    string token = HttpContext.Current.Session["M_User"].ToString();
                    return new ClsJWT().ValidateCurrentToken(token);
                }
                else
                {
                    return null;
                }
            }
        }

        public static UserInfo JoinMember { get; set; }

        public static String C_ID
        {
            get
            {
                return HttpContext.Current.Session["C_ID"] != null ? HttpContext.Current.Session["C_ID"].ToString() : "";
            }
        }

        public static String S_ID
        {
            get
            {
                return HttpContext.Current.Session["S_ID"] != null ? HttpContext.Current.Session["S_ID"].ToString() : "";
            }
        }

        public static String E_ID
        {
            get
            {
                return HttpContext.Current.Session["E_ID"] != null ? HttpContext.Current.Session["E_ID"].ToString() : "";
            }
        }

        public static String EmployeeCode
        {
            get
            {
                return HttpContext.Current.Session["Emp_code"] != null ? HttpContext.Current.Session["Emp_code"].ToString() : "";
            }
        }
    }

    /// <summary>
    /// 사용자 인증 엔티티
    /// </summary>
    public class AuthUserEntity
    {
        public String Session_Name { get; set; }
        public String Session_Type_Code { get; set; }
    }
}