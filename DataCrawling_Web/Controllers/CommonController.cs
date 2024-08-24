using DataCrawling_Web.BSL.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DataCrawling_Web.Controllers
{
    public class CommonController : Controller
    {
        public string uploadSummernoteImageFile(HttpPostedFileBase file)
        {
            string savePath = null;
            var fileName = string.Empty;
            if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
            {
                fileName = Path.GetFileName(file.FileName);
                // store the file inside ~/App_Data/uploads folder
                string path = Path.Combine(Server.MapPath("~/Uploads/Temp"), fileName);
                var WorkingImageId = Guid.NewGuid();
                Image image = ProcessUploadedImage(file);

                var WorkingImageExtension = Path.GetExtension(file.FileName).ToLower();
                DirectoryInfo dir = new DirectoryInfo(Server.MapPath("/Uploads/Temp"));
                if (!dir.Exists) dir.Create();
                fileName = WorkingImageId + WorkingImageExtension;
                savePath = "/Uploads/Temp/" + fileName;
                image.Save(Server.MapPath("/Uploads/Temp") + @"\" + fileName);

                //fileName = file.FileName;
                //string fileContentType = file.ContentType;
                //byte[] fileBytes = new byte[file.ContentLength];
                //var data = file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));

                // 처리
            }
            return savePath;
        }

        private Image ProcessUploadedImage(HttpPostedFileBase file)
        {
            var WorkingImageExtension = Path.GetExtension(file.FileName).ToLower();
            string[] allowedExtensions = { ".png", ".jpeg", ".jpg", ".gif" }; // Make sure it is an image that can be processed
            if (allowedExtensions.Contains(WorkingImageExtension))
            {

                Image workingImage = new Bitmap(file.InputStream);

                workingImage = ResizeImage(workingImage);

                return workingImage;
            }
            else
            {
                throw new Exception("Cannot process files of this type.");
            }
        }

        private Image ResizeImage(Image imgPhoto)
        {
            int logoSize = 800;

            float sourceWidth = imgPhoto.Width;
            float sourceHeight = imgPhoto.Height;
            float destHeight = 0;
            float destWidth = 0;
            int sourceX = 0;
            int sourceY = 0;
            int destX = 0;
            int destY = 0;

            // Resize Image to have the height = logoSize/2 or width = logoSize.
            // Height is greater than width, set Height = logoSize and resize width accordingly
            if (sourceWidth > (2 * sourceHeight))
            {
                destWidth = logoSize;
                destHeight = (float)(sourceHeight * logoSize / sourceWidth);
            }
            else
            {
                int h = logoSize / 2;
                destHeight = h;
                destWidth = (float)(sourceWidth * h / sourceHeight);
            }
            // Width is greater than height, set Width = logoSize and resize height accordingly

            Bitmap bmPhoto = new Bitmap((int)destWidth, (int)destHeight,
                                        PixelFormat.Format32bppPArgb);
            bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;

            grPhoto.DrawImage(imgPhoto,
                new Rectangle(destX, destY, (int)destWidth, (int)destHeight),
                new Rectangle(sourceX, sourceY, (int)sourceWidth, (int)sourceHeight),
                GraphicsUnit.Pixel);

            grPhoto.Dispose();

            return bmPhoto;
        }
    }
}