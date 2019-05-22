using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Capstone.Models
{
    public class Items
    {
        [Key]
        public int ItemsId { get; set; }

        [Display(Name = "Item Name:")]
        public string ItemName { get; set; }
        public int Price { get; set; }
        public string Category { get; set; }

        [Display(Name = "Photo of Item:")]
        public byte[] UserPhoto { get; set; }
        public string Condition { get; set; }
        public string Summary { get; set; }

    }
}