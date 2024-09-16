using DentalBooking.Core.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalBooking.Contract.Repository.Entity
{
    public class Appointment_Service : BaseEntity
    {
        [ForeignKey("Appointment")]
        public int AppointmentId { get; set; }
        public virtual Appointment? Appointment { get; set; }

        [ForeignKey("Services")]
        public int ServiceId { get; set; }
        public virtual Services? Services { get; set; }
    }
}
