using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineStore.Models
{
    public class ManageModel
    {
        [HiddenInput(DisplayValue = false)]
        public int UserId { get; set; }

        [Display(Name = "Логин")]
        [Required]
        public string Login { get; set; }

        [Display(Name = "Пароль")]
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Полное имя")]
        public string FullName { get; set; }

        [Display(Name = "Деньги")]
        [Required]
        [RegularExpression(@"^[0-9]*[.][0-9]+$", ErrorMessage = "Некорректный ввод суммы")]
        public decimal Money { get; set; }
    }
}