using System;
using System.Net;
using System.Web;

namespace DataCrawling_Web.BSL.Common
{
    public static class NetFunction
    {
        #region [Client IP Address 가져오기]
        /// <summary>
        /// Client IP Address 가져오기
        /// </summary>
        public static String getClientIP
        {
            get
            {
                String clientIP = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

                if (String.IsNullOrEmpty(clientIP))
                    clientIP = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

                if (String.IsNullOrEmpty(clientIP))
                    clientIP = HttpContext.Current.Request.UserHostAddress;

                if (String.IsNullOrEmpty(clientIP) || clientIP.Trim() == "127.0.0.1" || clientIP.Trim() == "::1")
                {
                    IPHostEntry hostEntry = Dns.GetHostEntry(Dns.GetHostName());
                    IPAddress[] arrIpAddr = Array.FindAll(
                    Dns.GetHostEntry(string.Empty).AddressList,
                    a => a.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);

                    try
                    {
                        clientIP = arrIpAddr[0].ToString();
                    }
                    catch
                    {
                        try
                        {
                            clientIP = arrIpAddr[0].ToString();
                        }
                        catch
                        {
                            try
                            {
                                arrIpAddr = Dns.GetHostAddresses(Dns.GetHostName());
                                clientIP = arrIpAddr[0].ToString();
                            }
                            catch
                            {
                                clientIP = "127.0.0.1";
                            }
                        }
                    }
                }

                return clientIP;
            }
        }
        #endregion

        #region [내부IP 여부]
        public static bool IsInternalIP(string IP)
        {
            return (IP.Contains("221.148.253") || IP.Contains("121.138.164") || IP.Contains("172.16.160") || IP.Contains("172.16.170") || IP.Contains("172.16.200"));
        }
        #endregion

        #region [내부 서버 IP 여부]
        public static bool IsInternalServerIP(string IP)
        {
            // VOD 업로드 서버 : 211.218.145.129 / 211.218.145.130 / 125.141.216.89 / 125.141.216.90
            return (IP.Contains("172.16.48") || IP.Contains("172.16.49") | IP.Contains("121.189.48") || IP.Contains("121.189.49")
                || IP.Equals("125.141.216.89") || IP.Equals("125.141.216.90") || IP.Equals("211.218.145.129") || IP.Equals("211.218.145.130"));
        }
        #endregion
    }
}