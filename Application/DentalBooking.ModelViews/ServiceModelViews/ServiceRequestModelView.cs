using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalBooking.ModelViews.ServiceModelViews
{
    public class ServiceRequestModelView
    {
        [Required]
        public string ServiceName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        [Required]
        public decimal Price { get; set; }
    }
}
