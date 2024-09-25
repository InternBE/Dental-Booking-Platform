
using DentalBooking.ModelViews.ClinicModelViews;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DentalBooking.Contract.Repository.Entity;

namespace DentalBooking_Contract_Services.Interface
{
    public interface IClinicService
    {
        Task<ClinicResponseModelView> GetClinicByIdAsync(int id);
        Task<ClinicResponseModelView> CreateClinicAsync(ClinicRequestModelView model);
        Task UpdateClinicAsync(int id, ClinicRequestModelView model);
        Task DeleteClinicAsync(int id);

        Task<bool> ApproveClinicAsync(int clinicId);
        Task<IEnumerable<ClinicResponseModelView>> GetClinicsAsync(int pageNumber, int pageSize);
    }
}
