﻿using DentalBooking.Core.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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

        //References[
        //
        //
        //
        [ForeignKey("Clinic")]
        public int ClinicId { get; set; }
        public virtual ICollection<Clinic>? Clinic { get; set; }

        public virtual ICollection<Appointment> Appointments { get; set; }
        public virtual ICollection<TreatmentPlans>? TreatmentPlans { get; set; }
        
        public virtual ICollection<Message>? Messages { get; set; }
        
    }
}
