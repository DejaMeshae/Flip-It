using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Capstone.Models
{
    public class Sellers 
    {
        [Key]
        public int SellersId { get; set; }

        [Display(Name = "First Name:")]
        public string Firstname { get; set; }

        [Display(Name = "Last Name:")]
        public string Lastname { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }

        [Display(Name = "Zip Code:")]
        public string ZipCode { get; set; }

        [Display(Name = "Profile Picture:")]
        public byte[] UserPhoto { get; set; }

        public string Lat { get; set; }

        public string Lng { get; set; }

        public IEnumerable<Items> Items { get; set; }

        [ForeignKey("ApplicationUser")]
        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }


    }
}