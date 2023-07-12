using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeApi.Models
{
    public class Employee
    {
        [Key]
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

        public string Image { get; set; }

        [NotMapped]
        //[Required]
        [MaxLength(100)]
        public IFormFile ImageFile { get; set; }
    }
}
