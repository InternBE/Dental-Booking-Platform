using DentalBooking.ModelViews.AppointmentModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalBooking.ModelViews.TreatmentModels
{
    public class TreatmentPlanResponseModelView
    {
        public string Description { get; set; } = string.Empty;  // Mô tả kết quả khám
        public DateTime AppointmentDate { get; set; }  // Ngày hẹn
        public string Status { get; set; } = "Pending";  // Trạng thái cuộc hẹn
        public string DoctorNotes { get; set; } = string.Empty;  // Ghi chú từ bác sĩ
                                                                
        public DateTime StartDate { get; set; }  // Ngày bắt đầu điều trị
        public DateTime EndDate { get; set; }    // Ngày kết thúc điều trị
    }
}
