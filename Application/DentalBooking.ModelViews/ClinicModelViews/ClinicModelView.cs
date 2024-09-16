using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalBooking.ModelViews.ClinicModelViews
{
    public class ClinicModelView
    {
        public int Id { get; set; }
        public string ClinicName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public TimeOnly OpeningTime { get; set; }
        public TimeOnly ClosingTime { get; set; }
        public int SlotDurationMinutes { get; set; }
        public int MaxPatientsPerSlot { get; set; }
        public int MaxTreatmentPerSlot { get; set; }
    }
}
