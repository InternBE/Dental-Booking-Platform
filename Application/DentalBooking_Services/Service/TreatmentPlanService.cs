using DentalBooking.ModelViews.TreatmentModels;
using DentalBooking.Contract.Repository.Entity;
using DentalBooking.Contract.Repository;
using DentalBooking_Contract_Services.Interface;
using Microsoft.EntityFrameworkCore;
using DentalBooking.ModelViews.TreatmentPlanModels;

public class TreatmentPlanService : ITreatmentService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly INotificationService _notificationService;

    public TreatmentPlanService(IUnitOfWork unitOfWork, INotificationService notificationService)
    {
        _unitOfWork = unitOfWork;
        _notificationService = notificationService;
    }

    public async Task<IEnumerable<TreatmentPlanResponseModelView>> GetAllAppointmentsAsync()
    {
        var appointments = await _unitOfWork.GetRepository<Appointment>().GetAllAsync();

        return appointments.Select(appointment => new TreatmentPlanResponseModelView
        {
            TreatmentPlanId = appointment.TreatmentPlanId,
            AppointmentDate = appointment.AppointmentDate,
            Status = appointment.Status,
            // Nếu bạn cần thêm thông tin khác, hãy bổ sung ở đây
        });
    }

    public async Task<TreatmentPlanResponseModelView> GetAppointmentDetailsAsync(int appointmentId)
    {
        var appointment = await _unitOfWork.GetRepository<Appointment>().GetByIdAsync(appointmentId);

        if (appointment == null)
        {
            throw new KeyNotFoundException("Appointment not found.");
        }

        // Chuyển đổi Appointment thành TreatmentPlanResponseModelView
        return new TreatmentPlanResponseModelView
        {
            TreatmentPlanId = appointment.TreatmentPlanId, // Thay đổi nếu cần
            AppointmentDate = appointment.AppointmentDate,
            Status = appointment.Status,
        };
    }

    public async Task<TreatmentPlanResponseModelView> GetTreatmentPlanAsync(int customerId)
    {
        var treatmentPlan = await _unitOfWork.GetRepository<TreatmentPlans>()
            .Entities
            .Where(tp => tp.CustomerId == customerId)
            .FirstOrDefaultAsync();

        if (treatmentPlan == null)
        {
            throw new KeyNotFoundException("Không tìm thấy kế hoạch điều trị cho khách hàng được chỉ định.");
        }

        return new TreatmentPlanResponseModelView
        {
            TreatmentPlanId = treatmentPlan.Id,
            Description = treatmentPlan.Description,
            AppointmentDate = treatmentPlan.Appointments.FirstOrDefault()?.AppointmentDate ?? DateTime.MinValue,
            Status = treatmentPlan.Appointments.FirstOrDefault()?.Status ?? "Pending", // Giá trị mặc định
        };
    }

    public async Task<bool> UpdateTreatmentPlanAsync(int treatmentId, TreatmentPlanRequestModelView treatmentRequestModel)
    {
        var treatmentPlan = await _unitOfWork.GetRepository<TreatmentPlans>().GetByIdAsync(treatmentId);

        if (treatmentPlan == null)
        {
            throw new KeyNotFoundException("Treatment plan not found.");
        }

        // Cập nhật thông tin kế hoạch điều trị
        treatmentPlan.Description = treatmentRequestModel.Description;
        // Cập nhật các thuộc tính khác nếu cần

        _unitOfWork.GetRepository<TreatmentPlans>().Update(treatmentPlan);
        await _unitOfWork.SaveAsync();

        return true;
    }

    public async Task<bool> CreateTreatmentPlanAsync(TreatmentPlanRequestModelView treatmentRequestModel)
    {
        var treatmentPlan = new TreatmentPlans
        {
            Description = treatmentRequestModel.Description,
            StartDate = treatmentRequestModel.StartDate,
            EndDate = treatmentRequestModel.EndDate,
            CustomerId = treatmentRequestModel.CustomerId,
            Appointments = treatmentRequestModel.Appointments.Select(a => new Appointment
            {
                AppointmentDate = a.AppointmentDate,
                UserId = a.UserId,
                ClinicId = a.ClinicId,
                Status = a.Status
            }).ToList()
        };

        await _unitOfWork.GetRepository<TreatmentPlans>().AddAsync(treatmentPlan);
        await _unitOfWork.SaveAsync();

        return true;
    }

    public async Task<bool> DeleteTreatmentPlanAsync(int treatmentId)
    {
        var treatmentPlan = await _unitOfWork.GetRepository<TreatmentPlans>().GetByIdAsync(treatmentId);

        if (treatmentPlan == null)
        {
            throw new KeyNotFoundException("Treatment plan not found.");
        }

        // Xóa mềm: Ghi lại thông tin người xóa và thời gian xóa
        treatmentPlan.DeletedTime = DateTimeOffset.Now;

        _unitOfWork.GetRepository<TreatmentPlans>().Update(treatmentPlan);
        await _unitOfWork.SaveAsync();

        return true;
    }

    public async Task SendTreatmentPlanToCustomerAsync(int customerId, int doctorId)
    {
        var treatmentPlan = await GetTreatmentPlanAsync(customerId); // Lấy kế hoạch điều trị

        if (treatmentPlan == null)
        {
            throw new KeyNotFoundException("Kế hoạch điều trị không tìm thấy.");
        }

        // Tạo thông điệp thông báo
        string message = $"Kế hoạch điều trị của bạn: {treatmentPlan.Description}. Ngày hẹn: {treatmentPlan.AppointmentDate}. Trạng thái: {treatmentPlan.Status}.";

        // Gửi thông báo cho khách hàng
        await _notificationService.SendNotificationAsync(customerId, doctorId, message);
    }

}
