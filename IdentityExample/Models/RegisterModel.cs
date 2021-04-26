using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IdentityExample.Models
{
    public class RegisterModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        [EmailAddress(ErrorMessage ="Giriş dizini hatalı. (@hotmail, @yahoo)")]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}