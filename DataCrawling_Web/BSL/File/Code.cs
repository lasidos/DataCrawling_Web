using System.Collections.Generic;

namespace DataCrawling_Web.BSL.File
{
    public static class Code
    {
        /// <summary>
        /// 업로드 가능한 파일 확장자
        /// </summary>
        public static List<string> AcceptFileExt
        {
            get
            {
                List<string> fileExt = new List<string>
                {
                    ".hwp",
                    ".doc",
                    ".docx",
                    ".ppt",
                    ".pptx",
                    ".xls",
                    ".xlsx",
                    ".pdf",
                    ".txt",
                    ".rtf",
                    ".gul",
                    ".jpg",
                    ".jpeg",
                    ".gif",
                    ".bmp",
                    ".png",
                    ".psd",
                    ".ai",
                    ".swf",
                    ".zip",
                    ".alz"
                };

                return fileExt;
            }
        }

        /// <summary>
        /// 공통 업로드 가능한 파일 확장자
        /// </summary>
        public static List<string> CommonAcceptFileExt
        {
            get
            {
                List<string> fileExt = new List<string>
                {
                    ".jpg",
                    ".jpeg",
                    ".gif",
                    ".bmp",
                    ".png"
                };

                return fileExt;
            }
        }

        public static List<string> CommonTempFolderService
        {
            get
            {
                List<string> TempFolderService = new List<string>
                {
                    "qstn"
                };
                return TempFolderService;
            }
        }

        public static List<string> AcceptTxt
        {
            get
            {
                List<string> MimeType = AcceptMimeType;
                MimeType.Add("text/plain");
                return MimeType;
            }
        }

        /// <summary>
        /// 업로드 가능한 MimeType
        /// </summary>
        public static List<string> AcceptMimeType
        {
            /**
             * 각종 문서파일(hwp, doc, docx, ppt, pptx, xls, xlsx, pdf), 압축파일(zip), 이미지파일(jpg, gif) 참고주소 http://www.pcpitstop.com/file-extension/open/extension/How~to~open~PNG~Image/png.html
             * hwp : application/haansofthwp, application/hwp, application/x-hwp
             * doc : application/haansoftdoc, application/msword, application/pdf.a520491b_3bf7_494d_8855_7fac2c6c0608, application/softgrid-doc, application/vnd.msword, x-softmaker-tm
             * docx : application/docxconverter, application/haansoftdocx, application/softgrid-docx, application/vnd.ms-word.document.12, application/vnd.openxmlformats-officedocument.wordprocessingml.document, x-softmaker-tm
             * ppt : application/haansoftppt, application/mspowerpoint, application/softgrid-ppt, application/vnd.ms-powerpoint, application/x-mspowerpoint, x-softmaker-pr
             * pptx : application/haansoftpptx, application/ppt, application/softgrid-pptx, application/vnd.ms-powerpoint.12, application/vnd.ms-powerpoint.presentation.12, application/vnd.openxmlformats-officedocument.presentationml.presentation, x-softmaker-pr
             * xls : application/haansoftxls, application/msexcell, application/softgrid-xls, application/vnd.ms-excel, x-softmaker-pm
             * xlsx : application/docxconverter, application/haansoftxlsx, application/vnd.ms-excel.12, application/vnd.openxmlformats-officedocument.spreadsheetml.sheet, x-softmaker-pm
             * pdf : application/adobe-file, application/pdf, image/pdf
             * zip : application/stuffit, application/x-zip, application/x-zip-compressed, application/zip, image/zip
             * jpg : application/soundpix, image/jpeg, image/jpeg; image/spj, image/jpeg-x, image/jpg
             * gif : image/gif
             */
            get
            {
                List<string> mimeType = new List<string>
                {
                    "application/octet-stream",
                    "application/haansofthwp",
                    "application/hwp",
                    "application/x-hwp",
                    "application/haansoftdoc",
                    "application/vnd.hancom.hwp",
                    "application/msword",
                    "application/pdf.a520491b_3bf7_494d_8855_7fac2c6c0608",
                    "application/softgrid-doc",
                    "application/vnd.msword",
                    "x-softmaker-tm",
                    "application/docxconverter",
                    "application/haansoftdocx",
                    "application/softgrid-docx",
                    "application/vnd.ms-word.document.12",
                    "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                    "application/haansoftppt",
                    "application/mspowerpoint",
                    "application/softgrid-ppt",
                    "application/vnd.ms-powerpoint",
                    "application/x-mspowerpoint",
                    "x-softmaker-pr",
                    "application/haansoftpptx",
                    "application/ppt",
                    "application/softgrid-pptx",
                    "application/vnd.ms-powerpoint.12",
                    "application/vnd.ms-powerpoint.presentation.12",
                    "application/vnd.openxmlformats-officedocument.presentationml.presentation",
                    "application/haansoftxls",
                    "application/msexcell",
                    "application/softgrid-xls",
                    "application/vnd.ms-excel",
                    "x-softmaker-pm",
                    "application/haansoftxlsx",
                    "application/vnd.ms-excel.12",
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    "application/adobe-file",
                    "application/pdf",
                    "image/pdf",
                    "application/stuffit",
                    "application/x-zip",
                    "application/x-zip-compressed",
                    "application/zip",
                    "image/zip",
                    "application/soundpix",
                    "image/jpeg",
                    "image/spj",
                    "image/jpeg-x",
                    "image/jpg",
                    "image/gif",
                    "image/x-png",
                    "image/png",
                    "image/pjpeg",
                    "text/plain",
                    "application/x-hwp"
                };
                //mimeType.Add("application/unknown");
                //mimeType.Add("document/unknown");
                //mimeType.Add("application/force-download");

                return mimeType;
            }
        }
    }
}