using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DentalBooking.ModelViews.UserModelViews;

namespace DentalBooking_Contract_Services.Interface
{
    public interface IUserService
    {
        Task<IList<UserResponseModel>> GetAll();
        Task<UserResponseModel> Create(UserRequestModel userRequest);


    }
}
