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
        
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User? User { get; set; }

        
        public int ClinicId { get; set; }
        [ForeignKey("ClinicId")]
        public virtual Clinic? Clinic { get; set; }

        
        public int TreatmentPlanId { get;set; }
        [ForeignKey("TreatmentPlanId")]
        public virtual TreatmentPlans? TreatmentPlans { get; set; }
        
        public virtual ICollection<Appointment_Service>? Appointment_Services { get; set; }

    }
}
