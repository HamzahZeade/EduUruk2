namespace EduUruk.Models.ViewModels
{
    public class QuestionViewModel
    {
        public Guid TestId { get; set; }
        public string TestName { get; set; }
        public int QuestionMark { get; set; }
        public List<QuestionInputModel> Questions { get; set; }

        // You can add more properties as needed for question creation
    }

    public class QuestionInputModel
    {
        public string QuestionText { get; set; }
        public List<AnswerInputModel> Answers { get; set; }
        public int CorrectAnswerIndex { get; set; }
    }
    public class AnswerInputModel
    {
        public string AnswerText { get; set; }
        public bool IsCorrect { get; set; }
    }
}
