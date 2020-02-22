using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Web.Helpers
{
    public interface IEmailHelper
    {
        void SendMail(string to, string subject, string body);
    }
}
