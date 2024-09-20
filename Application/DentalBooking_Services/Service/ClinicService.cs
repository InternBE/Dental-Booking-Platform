using DentalBooking.Contract.Repository;
using DentalBooking.Contract.Repository.Entity;
using DentalBooking.ModelViews.ClinicModelViews;
using DentalBooking_Contract_Services.Interface;

namespace DentalBooking.Services
{
    public class ClinicService : IClinicService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ClinicService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ClinicModelView> GetClinicByIdAsync(int id)
        {
            var clinic = await _unitOfWork.ClinicRepository.GetByIdAsync(id);
            if (clinic == null) return null;

            return new ClinicModelView
            {
                Id = clinic.Id,
                ClinicName = clinic.ClinicName,
                Address = clinic.Address,
                PhoneNumber = clinic.PhoneNumber,
                OpeningTime = clinic.OpeningTime,
                ClosingTime = clinic.ClosingTime,
                SlotDurationMinutes = clinic.SlotDurationMinutes,
                MaxPatientsPerSlot = clinic.MaxPatientsPerSlot,
                MaxTreatmentPerSlot = clinic.MaxTreatmentPerSlot
            };
        }

        public async Task<ClinicModelView> CreateClinicAsync(ClinicModelView model)
        {
            var clinic = new Clinic
            {
                ClinicName = model.ClinicName,
                Address = model.Address,
                PhoneNumber = model.PhoneNumber,
                OpeningTime = model.OpeningTime,
                ClosingTime = model.ClosingTime,
                SlotDurationMinutes = model.SlotDurationMinutes,
                MaxPatientsPerSlot = model.MaxPatientsPerSlot,
                MaxTreatmentPerSlot = model.MaxTreatmentPerSlot
            };

            await _unitOfWork.ClinicRepository.AddAsync(clinic);
            await _unitOfWork.SaveAsync();

            model.Id = clinic.Id;
            return model;
        }

        public async Task UpdateClinicAsync(int id, ClinicModelView model)
        {
            var clinic = await _unitOfWork.ClinicRepository.GetByIdAsync(id);
            if (clinic == null) throw new Exception("Clinic not found");

            clinic.ClinicName = model.ClinicName;
            clinic.Address = model.Address;
            clinic.PhoneNumber = model.PhoneNumber;
            clinic.OpeningTime = model.OpeningTime;
            clinic.ClosingTime = model.ClosingTime;
            clinic.SlotDurationMinutes = model.SlotDurationMinutes;
            clinic.MaxPatientsPerSlot = model.MaxPatientsPerSlot;
            clinic.MaxTreatmentPerSlot = model.MaxTreatmentPerSlot;

            _unitOfWork.ClinicRepository.Update(clinic);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteClinicAsync(int id)
        {
            var clinic = await _unitOfWork.ClinicRepository.GetByIdAsync(id);
            if (clinic == null) throw new Exception("Clinic not found");

            _unitOfWork.ClinicRepository.Delete(clinic);
            await _unitOfWork.SaveAsync();
        }

        public async Task<bool> ApproveClinicAsync(int clinicId)
        {
            var clinic = await _unitOfWork.ClinicRepository.GetByIdAsync(clinicId);
            if (clinic == null) return false;

            clinic.IsApproved = true;
            await _unitOfWork.SaveAsync();
            return true;
        }
    }
}
