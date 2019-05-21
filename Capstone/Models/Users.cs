using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Capstone.Models
{
    public abstract class Users
    {
        [Key]
        public int id { get; set; }

        public string firstname { get; set; }
        public string lastname { get; set; }
        public string address  { get; set; }
        public string city { get; set; }
        public string state { get; set; }



    }
}