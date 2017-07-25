using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BookingSystem.Models
{
    public class User
    {
        [Display(Name="Date of birth")]
        [DisplayFormat(ApplyFormatInEditMode =true, DataFormatString ="{0:dd/MM/yyyy}")]
        public Nullable<System.DateTime> DOB { get; set; }
    }
}