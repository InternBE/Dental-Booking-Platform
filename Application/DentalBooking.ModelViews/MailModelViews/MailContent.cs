using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalBooking.ModelViews.MailModelViews
{
    public class MailContent
    {
        public string To { get; set; }
        public string ToDisplayName { get; set; } // Tên hiển thị
        public string Subject { get; set; }
        public string Body { get; set; }
    }

}
