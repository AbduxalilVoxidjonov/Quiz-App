using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quiz_App.Models
{
    public class Quiz
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Savol kiritilishi shart")]
        [StringLength(500)]
        public string Question { get; set; }

        [Required(ErrorMessage = "1-javob kiritilishi shart")]
        [StringLength(200)]
        public string Option1 { get; set; }

        [Required(ErrorMessage = "2-javob kiritilishi shart")]
        [StringLength(200)]
        public string Option2 { get; set; }

        [Required(ErrorMessage = "3-javob kiritilishi shart")]
        [StringLength(200)]
        public string Option3 { get; set; }

        [Required(ErrorMessage = "4-javob kiritilishi shart")]
        [StringLength(200)]
        public string Option4 { get; set; }

        [Required(ErrorMessage = "To'g'ri javobni tanlang")]
        [Range(1, 4, ErrorMessage = "To'g'ri javob 1 dan 4 gacha bo'lishi kerak")]
        public int CorrectAnswer { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Kategoriyani tanlang")]
        [ForeignKey("Category")]
        public int CategoryId { get; set; }

        // Navigation property - validatsiyadan chiqarish
        public Category? Category { get; set; }  // ? belgisi qo'shildi
    }
}