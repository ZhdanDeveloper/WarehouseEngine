using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseEngineBusinessLayer.ViewModels
{
    public class SupplierViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Поле 'Назва' є обов'язковим.")]
        [StringLength(50, ErrorMessage = "Поле 'Назва' повинно містити не більше 50 символів.")]
        [DisplayName("Назва")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Поле 'Електронна адреса' є обов'язковим.")]
        [EmailAddress(ErrorMessage = "Поле 'Електронна адреса' має бути дійсною адресою електронної пошти.")]
        [StringLength(50, ErrorMessage = "Поле 'Електронна адреса' повинно містити не більше 50 символів.")]
        [DisplayName("Електронна адреса")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Поле 'Телефон' є обов'язковим.")]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Поле 'Телефон' повинно містити 10 цифр.")]
        [DisplayName("Телефон")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Поле 'Адреса' є обов'язковим.")]
        [StringLength(100, ErrorMessage = "Поле 'Адреса' повинно містити не більше 100 символів.")]
        [DisplayName("Адреса")]
        public string Address { get; set; }
    }
}
