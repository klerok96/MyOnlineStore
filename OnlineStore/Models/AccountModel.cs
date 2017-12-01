using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineStore.Models
{
    public class RegisterModel
    {
        [Display(Name = "Логин")]
        [Required]
        public string Login { get; set; }

        [Display(Name = "Пароль")]
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Подтвердите Пароль")]
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string PasswordConfirm { get; set; }
    }

    public class LoginModel
    {
        [Display(Name = "Логин")]
        [Required]
        public string Login { get; set; }

        [Display(Name ="Пароль")]
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}