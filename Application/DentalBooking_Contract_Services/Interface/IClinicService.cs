
using DentalBooking.ModelViews.ClinicModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalBooking_Contract_Services.Interface
{
    public interface IClinicService
    {
        Task<ClinicModelView> GetClinicByIdAsync(int id);
        Task<ClinicModelView> CreateClinicAsync(ClinicModelView model);
        Task UpdateClinicAsync(int id, ClinicModelView model);
        Task DeleteClinicAsync(int id);
    }
}
