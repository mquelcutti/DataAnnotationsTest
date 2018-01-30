using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Localization.SqlLocalizer.DbStringLocalizer;

namespace DataATest.ViewModels
{
    public class ContactViewModel
    {
        [Required]
        [MinLength(5)]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Subject { get; set; }


        //issue with Rnage https://github.com/jquery-validation/jquery-validation/issues/626
        //ToDo resolve range and Display errors
        [Required(ErrorMessage = ResourceKeys.MessageTextRequiredError)]
        [Display(Name = ResourceKeys.MessageLabel)]
       
        [Range(1, 250, ErrorMessage = ResourceKeys.MessageTextRangeError)]
        public string Message { get; set; }
    }
}
