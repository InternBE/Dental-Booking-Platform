﻿using DentalBooking.Core.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalBooking.Contract.Repository.Entity
{
    public class TreatmentPlans : BaseEntity
    {
        public string Description { get; set; } = string.Empty;
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public int DentistId { get; set; }

        //References
        [ForeignKey("User")]
        public int CustomerId { get; set; }
        public virtual User? User { get; set; }

        public virtual ICollection<Appointment>? Appointments { get; set; }
    }
}
