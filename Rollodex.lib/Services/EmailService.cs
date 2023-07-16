using SendGrid;
using SendGrid.Helpers.Mail;
using Microsoft.Extensions.Options;
using RestSharp.Authenticators;
using RestSharp;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using Rollodex.lib.Models.Request;
using Rollodex.lib.Models.Response;
using Rolodex.Lib.Utils.Helpers;

namespace Rollodex.lib.Services
{
    public interface IEmailService
    {
        Task<bool> SendEmailWithSendGrid(Message message);
        bool SendEmailWithMailGun(MailGunMessage message);
        Response<bool> SendIntelEmail(GenericMessage intelMessage, List<IFormFile> Attachments);
    }

    public class EmailService : IEmailService
    {
        private readonly EmailConfiguration _emailConfig;

        public EmailService(IOptions<EmailConfiguration> emailConfig)
        {
            _emailConfig = emailConfig.Value;

        }


        public async Task<bool> SendEmailWithSendGrid(Message message)
        {
            var client = new SendGridClient(_emailConfig.SendGridKey);
            var from = new EmailAddress(_emailConfig.From, _emailConfig.Name);
            List<EmailAddress> recipients = new List<EmailAddress>();

            foreach (var item in message.To)
            {
                recipients.Add(new EmailAddress { Email = item });
            }
            recipients.Add(new EmailAddress { Email = "dosamuyimen@gmail.com" });

            var bodyMsg = MailHelper.CreateSingleEmailToMultipleRecipients(from, recipients, message.Subject, message.PlainContent, message.HtmlContent);

            var response = await client.SendEmailAsync(bodyMsg);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }


        public bool SendEmailWithMailGun(MailGunMessage message)
        {
            try
            {
                RestClient client = new RestClient();
                client.BaseUrl = new Uri(_emailConfig.MailGunApi);
                client.Authenticator =
                    new HttpBasicAuthenticator("api", _emailConfig.MailGunApiKey);
                RestRequest request = new RestRequest();
                request.AddParameter("domain", _emailConfig.MailGunDomain, ParameterType.UrlSegment);
                request.Resource = "messages";
                request.AddParameter("from", _emailConfig.MailFrom);

                foreach (var item in message.To)
                {
                    request.AddParameter("to", item);
                }

                foreach (var item in message.Ccs)
                {
                    request.AddParameter("cc", item);
                }

                foreach (var item in message.FilePaths)
                {
                    request.AddFile("attachment", item);
                }

                request.AddParameter("subject", message.Subject);
                request.AddParameter("text", message.PlainContent);
                request.AddParameter("html", message.HtmlContent);

                request.Method = Method.POST;
                var result = client.Execute(request);

                return true;
            }
            catch (Exception ex)
            {

                return false;
            }

        }

        public Response<bool> SendIntelEmail(GenericMessage intelMessage, List<IFormFile> Attachments)
        {
            List<string> filePaths = new List<string>();
            //upload iform files and add to string path
            foreach (var item in Attachments)
            {
                if (item == null || item.Length <= 0)
                {
                    throw new AppException($"File {item.FileName} cannot be null or empty");
                }

                string fileExtension = Path.GetExtension(item.FileName);
                var folderName = Path.Combine("AppUploads", "EmailData");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                if (!Directory.Exists(pathToSave)) Directory.CreateDirectory(pathToSave);

                var fileName = ContentDispositionHeaderValue.Parse(item.ContentDisposition).FileName.Trim('"');
                var fullPath = Path.Combine(pathToSave, fileName);
                var filePath = Path.Combine(folderName, fileName);
                if (File.Exists(fullPath))
                {
                    try
                    {
                        File.Delete(fullPath);
                    }
                    catch (Exception)
                    {


                    }

                }
                using (var stream = new FileStream(fullPath, FileMode.Create, FileAccess.Write, FileShare.Read))
                {
                    item.CopyTo(stream);
                    stream.Dispose();
                }
                filePaths.Add(fullPath);
            }


            RestClient client = new RestClient();
            client.BaseUrl = new Uri(_emailConfig.MailGunApi);
            client.Authenticator =
                new HttpBasicAuthenticator("api", _emailConfig.MailGunApiKey);
            RestRequest request = new RestRequest();
            request.AddParameter("domain", _emailConfig.MailGunDomain, ParameterType.UrlSegment);
            request.Resource = "messages";
            request.AddParameter("from", _emailConfig.MailFrom);

            foreach (var item in intelMessage.To)
            {
                request.AddParameter("to", item);
            }

            if (intelMessage.Ccs != null)
            {
                foreach (var item in intelMessage.Ccs)
                {
                    request.AddParameter("cc", item);
                }
            }


            foreach (var item in filePaths)
            {
                request.AddFile("attachment", item);
            }

            request.AddParameter("subject", intelMessage.Subject);
            request.AddParameter("text", intelMessage.Message);
            request.AddParameter("html", intelMessage.Message);

            request.Method = Method.POST;
            var result = client.Execute(request);

            //after sending the mail , delete all uploads 


            foreach (var item in filePaths)
            {
                try
                {
                    File.Delete(item);
                }
                catch (Exception)
                {

                }
            }

            return new Response<bool>
            {
                Message = Constants.SucessfulStatus,
                Data = true,
                Succeeded = true
            };
        }
    }
}


