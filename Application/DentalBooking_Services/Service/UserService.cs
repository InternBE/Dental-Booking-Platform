using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DentalBooking.Contract.Repository.Interface;
using DentalBooking.ModelViews.UserModelViews;
using DentalBooking_Contract_Services.Interface;


namespace DentalBooking_Services.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public Task<IList<UserResponseModel>> GetAll()
        {
            IList<UserResponseModel> users = new List<UserResponseModel>
            {
                new UserResponseModel { Id = "1" },
                new UserResponseModel { Id = "2" },
                new UserResponseModel { Id = "3" }
            };

            return Task.FromResult(users);
        }


    }
}
