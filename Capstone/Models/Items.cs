using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Capstone.Models
{
    public class Items
    {
        [Key]
        public int ItemsId { get; set; }

        [Display(Name = "Item Name")]
        public string ItemName { get; set; }
        public decimal Price { get; set; }
        public string Status { get; set; }
        public string Category { get; set; }
        public string Condition { get; set; }
        public string Summary { get; set; }
        public string Lat { get; set; }
        public string Lng { get; set; }

  
        [ForeignKey("Sellers")]
        public int SellersId { get; set; }
        public virtual Sellers Sellers { get; set; }

        public virtual ICollection<File> Files { get; set; }
    }
}