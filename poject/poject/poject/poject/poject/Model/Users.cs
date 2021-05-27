using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace poject.Model
{
    public class Users
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [DataType("Password")]
        public string Password { get; set; }

        [DataType("Password")]
        [System.Web.Mvc.Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string Name { get; set; }

        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
    }
}