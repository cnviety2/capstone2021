using Capstone2021.DTO;
using System;
using System.Net.Mail;

namespace Capstone2021.Utils
{
    public static class EmailUtils
    {
        private readonly static string gmail = "cnviety9898@gmail.com";
        private readonly static string password = "bulilin63047";

        private static bool RemoteServerCertificateValidationCallback(object sender,
            System.Security.Cryptography.X509Certificates.X509Certificate certificate,
            System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            //Console.WriteLine(certificate);
            return true;
        }

        public static ResponseDTO sendEmail(string content, string to)
        {
            ResponseDTO response = new ResponseDTO();
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(gmail, "Trung tâm hỗ trợ sinh viên TPHCM");
            mail.To.Add(new MailAddress(to));
            mail.SubjectEncoding = System.Text.Encoding.UTF8;
            mail.Subject = "Lấy lại MK";
            mail.IsBodyHtml = true;
            mail.BodyEncoding = System.Text.Encoding.UTF8;
            mail.Body = "<p>Sử dụng mã này để lấy lại mật khẩu: " + content + "</p>";

            using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587))
            {
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new System.Net.NetworkCredential(gmail, password);
                smtpClient.Host = "smtp.gmail.com";
                smtpClient.Port = 587;
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                try
                {
                    smtpClient.Send(mail);
                }
                catch (Exception e)
                {
                    response.data = e;
                    response.message = "ERROR";
                    return response;
                }
            }
            response.message = "OK";
            return response;
        }

        //Gửi email liên hệ tới cho SAC,tạm thời chưa có tài khoản mail của sac
        public static ResponseDTO sendEmailToSAC(SendEmailToSACDTO dto)
        {
            ResponseDTO response = new ResponseDTO();
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(gmail, "Liên hệ từ :" + dto.name);
            mail.To.Add(new MailAddress(gmail));//chỗ này sẽ là mail của SAC
            mail.SubjectEncoding = System.Text.Encoding.UTF8;
            mail.Subject = dto.subject;
            mail.IsBodyHtml = true;
            mail.BodyEncoding = System.Text.Encoding.UTF8;
            mail.Body = "<p>Nội dung : " + dto.content + "</p><p>Email của người liên hệ : " + dto.email + "</p>";

            using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587))
            {
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new System.Net.NetworkCredential(gmail, password);
                smtpClient.Host = "smtp.gmail.com";
                smtpClient.Port = 587;
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                try
                {
                    smtpClient.Send(mail);
                }
                catch (Exception e)
                {
                    response.data = e;
                    response.message = "ERROR";
                    return response;
                }
            }
            response.message = "OK";
            return response;
        }
    }
}