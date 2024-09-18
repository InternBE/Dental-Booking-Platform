

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
        Task<User> GetClinicByIdAsync(int id);
        Task<User> CreateClinicAsync(User model);
        Task UpdateClinicAsync(int id, User model);
        Task DeleteClinicAsync(int id);
    }
}
