using DentalBooking.Core.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalBooking.Contract.Repository.Entity
{
    public class Clinic : BaseEntity
    {
        //public int ClinicId { get; set; }
        public string ClinicName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string PhoneNumber {  get; set; } = string.Empty;
        public TimeOnly OpeningTime {  get; set; }
        public TimeOnly ClosingTime { get; set; }
        public int SlotDurationMinutes { get; set; }
        public int MaxPatientsPerSlot { get; set; }
        public int MaxTreatmentPerSlot { get; set; }
        
        //References
        public virtual ICollection<User>? Users { get; set; }
        public virtual ICollection<Appointment>? Appointments { get; set; }
    }
}
