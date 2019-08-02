using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;

namespace Lib_SQM_Domain.SharedLibs
{
    public static class MailHelper
    {
        public static bool SendMail(MailMessage message)
        {
            bool r = true;

            SmtpClient client = new SmtpClient(Portal_Parameters.SMTPServer);
            try { client.Send(message); }
            catch { r = false; }

            return r;
        }
    }
}
