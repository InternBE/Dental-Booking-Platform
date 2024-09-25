using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalBooking.ModelViews.DentistModelViews
{
    public class DentistResponseModelView
    {
        public int Id { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string Status { get; set; }
        public int UserId { get; set; }
        public int ClinicId { get; set; }
        public int TreatmentPlanId { get; set; }
    }
}
