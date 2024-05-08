using System.ComponentModel.DataAnnotations.Schema;

namespace EduUruk.Models.Entities
{
    [Table("Question")]
    public class Question : _GlobalColumn
    {
        public Guid Id { get; set; }
        public Guid TestId { get; set; }
        [ForeignKey("TestId")]
        public virtual Test? Test { get; set; }
        public List<Answer> Answers { get; set; }
        public string QuestionText { get; set; }
        public int CorrectAnswerIndex { get; set; }
        public int QuestionMark { get; set; }
    }
}
