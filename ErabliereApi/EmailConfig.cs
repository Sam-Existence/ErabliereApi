using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ErabliereApi
{
    public class EmailConfig
    {
        public string Sender { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string SmtpServer { get; set; }
        public int SmtpPort { get; set; }
    }
}
