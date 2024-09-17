using DentalBooking.Contract.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DentalBooking.ModelViews.ServiceModelViews;

namespace DentalBooking_Services.Service
{
    public class ServiceServices
    {
        private readonly IUnitOfWork _unitOfWork;
       
        public ServiceServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public void haha()
        {
           
        }
    }
}
