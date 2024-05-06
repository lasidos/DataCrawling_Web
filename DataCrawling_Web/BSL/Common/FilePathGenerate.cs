using DataCrawling_Web.BSL.Extentions;
using DataCrawling_Web.Models.Files;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace DataCrawling_Web.BSL.Common
{
    public static class FilePathGenerate
    {
        public static string GetUserCoPassFilePath(string dir, bool isEmail)
        {
            string path = @"\\192.168.2.10\file2\f\Job_Files\Resume\";
            //string path = @"C:\TEMP\";

#if DEBUG
            path = @"C:\FileuploadTest\file2\f\Job_Files\Resume\";
#endif

            // 입사지원첨부파일 지원서 파일 경로
            if (isEmail)
            {
                path = String.Concat(path, @"CoPassFileEmail\", dir);
            }
            else
            {
                path = String.Concat(path, @"CoPassFile\", dir);
            }

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            return path;
        }

        public static string GetUserFilePath(string strUserId)
        {
            string path = @"\\192.168.2.10\file2\f\Job_Files\Resume\AttachFile\" + GetPath(strUserId);
            //string path = @"D:\FileuploadTest\";

#if DEBUG
            path = @"C:\FileuploadTest\file2\f\Job_Files\Resume\AttachFile\" + GetPath(strUserId);
#endif

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            return path;
        }

        // 헤드헌팅 로컬 파일 저장 경로
        public static string GetHeadHuntingFilePath()
        {
            string path = Path.Combine(@"\\192.168.2.10", @"file2\f\Job_Files\HeadHunter\Candidate\");

#if DEBUG
            path = Path.Combine(@"C:\FileServer", @"file2\f\Job_Files\HeadHunter\Candidate\");
#endif
            string yearPath = Path.Combine(path, DateTime.Today.ToString("yyyy"));
            string monthPath = Path.Combine(yearPath, DateTime.Today.ToString("MM"));


            if (!Directory.Exists(yearPath)) Directory.CreateDirectory(yearPath);
            if (!Directory.Exists(monthPath)) Directory.CreateDirectory(monthPath);

            return monthPath;
        }

        // 헤드헌팅 로컬 파일 다운로드경로 경로
        public static string GetHeadHuntingDownloadPath()
        {
            string path = Path.Combine(@"\\192.168.2.10", @"file2\f\Job_Files\HeadHunter\Candidate\");

#if DEBUG
            path = Path.Combine(@"C:\FileServer", @"file2\f\Job_Files\HeadHunter\Candidate\");
#endif
            return path;
        }

        public static string GetRecommandFilePath(string strUserId)
        {
            string path = @"\\192.168.2.10\file2\f\Job_Files\Recommend\AttachFile\" + GetPath(strUserId);
            //string path = @"D:\FileuploadTest\";

#if DEBUG
            path = @"C:\source\jobkorea_file_net\FileJobkorea\FileJobkorea.Web.Person\FileuploadTest\" + GetPath(strUserId);
#endif

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            return path;
        }

        public static string GetRecommandTempFilePath(string folderName)
        {
            string path = @"\\192.168.2.10\file2\f\Job_Files\RecommendTemp\" + folderName;
            //string path = @"D:\FileuploadTest\";

#if DEBUG
            path = @"C:\source\jobkorea_file_net\FileJobkorea\FileJobkorea.Web.Person\FileuploadTestTemp\" + folderName;
#endif

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            return path;
        }

        public static string GetRecommandshowFilePath(string Type)
        {
            string path = @"\\192.168.2.10\file2\f\Job_Files\" + Type + @"\AttachFile\";
            //string path = @"D:\FileuploadTest\";

#if DEBUG
            path = @"C:\source\jobkorea_file_net\FileJobkorea\FileJobkorea.Web.Person\FileuploadTest\";
#endif

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            return path;
        }

        /// <summary>
        /// 공통 이미지 파일 저장 경로
        /// </summary>
        /// <param name="strUserId"></param>
        /// <returns></returns>
        public static string GetUserImageFilePath(string type, string strUserId)
        {
            string path = @"\\192.168.2.10\file2\f\Job_Files\" + type + @"\AttachFile\" + GetPath(strUserId);
            //string path = @"D:\FileuploadTest\";

#if DEBUG
            path = @"C:\source\jobkorea_file_net\FileJobkorea\FileJobkorea.Web.Person\FileuploadTest\" + GetPath(strUserId);
#endif

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            return path;
        }

        /// <summary>
        /// 공통이미지 업로드 임시경로 폴더 위치
        /// </summary>
        /// <param name="type"></param>
        /// <param name="folderName"></param>
        /// <returns></returns>
        public static string GetCommonTempFilePath(string type, string folderName)
        {
            string path = @"\\192.168.2.10\file2\f\Job_Files\" + type + "Temp" + @"\" + folderName + @"\";
            //string path = @"D:\FileuploadTest\";

#if DEBUG
            path = @"C:\source\jobkorea_file_net\FileJobkorea\FileJobkorea.Web.Person\FileuploadTestTemp\" + folderName + @"\";
#endif

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            return path;
        }

        /// <summary>
        /// 공통이미지 업로드 임시경로 폴더 위치
        /// </summary>
        /// <param name="type"></param>
        /// <param name="folderName"></param>
        /// <returns></returns>
        public static string GetCommonFilePath(string type, string strUserId)
        {
            string path = @"\\192.168.2.10\file2\f\Job_Files\" + type + @"\AttachFile\" + GetPath(strUserId);
            //string path = @"D:\FileuploadTest\";

#if DEBUG
            path = @"C:\source\jobkorea_file_net\FileJobkorea\FileJobkorea.Web.Person\FileuploadTest\" + GetPath(strUserId);
#endif

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            return path;
        }

        public static string GetHelpFilePath()
        {

            string path = @"\\192.168.2.10\file1\live\Customer_Center_PDS\";
            //string path = @"D:\FileuploadTest\";

#if DEBUG
            path = @"C:\FileuploadTest\\file1\live\Customer_Center_PDS\";
#endif

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            return path;
        }

        public static string GetEventsContestFilePath()
        {

            string path = @"\\192.168.2.10\file2\f\Job_Files\Event\MBTI\JK8555\";
            //string path = @"D:\FileuploadTest\";

#if DEBUG
            path = @"C:\FileuploadTest\\file2\f\Job_Files\Event\MBTI\JK8555\";
#endif

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            return path;
        }

        public static string GetReviewWriteContestFilePath()
        {
            DateTime nowDate = DateTime.Now;

            string year = nowDate.Year.ToString();
            string month = nowDate.Month.ToString();

            if (month.Length == 1)
            {
                month = "0" + month;
            }

            string path = @"\\192.168.2.10\file2\f\Job_Files\Starter\Resume\" + year + @"\" + month + @"\";
            //string path = @"D:\FileuploadTest\";

#if DEBUG
            path = @"C:\FileuploadTest\\file2\f\Job_Files\Starter\Resume\" + year + @"\" + month + @"\";
#endif

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            return path;
        }

        public static string GetSplashImageFilePath()
        {
            DateTime nowDate = DateTime.Now;

            string year = nowDate.Year.ToString();
            string month = nowDate.Month.ToString();

            if (month.Length == 1)
            {
                month = "0" + month;
            }

            string path = @"\\192.168.2.10\file2\f\Job_Files\Splash\Image\" + year + @"\" + month + @"\";
            //string path = @"D:\FileuploadTest\";

#if DEBUG
            path = @"C:\FileuploadTest\\file2\f\Job_Files\Splash\Image\" + year + @"\" + month + @"\";
#endif

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            return path;
        }

        public static string GetTvcEvent2023FilePath(string giftType)
        {
            DateTime nowDate = DateTime.Now;

            string year = nowDate.Year.ToString();
            string month = nowDate.Month.ToString();

            if (month.Length == 1)
            {
                month = "0" + month;
            }

            string path = @"\\192.168.2.10\file2\f\Job_Files\TvcEvent2023\Image\" + giftType + @"\" + year + @"\" + month + @"\";
            //string path = @"D:\FileuploadTest\";

#if DEBUG
            path = @"C:\FileuploadTest\\file2\f\Job_Files\TvcEvent2023\Image\" + giftType + @"\" + year + @"\" + month + @"\";
#endif

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            return path;
        }

        public static string GetCommonEventFilePath(string Event_Code, int Cntnts_Code, int Cntnts_Order)
        {
            DateTime nowDate = DateTime.Now;

            string year = nowDate.ToString("yyyy");
            string month = nowDate.ToString("MM");
            string detailPath = Event_Code + @"\" + year + @"\" + month + @"\" + Cntnts_Code.ToString() + "_" + Cntnts_Order.ToString() + @"\";
            string path = @"\\192.168.2.10\file2\f\Job_Files\CommonEvent\Image\" + detailPath;
#if DEBUG
            path = @"C:\FileuploadTest\\file2\f\Job_Files\CommonEvent\" + detailPath;
#endif

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            return path;
        }

        public static string GetLogoImageFilePath()
        {
            DateTime nowDate = DateTime.Now;

            string year = nowDate.Year.ToString();
            string month = nowDate.Month.ToString();

            if (month.Length == 1)
            {
                month = "0" + month;
            }

            string path = @"\\192.168.2.10\file2\f\Job_Files\Logo\Image\" + year + @"\" + month + @"\";
            //string path = @"D:\FileuploadTest\";

#if DEBUG
            path = @"C:\FileuploadTest\\file2\f\Job_Files\Logo\Image\" + year + @"\" + month + @"\";
#endif

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            return path;
        }

        public static string GetLossReportPath()
        {
            string path = @"\\192.168.2.10\file2\d\Job_Files\Login\Loss_Report\";

#if DEBUG
            path = @"C:\FileuploadTest\file2\d\Job_Files\Login\Loss_Report\";
#endif
            return path;
        }

        public static string GetLossReportPath(DateTime reqDate)
        {
            string path = GetLossReportPath() + reqDate.Year.ToString() + @"\" + reqDate.Month.ToString() + @"\";
            //string path = @"D:\FileuploadTest\";

#if DEBUG
            path = GetLossReportPath() + reqDate.Year.ToString() + @"\" + reqDate.Month.ToString() + @"\";
#endif
            return path;
        }


        public static string GetUserPhotoPath()
        {
            string path = @"\\192.168.2.10\file1\live\User_Photo\Photo\";
            //string path = @"D:\FileuploadTest\";

#if DEBUG
            path = @"C:\FileuploadTest\file1\live\User_Photo\Photo\";
#endif
            return path;
        }

        public static string GetUserPhotoThumbnailPath()
        {
            string path = @"\\192.168.2.10\file1\live\User_Photo_Thumbnail\Photo\";
            //string path = @"D:\FileuploadTest\";

#if DEBUG
            path = @"C:\FileuploadTest\file1\live\User_Photo_Thumbnail\Photo\";
#endif
            return path;
        }

        public static string GetUserKnowledgePath(DateTime reqDate, string folderNm)
        {
            string path = @"\\192.168.2.10\file1\live\Knowledge\" + folderNm + @"\" + reqDate.Year.ToString() + @"\" + reqDate.Month.ToString() + @"\" + reqDate.Day.ToString() + @"\";
            //string path = @"D:\FileuploadTest\";

#if DEBUG
            path = @"C:\FileuploadTest\file1\live\Knowledge\" + folderNm + @"\" + reqDate.Year.ToString() + @"\" + reqDate.Month.ToString() + @"\" + reqDate.Day.ToString() + @"\";
#endif
            return path;
        }

        public static string GetUserGlobalPath(DateTime reqDate)
        {
            string path = @"\\192.168.2.10\file2\d\Direct\Global\" + reqDate.Year.ToString() + @"\" + reqDate.Month.ToString() + @"\" + reqDate.Day.ToString() + @"\";
            //string path = @"D:\FileuploadTest\";

#if DEBUG
            path = @"C:\FileuploadTest\file2\d\Direct\Global\" + reqDate.Year.ToString() + @"\" + reqDate.Month.ToString() + @"\" + reqDate.Day.ToString() + @"\";
#endif
            return path;
        }

        public static string GetUserContestPath()
        {
            string path = @"\\192.168.2.10\file1\live\contest\files\cntst\";
            //string path = @"D:\FileuploadTest\";

#if DEBUG
            path = @"C:\FileuploadTest\file1\live\contest\files\cntst\";
#endif
            return path;
        }

        public static string GetUnivPartnerSvcLogoFilePath()
        {

            string path = @"\\192.168.2.10\file2\UnivPartnerSvc\Logo\";
            //string path = @"D:\FileuploadTest\";

#if DEBUG
            path = @"C:\FileuploadTest\\file2\UnivPartnerSvc\Logo\";
#endif

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            return path;
        }

        public static string GetPath(string strUserId)
        {
            int UserIdLen = strUserId.Length;
            if (UserIdLen > 4)
            {
                UserIdLen = 4;
            }

            string GetPath = string.Empty;
            for (var k = 1; k <= UserIdLen; k++)
            {
                string ch = strUserId.Substring(k - 1, 1);
                int ch1 = Convert.ToChar(ch);

                if ((ch1 >= 65 && ch1 <= 90) || (ch1 >= 97 && ch1 <= 122) || (ch1 >= 48 && ch1 <= 57))
                {
                    GetPath = GetPath + ch + @"\";
                }
                else
                {
                    GetPath = GetPath + @"_\";
                }
            }

            if (UserIdLen < 4)
            {
                for (var k = 1; k < 4 - UserIdLen; k++)
                {
                    GetPath = GetPath + @"_\";
                }
            }

            return GetPath;
        }

        public static string GetCreateRandomFileName()
        {
            string randomStr = StringHelper.GenerateRandomStringWithNumber(6);
            randomStr = randomStr + String.Format("{0:yyMMddHHmmssff}", DateTime.Now);

            return randomStr;

        }

        /// <summary>
        /// 기업인증 사업자등록증 파일 수집 경로
        /// </summary>
        /// <param name="ImgTypeCode"></param>
        /// <returns></returns>
        public static string GetBizLogkFilePath(int ImgTypeCode)
        {
            string path = @"\\192.168.2.10";

#if DEBUG
            path = @"C:\FileuploadTest";
#endif

            string PathDir = "";
            if (ImgTypeCode == 1 || ImgTypeCode == 5)  // 알바몬 관리자, 제한키워드
            {
                PathDir = string.Format(@"{0}\file2\d\Job_Files\Albamon\co_cert", path);
            }
            else if (ImgTypeCode == 2)   // CS메모
            {
                PathDir = string.Format(@"{0}\img\Albamon_img\csmemo", path);
            }
            else if (ImgTypeCode == 3)   // 마그마
            {
                PathDir = string.Format(@"{0}\file2\d\Job_Files\Albamon\MagmaFile\BizLogk", path);
            }
            else if (ImgTypeCode == 4)   // 정보확인기업
            {
                PathDir = string.Format(@"{0}\file2\d\Job_Files\Albamon\CoFilterKwrd\CoConfirm", path);
            }

            return PathDir;
        }

        private static List<TextUserResponseFileModel> _fileUploadList;
        public static List<TextUserResponseFileModel> FileUploadList
        {
            get
            {
                if (_fileUploadList == null) _fileUploadList = new List<TextUserResponseFileModel>();
                return _fileUploadList;
            }
        }
    }
}