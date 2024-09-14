using DentalBooking.Core.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalBooking.Contract.Repository.Entity
{
    public class Appointment : BaseEntity
    {
        public DateTime AppointmentDate { get; set; }
        public int DentistId { get; set; }
        public string Status { get; set; } = "Pending";

        //ForeignKey
        [ForeignKey("User")]
        public int UserId { get; set; }
        public virtual User? User { get; set; }

        [ForeignKey("Clinic")]
        public int ClinicId { get; set; }
        public virtual Clinic? Clinic { get; set; }

        [ForeignKey("TreatmentPlans")]
        public int TreatmentPlanId { get;set; }
        public virtual TreatmentPlans? TreatmentPlans { get; set; }
        
        public virtual ICollection<Appointment_Service>? Appointment_Services { get; set; }

    }
}
