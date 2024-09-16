using DentalBooking.Core.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalBooking.Contract.Repository.Entity
{
    public class Services : BaseEntity
    {
        public string ServiceName { get; set; } = string.Empty;
        public string Description {  get; set; } = string.Empty;
        public decimal Price {  get; set; }
        
        public virtual ICollection<Appointment_Service>? GetAppointment_Services {  get; set; } 
    }
}
