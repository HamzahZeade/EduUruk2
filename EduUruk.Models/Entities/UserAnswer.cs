namespace EduUruk.Models.Entities
{
    public class UserAnswer
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid TestId { get; set; }
        public List<QuestionAnswer> QuestionAnswers { get; set; }
        public int TotalScore { get; set; }
    }
}
