using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseEngineBusinessLayer.ViewModels
{
    public class ProductViewModel
    {
        public int? Id { get; set; }

        [DisplayName("Назва")]
        [Required(ErrorMessage = "Поле 'Назва' є обов'язковим.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Поле 'Назва' повинно містити від 3 до 50 символів.")]
        public string Name { get; set; }

        [DisplayName("Опис")]
        [Required(ErrorMessage = "Поле 'Опис' є обов'язковим.")]
        [StringLength(200, MinimumLength = 10, ErrorMessage = "Поле 'Опис' повинно містити від 10 до 200 символів.")]
        public string Description { get; set; }

        [DisplayName("Кількість")]
        [Required(ErrorMessage = "Поле 'Кількість' є обов'язковим.")]
        [Range(0, int.MaxValue, ErrorMessage = "Поле 'Кількість' повинно бути додатнім числом.")]
        public int Quantity { get; set; }

        [DisplayName("Ціна")]
        [Required(ErrorMessage = "Поле 'Ціна' є обов'язковим.")]
        [Range(0, double.MaxValue, ErrorMessage = "Поле 'Ціна' повинно бути додатнім числом.")]
        public decimal Price { get; set; }

        [DisplayName("Постачальник")]
        [Required(ErrorMessage = "Поле 'Постачальник' є обов'язковим.")]
        public int SupplierId { get; set; }
    }
}
