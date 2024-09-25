using DentalBooking.ModelViews.AppointmentModelViews;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalBooking.ModelViews.TreatmentPlanModels
{
    public class TreatmentPlanRequestModelView
    {
        [Required]
        public string Description { get; set; } = string.Empty;  // Mô tả điều trị
        public DateTime StartDate { get; set; }  // Ngày bắt đầu điều trị
        public DateTime EndDate { get; set; }    // Ngày kết thúc điều trị
        [Required]
        public int CustomerId { get; set; }  // ID khách hàng
       
    }
}
