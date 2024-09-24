using DentalBooking.ModelViews.AppointmentModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalBooking.ModelViews.TreatmentPlanModels
{
    public class TreatmentPlanRequestModelView
    {
        public int TreatmentPlanId { get; set; }  // ID kế hoạch điều trị
        public string Description { get; set; } = string.Empty;  // Mô tả điều trị
        public DateOnly StartDate { get; set; }  // Ngày bắt đầu điều trị
        public DateOnly EndDate { get; set; }    // Ngày kết thúc điều trị
        public int CustomerId { get; set; }  // ID khách hàng
        public List<AppointmentRequestModelView> Appointments { get; set; } = new();  // Danh sách cuộc hẹn
    }
}
