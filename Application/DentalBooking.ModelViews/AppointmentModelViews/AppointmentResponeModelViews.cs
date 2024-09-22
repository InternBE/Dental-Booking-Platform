using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalBooking.ModelViews.AppointmentModelViews
{
    public class AppointmentResponeModelViews
    {
        public DateTime AppointmentDate { get; set; }
        public string? Status { get; set; } = "Pending";

        public int UserId { get; set; }           // Thêm UserId
        public int ClinicId { get; set; }         // Thêm ClinicId
        public int TreatmentPlanId { get; set; }  // Thêm TreatmentPlanId

        // Include related entities if needed
    }
}
