using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WarehouseEngine.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Введіть логін")]
        [DisplayName("Логін")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Введіть пароль")]
        [DisplayName("Пароль")]
        public string Password { get; set; }
    }
}
