# Hệ thống Quản lý Phòng khám Nha khoa

## Vai trò và Chức năng

### Guest
- Xem thông tin phòng khám, lịch khám, danh mục dịch vụ.
- Đăng ký và xác thực thông tin cá nhân để trở thành khách hàng (customer).

### Customer
- Đăng ký lịch khám một lần. Nhận thông báo nhắc lịch 1 ngày trước ngày khám.
- Đăng ký lịch khám và điều trị định kỳ (theo lịch điều trị của nha sĩ). 
  - Ví dụ: Đăng ký lịch khám & điều trị niềng răng (mỗi tháng 1 lần trong suốt 12 tháng).
- Nhận kết quả khám và thông báo về lịch điều trị từ nha sĩ.
- Nhắn tin và trao đổi trực tiếp với nha sĩ của phòng khám.

### Dentist
- Xem lịch khám trong tuần của mình.
- Đề xuất lịch khám định kỳ cho bệnh nhân.
- Gửi kết quả khám lên hệ thống và xem hồ sơ khám chữa bệnh của bệnh nhân mà mình đang điều trị.
- Trao đổi với bệnh nhân thông qua kênh chat của hệ thống.

### Clinic Owner
- Đăng ký thông tin phòng khám vào hệ thống, bao gồm các thông tin xác thực.
- Đăng ký thông tin bác sĩ của phòng khám mình và tạo tài khoản cho bác sĩ.
- Nhập lịch hoạt động của phòng khám.
  - Ví dụ: Giờ mở cửa, giờ đóng cửa, time slot (khung giờ) cho mỗi lần khám/điều trị.
    - Cụ thể: 1 slot là 45 phút; tối đa 3 bệnh nhân khám cho 1 slot; 1 bệnh nhân điều trị cho 1 slot.
- Quản lý thông tin bệnh nhân, lịch khám, bác sĩ. Quản lý thông tin trao đổi giữa bác sĩ và bệnh nhân.

### System Admin
- Xét duyệt thông tin phòng khám và thông tin bác sĩ.
- Quản lý tài khoản người dùng.
