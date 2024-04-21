namespace EduUruk.Models.ViewModels
{
    public class AnswerTestViewModel
    {
        public Guid TestId { get; set; }
        public string TestName { get; set; }
        public List<QuestionViewModel> Questions { get; set; }
    }

    public class UserAnswerViewModel
    {
        public Guid QuestionId { get; set; }
        public Guid AnswerId { get; set; }
        public bool IsCorrect { get; set; }
    }
}
