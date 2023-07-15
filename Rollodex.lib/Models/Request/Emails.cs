using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rollodex.lib.Models.Request
{
    public class GenericMessage
    {
        public List<string> To { get; set; }
        public List<string>? Ccs { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }

    }

    public class MailGunMessage
    {
        public MailGunMessage()
        {
            FilePaths = new List<string>();
            To = new List<string>();
            Ccs = new List<string>();
            PlainContent = string.Empty;
            HtmlContent = string.Empty;
        }
        public List<string> To { get; set; }
        public List<string> Ccs { get; set; }
        public string Subject { get; set; }
        public string HtmlContent { get; set; }
        public string PlainContent { get; set; }
        public List<string> FilePaths { get; set; }

    }

    public class EmailConfiguration
    {
        public string From { get; set; }
        public string Name { get; set; }
        public string SendGridKey { get; set; }
        public string MailGunApiKey { get; set; }
        public string MailGunDomain { get; set; }
        public string MailGunApi { get; set; }
        public string MailFrom { get; set; }
    }

    public class Message
    {
        public List<string> To { get; set; }
        public List<string> Ccs { get; set; }
        public string Subject { get; set; }
        public string HtmlContent { get; set; }
        public string PlainContent { get; set; }
        public List<string> FilePaths { get; set; }

        public List<Attachment> Attachments { get; set; }


    }

}
