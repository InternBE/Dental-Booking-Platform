using DentalBooking.ModelViews.TreatmentModels;
using DentalBooking.Contract.Repository.Entity;
using DentalBooking.Contract.Repository;
using DentalBooking_Contract_Services.Interface;
using Microsoft.EntityFrameworkCore;
using DentalBooking.ModelViews.TreatmentPlanModels;
using DentalBooking.ModelViews.MailModelViews;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;
using DentalBooking.Repository.Context;
using DentalBooking_Services.Service;

public class TreatmentPlanService : ITreatmentPlanService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly INotificationService _notificationService;
    private readonly MailSettings _mailSettings;
    private readonly DatabaseContext _context;
    private readonly ISendMailService _sendMailService;

    public TreatmentPlanService(IUnitOfWork unitOfWork, INotificationService notificationService, IOptions<MailSettings> mailSettings, DatabaseContext context, ISendMailService sendMailService)
    {
        _unitOfWork = unitOfWork;
        _notificationService = notificationService;
        _mailSettings = mailSettings.Value;
        _context = context;
        _sendMailService = sendMailService;
    }

    public async Task<IEnumerable<TreatmentPlanResponseModelView>> GetAllTreatmentPlansAsync()
    {
        var treatmentPlans = await _unitOfWork.GetRepository<TreatmentPlans>().GetAllAsync();

        // Chuyển đổi danh sách TreatmentPlans thành danh sách TreatmentPlanResponseModelView
        return treatmentPlans.Select(treatmentPlan => new TreatmentPlanResponseModelView
        {
            Description = treatmentPlan.Description,
            AppointmentDate = treatmentPlan.Appointments.FirstOrDefault()?.AppointmentDate ?? DateTime.MinValue,
            Status = treatmentPlan.Appointments.FirstOrDefault()?.Status ?? "Pending", // Giá trị mặc định
        });
    }

    // Phương thức mới: Lấy danh sách kế hoạch điều trị với phân trang
    public async Task<IEnumerable<TreatmentPlanResponseModelView>> GetPaginatedTreatmentPlansAsync(int pageNumber, int pageSize)
    {
        var treatmentPlans = await _unitOfWork.GetRepository<TreatmentPlans>()
            .Entities
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return treatmentPlans.Select(treatmentPlan => new TreatmentPlanResponseModelView
        {
            Description = treatmentPlan.Description,
            AppointmentDate = treatmentPlan.Appointments.FirstOrDefault()?.AppointmentDate ?? DateTime.MinValue,
            Status = treatmentPlan.Appointments.FirstOrDefault()?.Status ?? "Pending", // Giá trị mặc định
        });
    }

    // Phương thức mới: Lấy tổng số lượng kế hoạch điều trị
    public async Task<int> GetTotalTreatmentPlansCountAsync()
    {
        var totalPlans = await _unitOfWork.GetRepository<TreatmentPlans>()
            .Entities
            .CountAsync();
        return totalPlans;
    }

    public async Task<TreatmentPlanResponseModelView> GetTreatmentPlanDetailsAsync(int treatmentPlanId)
    {
        var treatmentPlan = await _unitOfWork.GetRepository<TreatmentPlans>().GetByIdAsync(treatmentPlanId);

        if (treatmentPlan == null)
        {
            throw new KeyNotFoundException("Kế hoạch điều trị không tìm thấy.");
        }

        return new TreatmentPlanResponseModelView
        {
            Description = treatmentPlan.Description,
            AppointmentDate = treatmentPlan.Appointments.FirstOrDefault()?.AppointmentDate ?? DateTime.MinValue,
            Status = treatmentPlan.Appointments.FirstOrDefault()?.Status ?? "Pending", // Giá trị mặc định
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
            throw new KeyNotFoundException("Không tìm thấy kế hoạch điều trị.");
        }

        // Kiểm tra ngày hợp lệ
        if (treatmentRequestModel.StartDate > treatmentRequestModel.EndDate)
        {
            throw new ArgumentException("Ngày bắt đầu không được lớn hơn ngày kết thúc.");
        }

        // Cập nhật thông tin kế hoạch điều trị
        treatmentPlan.Description = treatmentRequestModel.Description;
        treatmentPlan.StartDate = treatmentRequestModel.StartDate;
        treatmentPlan.EndDate = treatmentRequestModel.EndDate;
        treatmentPlan.CustomerId = treatmentRequestModel.CustomerId;

        try
        {
            _unitOfWork.GetRepository<TreatmentPlans>().Update(treatmentPlan);
            await _unitOfWork.SaveAsync();
            return true;
        }
        catch (DbUpdateException ex)
        {
            Console.WriteLine($"Error updating treatment plan: {ex.Message}");
            throw new Exception("Không thể cập nhật kế hoạch điều trị do lỗi cơ sở dữ liệu.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating treatment plan: {ex.Message}");
            throw new Exception("Không thể cập nhật kế hoạch điều trị. Vui lòng thử lại sau.");
        }
    }

    public async Task<bool> CreateTreatmentPlanAsync(TreatmentPlanRequestModelView treatmentRequestModel)
    {
        var customer = await _unitOfWork.GetRepository<User>().GetByIdAsync(treatmentRequestModel.CustomerId);
        if (customer == null)
        {
            throw new KeyNotFoundException("Không tìm thấy khách hàng với ID đã cung cấp.");
        }

        var treatmentPlan = new TreatmentPlans
        {
            Description = treatmentRequestModel.Description,
            StartDate = treatmentRequestModel.StartDate,
            EndDate = treatmentRequestModel.EndDate,
            CustomerId = treatmentRequestModel.CustomerId,
            CreatedTime = DateTime.UtcNow,
            LastUpdatedTime = DateTime.UtcNow
        };

        try
        {
            await _unitOfWork.GetRepository<TreatmentPlans>().AddAsync(treatmentPlan);
            await _unitOfWork.SaveAsync();
            return true;
        }
        catch (DbUpdateException ex)
        {
            Console.WriteLine($"Lỗi khi tạo kế hoạch điều trị: {ex.Message}");
            throw new Exception("Không thể tạo kế hoạch điều trị do lỗi cơ sở dữ liệu.", ex);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Lỗi khi tạo kế hoạch điều trị: {ex.Message}");
            throw new Exception("Không thể tạo kế hoạch điều trị. Vui lòng thử lại sau.", ex);
        }
    }

    public async Task<bool> DeleteTreatmentPlanAsync(int treatmentId)
    {
        var treatmentPlan = await _unitOfWork.GetRepository<TreatmentPlans>().GetByIdAsync(treatmentId);

        if (treatmentPlan == null)
        {
            throw new KeyNotFoundException("Không tìm thấy kế hoạch điều trị.");
        }

        treatmentPlan.DeletedTime = DateTimeOffset.Now;

        try
        {
            _unitOfWork.GetRepository<TreatmentPlans>().Update(treatmentPlan);
            await _unitOfWork.SaveAsync();
            return true;
        }
        catch (DbUpdateException ex)
        {
            Console.WriteLine($"Error deleting treatment plan: {ex.Message}");
            throw new Exception("Không thể xóa kế hoạch điều trị do lỗi cơ sở dữ liệu.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting treatment plan: {ex.Message}");
            throw new Exception("Không thể xóa kế hoạch điều trị. Vui lòng thử lại sau.");
        }
    }

    public async Task SendTreatmentPlanToCustomerAsync(int customerId, int doctorId)
    {
        var treatmentPlan = await GetTreatmentPlanAsync(customerId);

        if (treatmentPlan == null)
        {
            throw new KeyNotFoundException("Kế hoạch điều trị không tìm thấy.");
        }

        string message = $"Kế hoạch điều trị của bạn: {treatmentPlan.Description}. Ngày hẹn: {treatmentPlan.AppointmentDate}. Trạng thái: {treatmentPlan.Status}.";

        await _notificationService.SendNotificationAsync(customerId, doctorId, message);
    }

    public async Task<IEnumerable<TreatmentPlanResponseModelView>> GetAllTreatmentPlansForCustomerAsync(int customerId)
    {
        var treatmentPlans = await _unitOfWork.GetRepository<TreatmentPlans>()
            .Entities
            .Where(tp => tp.CustomerId == customerId)
            .ToListAsync();

        return treatmentPlans.Select(treatmentPlan => new TreatmentPlanResponseModelView
        {
            Description = treatmentPlan.Description,
            StartDate = treatmentPlan.StartDate,
            EndDate = treatmentPlan.EndDate,
        });
    }
    public async Task<TreatmentPlans?> GetTreatmentPlanByIdAsync(int id)
    {
        return await _context.TreatmentPlans.Include(tp => tp.User).FirstOrDefaultAsync(tp => tp.Id == id);
    }

    public async Task SendTreatmentPlanEmailAsync(int treatmentPlanId)
    {
        // Lấy thông tin kế hoạch điều trị theo ID
        var treatmentPlan = await GetTreatmentPlanByIdAsync(treatmentPlanId);

        if (treatmentPlan == null || treatmentPlan.User == null)
        {
            throw new Exception("Không tìm thấy kế hoạch điều trị hoặc khách hàng.");
        }

        // Kiểm tra địa chỉ email
        if (string.IsNullOrWhiteSpace(treatmentPlan.User.Email))
        {
            throw new Exception("Địa chỉ email của khách hàng không hợp lệ.");
        }

        // Chuẩn bị nội dung email
        var mailContent = new MailContent
        {
            To = treatmentPlan.User.Email,  // Gửi email tới khách hàng
            Subject = "Kế hoạch điều trị của bạn",
            Body = $"<p>Kính gửi {treatmentPlan.User.FullName},</p>" +
                   $"<p>Dưới đây là thông tin kế hoạch điều trị của bạn:</p>" +
                   $"<table style='border-collapse: collapse; width: 100%;'>" +
                   $"<tr><th style='border: 1px solid #ddd; padding: 8px;'>Mô tả</th><td style='border: 1px solid #ddd; padding: 8px;'>{treatmentPlan.Description}</td></tr>" +
                   $"<tr><th style='border: 1px solid #ddd; padding: 8px;'>Ngày bắt đầu</th><td style='border: 1px solid #ddd; padding: 8px;'>{treatmentPlan.StartDate:dd/MM/yyyy}</td></tr>" +
                   $"<tr><th style='border: 1px solid #ddd; padding: 8px;'>Ngày kết thúc</th><td style='border: 1px solid #ddd; padding: 8px;'>{treatmentPlan.EndDate:dd/MM/yyyy}</td></tr>" +
                   $"</table>" +
                   "<p>Trân trọng,<br>Đội ngũ nha khoa</p>"
        };

        // Gửi email
        await _sendMailService.SendMail(mailContent); // Gọi phương thức SendMail
    }


}
