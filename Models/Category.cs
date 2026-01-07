using System.ComponentModel.DataAnnotations;

namespace Quiz_App.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Kategoriya nomi kiritilishi shart")]
        [StringLength(100, ErrorMessage = "Kategoriya nomi 100 ta belgidan oshmasligi kerak")]
        [Display(Name = "Kategoriya nomi")]
        public string Name { get; set; }

        [Display(Name = "Ta'rif")]
        [StringLength(500)]
        public string Description { get; set; }

        [Display(Name = "Yaratilgan sana")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Display(Name = "Faol")]
        public bool IsActive { get; set; } = true;

        public ICollection<Quiz> Quizzes { get; set; } = new List<Quiz>();
    }
}
