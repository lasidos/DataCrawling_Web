using System;
using System.Configuration;
using System.Net;
using System.Net.Configuration;
using System.Net.Mail;

namespace DataCrawling_Web.BSL.Smtp
{
    public class Smtp
    {
        /// <summary>
        /// 메일발송
        /// </summary>
        /// <param name="title">메일 제목</param>
        /// <param name="content">메일 내용</param>
        /// <param name="receiveID">수신자메일</param>
        /// <returns></returns>
        public string SendMail(string title, string content, string receiveID)
        {
            string msg = "ok";

            string senderName = "myplatformkorea";
            string senderID = "myplatformkorea@gmail.com";

            try
            {
                //SMTP서버 설정 읽어옴 > Web.config에 정의됨
                SmtpSection smtpConfig = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");

                //메일 컨텐츠 설정 (발송자, 수신자, 메일제목, 메일내용 등..)
                MailMessage message = new MailMessage();
                message.From = new MailAddress(senderID, senderName); //new MailAddress(발송자메일, 발송자명) 설정 시 : 받은메일함에 메일 주소가아닌 보낸이 이름이 표시된다. (발송자명은 옵션)
                message.To.Add(new MailAddress(receiveID));
                message.Subject = title;
                message.Body = content;
                message.SubjectEncoding = System.Text.Encoding.UTF8;  //메일 제목의 Encoding을 UTF8로 설정
                message.BodyEncoding = System.Text.Encoding.UTF8;     //메일 내용의 Encoding을 UTF8로 설정
                message.IsBodyHtml = true;                            //메일 본문을 HTML형식을 지원하도록 설정

                //SMTP 설정
                SmtpClient smtpClient = new SmtpClient(smtpConfig.Network.Host, smtpConfig.Network.Port);
                smtpClient.UseDefaultCredentials = false;
                //SMTP서버로부터 인증을 받기위한 Credentials 생성
                NetworkCredential networkCred = new NetworkCredential(smtpConfig.From, smtpConfig.Network.Password);
                smtpClient.Credentials = networkCred;
                //SSL접속을 할 수 있도록 EnableSsl을 True로 설정 (구글은 필수, 미설정 시 메일 발송이 되지않음.)
                smtpClient.EnableSsl = true;
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;

                //Send 메서드를 이용하여 메일을 발송한다.
                smtpClient.Send(message);
            }
            catch (Exception e)
            {
                msg = e.Message;
            }

            return msg;
        }
    }
}