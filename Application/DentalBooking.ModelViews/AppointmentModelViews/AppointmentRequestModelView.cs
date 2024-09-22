using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalBooking.ModelViews.AppointmentModelViews
{
    public class AppointmentRequestModelView
    {
        [Required]
        public DateTime AppointmentDate { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int ClinicId { get; set; }

        [Required]
        public int TreatmentPlanId { get; set; }

        public string Status { get; set; } = "Pending"; // Giá trị mặc định là "Pending"
    }
}
