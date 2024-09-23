using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;


namespace DentalBooking.Contract.Repository.Entity
{
    public class ApplicationUser : IdentityUser
    {
        public bool IsApproved { get; set; } = false;


        public int UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual User? User { get; set; }

    }
}
