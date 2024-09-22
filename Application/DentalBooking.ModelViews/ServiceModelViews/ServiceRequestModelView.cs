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
        public string? ServiceName { get; set; }
        public string? Description { get; set; } 
        [Required]
        public decimal Price { get; set; }
        public string? Status { get; set; } // Giá trị mặc định là "Pending"
    }
}
