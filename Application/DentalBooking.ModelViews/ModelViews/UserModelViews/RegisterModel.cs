using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalBooking.ModelViews.ModelViews.UserModelViews
{
    public class RegisterModel
    {
       
        public string Email { get; set; } = string.Empty;
     
        public string Password { get; set; }   
        public int ClinicId { get; set; }
         public string Role { get; set; }
    }
}
