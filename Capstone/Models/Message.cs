using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Capstone.Models
{
    public class Message 
    {
        [Key]
        public int MessageId { get; set; }

        [Display(Name = "Your Message:")]
        public string MessageBox { get; set; }

        [ForeignKey("Sellers")]
        public int SellersId { get; set; }
        public virtual Sellers Sellers { get; set; }
    }
}
