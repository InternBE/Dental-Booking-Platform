
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
        Task<ClinicModelView> GetClinicByIdAsync(int id);
        Task<ClinicModelView> CreateClinicAsync(ClinicModelView model);
        //Task<User> CreateClinicAsync(User model);
        Task UpdateClinicAsync(int id, ClinicModelView model);
        Task DeleteClinicAsync(int id);

        Task<bool> ApproveClinicAsync(int clinicId);
        Task<IEnumerable<ClinicModelView>> GetClinicsAsync(int pageNumber, int pageSize);
    }
}
