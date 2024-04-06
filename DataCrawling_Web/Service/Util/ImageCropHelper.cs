using ImageProcessor;
using ImageProcessor.Imaging.Formats;
using System;
using System.Drawing;
using System.IO;
using System.Linq;

namespace DataCrawling_Web.Service.Util
{
    public class ImageCropHelper
    {
        /// <summary>
        /// 원본 파일경로
        /// </summary>
        private string _originFilePath;

        /// <summary>
        /// 기본 파일경로
        /// </summary>
        public string originFilePath { get { return _originFilePath; } }

        /// <summary>
        /// 파일명
        /// </summary>
        private string _fileName;

        /// <summary>
        /// 파일명
        /// </summary>
        public string FileName { get { return _fileName; } }

        /// <summary>
        /// 파일명
        /// </summary>
        private string _realFilePath;

        /// <summary>
        /// 파일명
        /// </summary>
        public string RealFilePath { get { return _realFilePath; } }




        /// <summary>
        /// 파일업로드 경로전체
        /// </summary>
        public string FileFullPath { get { return _originFilePath; } }

        /// <summary>
        /// 파일업로드 경로전체
        /// </summary>
        public string FileRealSavePath { get { return Path.Combine(_realFilePath + _fileName); } }


        public string FileExtention { get; set; }

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="filePath">파일업로드 기본페스</param>
        /// <param name="baseFIleName">파일명기본 규칙</param>

        public ImageCropHelper(string filePath, string baseFIleName, string realFilePath)
        {
            var fileSplit = baseFIleName.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            var fileSpliter = fileSplit.Take(fileSplit.Count() - 1).Union(new string[] { "jpg" });

            _originFilePath = filePath;
            _fileName = string.Join(".", fileSpliter);
            _realFilePath = realFilePath;


        }


        /// <summary>
        /// 이미지 파일 Crop
        /// </summary>
        /// <param name="postfile">첨부파일개체</param>
        /// <param name="isvirus">바이러스 검출유무</param>
        /// <param name="width">가로길이만 입력할경우 가로기준으로 리사이즈</param>
        /// <param name="height">세로길이만 입력할경우 세로기준 리스아즈</param>
        /// <param name="isAuotRotate">exif데이터 기준 사진 각도 자동회전</param>
        /// <returns></returns>
        public bool CropImage(int ow = 0, int oh = 0, int cropSize_nx = 0, int cropSize_ny = 0, int cropSize_nw = 0, int cropSize_nh = 0, bool isTemp = false, bool isCrop = true)
        {
            try
            {
                //파일형식 변환
                ISupportedImageFormat format = new JpegFormat { Quality = 100 };

                //var overWriteFilePath = Path.Combine(FileFullPath);
                var originFilePath = FileFullPath;
                var FileRealPath = RealFilePath;

                if (isTemp)
                {
                    FileRealPath = FileRealSavePath;
                }

                //if (!System.IO.File.Exists(overWriteFilePath))
                //{
                //   isvirus = false;
                //   return false;
                //}

                byte[] photoBytes = System.IO.File.ReadAllBytes(originFilePath);

                Size size = new Size(cropSize_nw, cropSize_nh);
                Size Origin_size = new Size(ow, oh);
                using (MemoryStream inStream = new MemoryStream(photoBytes))
                {
                    using (FileStream outStream = new FileStream(FileRealPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
                    {
                        // Initialize the ImageFactory using the overload to preserve EXIF metadata.
                        using (ImageFactory imageFactory = new ImageFactory(preserveExifData: true))
                        {
                            var cropLayer = new Rectangle(cropSize_nx, cropSize_ny, cropSize_nw, cropSize_nh);
                            // Load, resize, set the format and quality and save an image.
                            imageFactory.Load(inStream);

                            imageFactory.Resize(Origin_size);

                            //crop 등록을 요청 했을경우만
                            if (isCrop)
                            {
                                imageFactory.Crop(cropLayer)
                                         .Resize(size);
                            }

                            imageFactory.Format(format)
                                     .Save(outStream);
                        }
                    }
                }
                return true;

            }
            catch (Exception ex)
            {
                return false;
            }


        }
    }


}
