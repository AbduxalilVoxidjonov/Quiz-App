using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quiz_App.Models
{
    public class UserProgress
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string UserName { get; set; }

        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        public int AttemptNumber { get; set; } // Nechanchi urinish
        public int Score { get; set; } // Natija (foizda)
        public int CorrectAnswers { get; set; }
        public int TotalQuestions { get; set; }
        public DateTime AttemptDate { get; set; } = DateTime.Now;
    }
}