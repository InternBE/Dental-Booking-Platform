using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalBooking.Contract.Repository.Entity
{
    public class Message
    {
        public string Content { get; set; } = string.Empty;
        public DateTime SendDate { get; set; }

        [ForeignKey("User")]
        public int SenderId { get; set; }
        public virtual User? User { get; set; }

        public int ReceiverId { get; set; }
    }
}
