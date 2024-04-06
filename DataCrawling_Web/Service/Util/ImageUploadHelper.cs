using ImageProcessor;
using ImageProcessor.Imaging.Formats;
using System;
using System.Drawing;
using System.IO;
using System.Web;

namespace DataCrawling_Web.Service.Util
{
    public class ImageUploadHelper
    {
        /// <summary>
        /// 기본파일경로
        /// </summary>
        private string _filePath;

        /// <summary>
        /// 기본 파일경로
        /// </summary>
        public string FilePath { get { return _filePath; } }

        /// <summary>
        /// 파일명
        /// </summary>
        private string _fileName;

        /// <summary>
        /// 파일명
        /// </summary>
        public string FileName { get { return _fileName; } }


        /// <summary>
        /// 파일업로드 경로전체
        /// </summary>
        public string FileFullPath { get { return Path.Combine(_filePath, _fileName); } }


        public string FileExtention { get; set; }

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="filePath">파일업로드 기본페스</param>
        /// <param name="baseFIleName">파일명기본 규칙</param>

        public ImageUploadHelper(string filePath, string baseFIleName)
        {
            _filePath = filePath;
            this.GenerateUniqueFileName(baseFIleName);

        }

        /// <summary>
        /// 유니크 파일명 생성
        /// </summary>
        /// <param name="basefileName"></param>
        private void GenerateUniqueFileName(string basefileName)
        {
            var fileName = string.Empty;
            do
            {
                _fileName = string.Concat(basefileName, Path.GetRandomFileName().Substring(0, 5));

            }
            while (System.IO.File.Exists(FileFullPath));
        }

        /// <summary>
        /// 이미지 파일업로드 원본크기
        /// </summary>
        /// <param name="postfile">첨부파일개체</param>
        /// <param name="isvirus">바이러스 검출유무</param>

        public bool WriteImage(HttpPostedFileBase postfile, out string ext)
        {
            return this.WriteImage(postfile, out ext, 0, 0, true);
        }

        /// <summary>
        /// 이미지 파일업로드 원본크기
        /// </summary>
        /// <param name="postfile">첨부파일개체</param>
        /// <param name="isvirus">바이러스 검출유무</param>      
        /// <param name="isAuotRotate">exif데이터 기준 사진 각도 자동회전</param
        /// <returns></returns>
        public bool WriteImage(HttpPostedFileBase postfile, out string ext, bool isAuotRotate = true)
        {
            return this.WriteImage(postfile, out ext, 0, 0, isAuotRotate);
        }

        /// <summary>
        /// 이미지 파일업로드
        /// </summary>
        /// <param name="postfile">첨부파일개체</param>
        /// <param name="isvirus">바이러스 검출유무</param>
        /// <param name="width">가로길이만 입력할경우 가로기준으로 리사이즈</param>
        /// <param name="height">세로길이만 입력할경우 세로기준 리스아즈</param>
        /// <param name="isAuotRotate">exif데이터 기준 사진 각도 자동회전</param>
        /// <returns></returns>
        public bool WriteImage(HttpPostedFileBase postfile, out string ext, int width = 0, int height = 0, bool isAuotRotate = true)
        {
            try
            {

                //파일형식 변환
                //ISupportedImageFormat format = new JpegFormat { Quality = 100 };

                //System.Drawing.Image img = System.Drawing.Image.FromStream(file.InputStream);
                var extention = string.Empty;

                byte[] fileData = null;
                using (var binaryReader = new BinaryReader(postfile.InputStream))
                {
                    fileData = binaryReader.ReadBytes(postfile.ContentLength);
                    postfile.InputStream.Position = 0;

                    using (MemoryStream inStream = new MemoryStream(fileData))
                    {

                        using (ImageFactory imageFactory = new ImageFactory(preserveExifData: false))
                        {
                            var resize = new Size(width, height);

                            // Load, resize, set the format and quality and save an image.
                            imageFactory.Load(postfile.InputStream);

                            var currentFormat = imageFactory.CurrentImageFormat;
                            currentFormat.Quality = 100;

                            //extention = currentFormat.DefaultExtension;
                            extention = GetExtentionName(currentFormat);

                            imageFactory.Resize(resize)
                                        .Format(currentFormat);

                            if (isAuotRotate)
                            {
                                imageFactory.AutoRotate();
                            }

                            //fullpath를 변경해야함
                            using (FileStream outStream = new FileStream(string.Format("{0}.{1}", FileFullPath, extention), FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
                            {
                                imageFactory.Save(outStream);
                            }
                        }
                    }
                }

                ext = extention;
                return true;

            }
            catch (Exception ex)
            {
                ext = "";
                return false;
            }


        }

        private string GetExtentionName(ISupportedImageFormat format)
        {
            if (format is JpegFormat)
            {
                return "jpg";
            }
            else if (format is GifFormat)
            {
                return "gif";
            }
            else if (format is PngFormat)
            {
                return "png";
            }
            else if (format is BitmapFormat)
            {
                return "bmp";
            }
            else
            {
                return "jpg";
            }
        }
    }

}