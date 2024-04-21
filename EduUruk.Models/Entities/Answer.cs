using System.ComponentModel.DataAnnotations.Schema;

namespace EduUruk.Models.Entities
{
    public class Answer
    {
        public int AnswerId { get; set; }
        public string AnswerText { get; set; }

        public Guid QuestionId { get; set; }
        [ForeignKey("QuestionId")]
        public virtual Question? Question { get; set; }
        public bool IsCorrect { get; set; }
    }
}
