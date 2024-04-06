using DataCrawling_Web.BSL.Authentication;
using DataCrawling_Web.BSL.Extentions;
using DataCrawling_Web.Models.Files;
using DataCrawling_Web.Models.Param;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;

namespace DataCrawling_Web.Service
{
    /// <summary>
    /// 모든 서비스에서 사용할 수 있는 공용 서비스
    /// </summary>
    public class CommonService 
    {

#if DEBUG
        public readonly string ROOTPATH_192_168_2_10 = @"C:\FileuploadTest";
#else
        public readonly string ROOTPATH_192_168_2_10 = @"\\192.168.2.10";
#endif

        #region [ ErrorLog ]

        public void WebSiteErrorLog(Exception ex, string addMessage = "", string errorSite = "file2.jobkorea.co.kr")
        {
            var httpCtx = HttpContext.Current;
            var request = httpCtx.Request;
            var trace = new StackTrace(ex, true);
            var errorMsg = new StringBuilder();
            var httpException = ex as HttpException;

            errorMsg.Append("ServerVariables : ").AppendLine();
            foreach (string key in request.ServerVariables)
            {
                if (string.IsNullOrEmpty(request.ServerVariables[key]))
                    continue;

                errorMsg.AppendFormat("ServerVariables_{0}={1}", key, request.ServerVariables[key]).AppendLine();
            }

            errorMsg.AppendLine().Append("Get : ").AppendLine();
            foreach (string key in request.QueryString)
            {
                if (key.IndexOf("pwd", StringComparison.OrdinalIgnoreCase) > -1)
                    continue;

                errorMsg.AppendFormat("{0}={1}", key, request.QueryString[key]).AppendLine();
            }

            errorMsg.AppendLine().Append("Post : ").AppendLine();
            foreach (string key in request.Form)
            {
                if (key.IndexOf("pwd", StringComparison.OrdinalIgnoreCase) > -1)
                    continue;

                errorMsg.AppendFormat("{0}={1}", key, request.Form[key]).AppendLine();
            }

            errorMsg.AppendLine().Append("Session : ").AppendLine();
            errorMsg.AppendFormat("M_ID={0}", AuthUser.M_ID).AppendLine();
            errorMsg.AppendFormat("C_ID={0}", AuthUser.C_ID).AppendLine();
            errorMsg.AppendFormat("E_ID={0}", AuthUser.E_ID).AppendLine();
            errorMsg.AppendFormat("S_ID={0}", AuthUser.S_ID).AppendLine();

            errorMsg.AppendLine().Append("Exception : ").AppendLine();
            errorMsg.AppendLine().Append(ex.ToString());

            if (string.IsNullOrEmpty(addMessage) == false)
            {
                errorMsg.AppendLine().Append(addMessage);
            }

            var p = new USP_AAA_WebServerErrorLog_I_param()
            {
                Server_Ip = request.ServerVariables["LOCAL_ADDR"],
                Error_Site = errorSite,
                Error_Server_Name = request.ServerVariables["SERVER_NAME"],
                Error_Ctgr_Name = ex.Source,
                Error_Desct = ex.Message,
                Org_File_Name = request.ServerVariables["URL"],
                User_Ip = request.UserHostAddress,
                Detail_Cntnt = errorMsg.ToString(),
                Http_Status_Code = httpException == null ? 0 : httpException.GetHttpCode()
            };

            Ctx.RpSvc.USP_AAA_WebServerErrorLog_I(p);
        }

        public void FileDownloadErrorLog(int code = 0, string message = null)
        {
            var httpCtx = HttpContext.Current;
            var request = httpCtx.Request;
            var errorMsg = new StringBuilder();

            errorMsg.AppendLine().Append("Get : ").AppendLine();
            foreach (string key in request.QueryString)
                errorMsg.AppendFormat("{0}={1}", key, request.QueryString[key]).AppendLine();

            errorMsg.AppendLine().Append("Post : ").AppendLine();
            foreach (string key in request.Form)
                errorMsg.AppendFormat("{0}={1}", key, request.Form[key]).AppendLine();

            errorMsg.AppendLine().Append("Session : ").AppendLine();
            errorMsg.AppendFormat("M_ID={0}", AuthUser.M_ID).AppendLine();
            errorMsg.AppendFormat("C_ID={0}", AuthUser.C_ID).AppendLine();
            errorMsg.AppendFormat("E_ID={0}", AuthUser.E_ID).AppendLine();
            errorMsg.AppendFormat("S_ID={0}", AuthUser.S_ID).AppendLine();

            errorMsg.AppendLine();
            errorMsg.AppendFormat("CODE={0}", code).AppendLine();
            errorMsg.AppendFormat("MSG={0}", message).AppendLine();

            var p = new USP_FileDownLoad_Error_Log_I_param()
            {
                Referer = request.UrlReferrer == null ? "" : request.UrlReferrer.ToString(),
                MoveUrl = request.Url.ToString(),
                UserIP = request.ServerVariables["REMOTE_ADDR"],
                ServerIP = request.ServerVariables["LOCAL_ADDR"],
                CONTENT = errorMsg.ToString()
            };

            Ctx.RpSvc.USP_FileDownLoad_Error_Log_I(p);
        }

