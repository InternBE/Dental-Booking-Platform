using DentalBooking.Core.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DentalBooking.Contract.Repository.Entity
{
    public class Appointment : BaseEntity
    {
        public DateTime AppointmentDate { get; set; }
        public string Status { get; set; } = "Pending";
        //public int DentistId { get; set; }

        // ForeignKey
        public int UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual User? User { get; set; }

        public int ClinicId { get; set; }
        [ForeignKey(nameof(ClinicId))]
        public virtual Clinic? Clinic { get; set; }

        public int TreatmentPlanId { get; set; }
        [ForeignKey(nameof(TreatmentPlanId))]
        public virtual TreatmentPlans? TreatmentPlans { get; set; }

        public virtual ICollection<Appointment_Service>? Appointment_Services { get; set; }
    }
}
