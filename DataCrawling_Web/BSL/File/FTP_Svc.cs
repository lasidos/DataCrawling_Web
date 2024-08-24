using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DataCrawling_Web.BSL.File
{
    public class FTP_Svc
    {
        protected readonly string server;
        protected readonly string port;
        protected readonly string userId;
        protected readonly string pwd;

        public FTP_Svc()
        {
            server = "mkapi.godohosting.com";
            port = "";
            userId = "mkapi";
            pwd = "akvmfzh1!@";
        }

        #region 리스트 가져오기

        public async Task<List<string>> GetFTPList(string path)
        {
            return await Task.FromResult(GetPathList(path));
        }

        private List<string> GetPathList(string path)
        {
            string url = string.Concat("FTP://", server, (port != "" ? port : ""), (path != "" ? path : "/"));
            List<string> fileList = new List<string>();
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(url);
            request.Credentials = new NetworkCredential(userId, pwd);
            request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;

            using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
            {
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.UTF8))
                {
                    string strData = reader.ReadToEnd();

                    string[] filename = strData.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string file in filename)
                    {
                        if (file.IndexOf("<DIR>") > -1) continue;
                        string[] fileDetailes = file.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                        fileList.Add(fileDetailes[fileDetailes.Length - 1]);
                    }
                    return fileList;
                }
            }
        }

        #endregion

        #region 파일 업로드

        public async Task<bool> UpLoad(string folder, string filename)
        {
            return await Task.FromResult(File_Upload(folder, filename));
        }

        private bool File_Upload(string folder, string filename)
        {
            try
            {
                //CreateDirectory(folder);
                FileInfo fileinfo = new FileInfo(filename);

                folder = folder.Replace('\\', '/');
                filename = fileinfo.Name;

                string url = string.Concat("FTP://", server, (port != "" ? port : ""), folder, filename);

                FtpWebRequest ftpWebRequest = (FtpWebRequest)WebRequest.Create(url);
                ftpWebRequest.Method = WebRequestMethods.Ftp.UploadFile;
                ftpWebRequest.Credentials = new NetworkCredential(this.userId, this.pwd);
                ftpWebRequest.KeepAlive = false;
                ftpWebRequest.UseBinary = false;
                ftpWebRequest.UsePassive = false;
                ftpWebRequest.ContentLength = fileinfo.Length;

                int buffLength = 2048;
                byte[] buff = new byte[buffLength];
                int contentLen;

                using (FileStream fs = fileinfo.OpenRead())
                {
                    using (Stream strm = ftpWebRequest.GetRequestStream())
                    {
                        contentLen = fs.Read(buff, 0, buffLength);

                        while (contentLen != 0)
                        {
                            strm.Write(buff, 0, contentLen);
                            contentLen = fs.Read(buff, 0, buffLength);
                        }
                    }
                    fs.Flush();
                    fs.Close();
                }

                if (buff != null)
                {
                    Array.Clear(buff, 0, buff.Length);
                    buff = null;
                }
            }
            catch(Exception e)
            {
                return false;
            }
            return true;
        }

        private void CreateDirectory(string dirName)
        {
            string[] arrDir = dirName.Split('\\');
            string currentDir = string.Empty;

            foreach (string tmpFolder in arrDir)
            {
                try
                {
                    if (string.IsNullOrEmpty(tmpFolder)) continue;

                    currentDir += @"/" + tmpFolder + @"/";
                    string url = $@"FTP://{server}:{port}{currentDir}";

                    FtpWebRequest ftpWebRequest = (FtpWebRequest)WebRequest.Create(url);
                    ftpWebRequest.Credentials = new NetworkCredential(this.userId, this.pwd);
                    ftpWebRequest.Method = WebRequestMethods.Ftp.MakeDirectory;
                    ftpWebRequest.KeepAlive = false;
                    ftpWebRequest.UsePassive = false;

                    FtpWebResponse response = (FtpWebResponse)ftpWebRequest.GetResponse();
                    response.Close();
                }
                catch { }
            }
        }

        #endregion

        #region 다운로드

        public async Task<bool> DownLoad(string localFullPathFile, string serverFullPathFile)
        {
            return await Task.FromResult(File_Download(localFullPathFile, serverFullPathFile));
        }

        private bool File_Download(string localFullPathFile, string serverFullPathFile)
        {
            try
            {
                CheckDirectory(localFullPathFile);

                string url = $@"FTP://{server}:{port}/{serverFullPathFile}";

                FtpWebRequest ftpWebRequest = (FtpWebRequest)WebRequest.Create(url);

                ftpWebRequest.Credentials = new NetworkCredential(userId, pwd);
                ftpWebRequest.KeepAlive = false;
                ftpWebRequest.UseBinary = true;
                ftpWebRequest.UsePassive = false;

                using (FtpWebResponse response = (FtpWebResponse)ftpWebRequest.GetResponse())
                {
                    using (FileStream outputStrem = new FileStream(localFullPathFile, FileMode.Create, FileAccess.Write))
                    {
                        using (Stream ftpStream = response.GetResponseStream())
                        {
                            int bufferSize = 2048;
                            int readCount;
                            byte[] buffer = new byte[bufferSize];

                            readCount = ftpStream.Read(buffer, 0, bufferSize);
                            while (readCount > 0)
                            {
                                outputStrem.Write(buffer, 0, readCount);
                                readCount = ftpStream.Read(buffer, 0, bufferSize);
                            }

                            ftpStream.Close();
                            outputStrem.Close();

                            if (buffer != null)
                            {
                                Array.Clear(buffer, 0, buffer.Length);
                                buffer = null;
                            }
                        }
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void CheckDirectory(string localFullPathFile)
        {
            FileInfo fileinfo = new FileInfo(localFullPathFile);

            if (!fileinfo.Exists)
            {
                DirectoryInfo dirinfo = new DirectoryInfo(fileinfo.DirectoryName);
                if (!dirinfo.Exists) dirinfo.Create();
            }
        }

        #endregion

        #region 파일 삭제

        public async Task<bool> DeleteFTPFile(string path)
        {
            return await Task.FromResult(DeleteFile(path));
        }

        private bool DeleteFile(string path)
        {
            try
            {
                string url = $@"FTP://{server}:{port}/{path}";

                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(url);

                request.Credentials = new NetworkCredential(userId, pwd);

                if (Regex.IsMatch(path, @"(\.)[a-zA-Z0-9ㄱ-ㅎ가-힣]+$")) request.Method = WebRequestMethods.Ftp.DeleteFile;
                else request.Method = WebRequestMethods.Ftp.RemoveDirectory;

                using (request.GetResponse()) { }
            }
            catch
            {
                return false;
            }
            return true;
        }

        #endregion
    }

    public class DirectoryPath
    {
        public string Folder { get; set; }
        public string File { get; set; }
    }
}