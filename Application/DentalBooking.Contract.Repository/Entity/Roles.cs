using DentalBooking.Core.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalBooking.Contract.Repository.Entity
{
    public class Roles : BaseEntity
    {
        public string RolesName { get; set; } = string.Empty;
        public virtual ICollection<User>? Users { get; set; }
    }
}
