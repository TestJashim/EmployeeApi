using System.ComponentModel.DataAnnotations;

namespace EmployeeApi.Models.ViewModels
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Range(18, 65)]
        public int Age { get; set; }

        [Required]
        [MaxLength(10)]
        public string Gender { get; set; }

        [Required]
        [MaxLength(50)]
        public string Department { get; set; }

        public bool IsFullTime { get; set; }

        [DataType(DataType.Date)]
        public DateTime HireDate { get; set; }

        [Required]
        public IFormFile Image { get; set; }
    }
}