        #endregion

        #region [ 개인정보검사 ]

        /// <summary>
        /// 개인정보 모듈 결과 가져오기
        /// (UploadHelper에서 가져옴) 
        /// </summary>
        /// <returns></returns>
        public JKFileContentsFilterResult CheckFileContents()
        {
            JKFileContentsFilterResult model = new JKFileContentsFilterResult();
            var request = HttpContext.Current.Request;
            try
            {
                // 개인정보 포함 여부 확인
                if (request.Files != null &&
                    request.Files.Count > 0 &&
                    !string.IsNullOrEmpty(request.Headers["JKFilterPassed"]))
                {
                    string headers = request.Headers["JKFilterPassed"];
                    char sp = ';';
                    string[] arrHeaders = headers.Split(sp);

                    char dSp = '=';
                    string[] strResult = arrHeaders[0].Split(dSp);


                    model.Result = !strResult[1].Equals("FALSE") ? true : false;

                    if (!model.Result)
                    {
                        if (arrHeaders.Length > 1)
                        {
                            string[] strIvalidCount = arrHeaders[1].Split(dSp);
                            model.InvalidCount = Convert.ToInt32(strIvalidCount[1]);
                        }

                        if (arrHeaders.Length > 2)
                        {
                            string[] strFileNames = arrHeaders[2].Split(dSp);
                            if (!string.IsNullOrEmpty(strFileNames[1]))
                            {
                                model.FileNames = strFileNames[1].Split(',');
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                model.Result = false;
                model.ErrorMessage = ex.Message;
            }

            return model;
        }

        #endregion

        #region [ 폴더생성 ]
        /// <summary>
        /// 새로운 폴더 생성
        /// </summary>
        /// <param name="path">파일 경로</param>
        public void CreateDirectory(string path)
        {
            var targetDirectory = Path.GetDirectoryName(path);
            if (!Directory.Exists(targetDirectory))
            {
                Directory.CreateDirectory(targetDirectory);
            }
        }
        #endregion

        #region [ CheckFileNameOverlap ]
        /// <summary>
        /// 중복 파일 - 파일명 (N) 추가
        /// </summary>
        /// <param name="directoryPath"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public bool CheckFileNameOverlap(string fileFullName, out string outFileName)
        {
            return CheckFileNameOverlap(Path.GetDirectoryName(fileFullName), Path.GetFileName(fileFullName), out outFileName);
        }

        public bool CheckFileNameOverlap(string directoryPath, string fileName, out string outFileName)
        {
            outFileName = string.Empty;
            if (fileName.Length == 0)
                return false;

            string extension = Path.GetExtension(fileName);
            if (string.IsNullOrEmpty(extension))
                return false;

            string strName = fileName.Replace(extension, "");
            int fileCount = 0;

            while (true)
            {
                if (!File.Exists(Path.Combine(directoryPath, fileName)))
                    break;

                fileName = strName + "(" + ++fileCount + ")" + extension;
            }
            outFileName = fileName;
            return true;
        }
        #endregion

        /// <summary>
        /// 랜덤파일명 생성
        /// </summary>
        /// <returns></returns>
        public string GetCreateRandomFileName(int length)
        {
            string randomStr = StringHelper.GenerateRandomStringWithNumber(length);
            randomStr += String.Format("{0:yyMMddHHmmssff}", DateTime.Now);

            return randomStr;

        }

        /// <summary>
        /// BadCharacter 치환
        /// (UploadHelper에서 가져옴)
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public string ReplaceBadCharacter(string text)
        {
            string result = string.Empty;
            Dictionary<string, string> badCharacters = new Dictionary<string, string>()
            {
                {
                    "'", "`"
                }
            };

            if (string.IsNullOrWhiteSpace(text))
            {
                return result;
            }

            result = text;

            foreach (var character in badCharacters)
            {
                if (text.IndexOf(character.Key) > -1)
                {
                    result = text.Replace(character.Key, character.Value);
                }
            }
            return result;
        }
    }

}