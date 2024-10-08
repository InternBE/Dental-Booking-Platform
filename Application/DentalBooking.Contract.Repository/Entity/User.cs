﻿using DentalBooking.Core.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalBooking.Contract.Repository.Entity
{
    public class User : BaseEntity
    {
        
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber {  get; set; } = string.Empty;

        [ForeignKey("Clinic")]
        public int ClinicId { get; set; }
        public virtual Clinic? Clinic { get; set; }
        public bool IsApproved { get; set; } = false;
        public virtual ICollection<Appointment>? Appointments { get; set; }
        public virtual ICollection<TreatmentPlans>? TreatmentPlans { get; set; }
        
        public virtual ICollection<Message>? Messages { get; set; }
       
        
    }
}
