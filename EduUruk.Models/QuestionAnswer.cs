namespace EduUruk.Models
{
    public class QuestionAnswer
    {
        public Guid QuestionId { get; set; }
        public Guid AnswerId { get; set; }
        public int Marks { get; set; }
    }
}
