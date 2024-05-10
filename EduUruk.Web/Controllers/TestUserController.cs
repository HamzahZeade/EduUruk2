using EduUruk.DAL.EnitityDAL;
using EduUruk.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduUruk.Web.Controllers
{
    public class TestUserController : Controller
    {

        private readonly ApplicationDbContext _context;

        public TestUserController(ApplicationDbContext context)
        {
            _context = context;
        }
        // GET: TestUser/Index
        public IActionResult Index()
        {
            var tests = _context.Tests.Where(x => x.IsActive == true).ToList();
            return View(tests);
        }


        public IActionResult QustionsTest(Guid Id)
        {
            var Questions = _context.Questions.Where(x => x.TestId == Id).Include(t => t.Answers).ToList();
            return View(Questions);
        }
        //public IActionResult Index()
        //{
        //    var test = _context.Tests
        //        .Include(t => t.Questions) // Include questions related to the test
        //        .ThenInclude(q => q.Answers) // Include answers related to each question
        //        .ToList();

        //    if (test == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(test);
        //}

        // GET: TestUser/AnswerTest/5
        //public IActionResult AnswerTest(Guid id)
        //{
        //    var test = _context.Tests.Find(id);
        //    if (test == null)
        //    {
        //        return NotFound();
        //    }

        //    var viewModel = new AnswerTestViewModel
        //    {
        //        TestId = test.Id,
        //        Title = test.Title,
        //        Questions = _context.Questions.Where(q => q.TestId == id).ToList()
        //    };

        //    return View(viewModel);
        //}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AnswerTest(Test viewModel)
        {
            if (ModelState.IsValid)
            {
                // Calculate the score
                int totalScore = 0;
                foreach (var question in viewModel.Questions)
                {
                    var correctAnswer = question.Answers.FirstOrDefault(a => a.IsCorrect);
                    //if (correctAnswer != null && correctAnswer.AnswerId == question.UserAnswerId)
                    //{
                    //    totalScore += 10; // Assuming each correct answer gives 10 marks
                    //}
                }

                // Save the user's answers and score in your database or session
                // For example:
                // userService.SaveUserAnswers(viewModel.UserId, viewModel.Questions);
                // userService.SaveUserScore(viewModel.UserId, totalScore);

                // Pass the score to the view for display
                ViewData["TotalScore"] = totalScore;

                return View("TestResult", viewModel);
            }

            // If model state is not valid, return to the test view with the same model
            return View("Index", viewModel);
        }


        // POST: TestUser/SubmitAnswers
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult SubmitAnswers(AnswerTestViewModel viewModel)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        // Calculate the score based on user answers
        //        int totalMarks = viewModel.Questions.Count * 10; // Assuming each question is worth 10 marks
        //        int score = 0;

        //        foreach (var question in viewModel.Questions)
        //        {
        //            var selectedAnswer = viewModel.UserAnswers.FirstOrDefault(a => a.QuestionId == question.QuestionId);
        //            if (selectedAnswer != null && selectedAnswer.IsCorrect)
        //            {
        //                score += 10; // Increment score by 10 for each correct answer
        //            }
        //        }

        //        // You can save the user's score and answers to the database here if needed

        //        // Pass the score and total marks to the result view
        //        var resultViewModel = new TestResultViewModel
        //        {
        //            TestTitle = viewModel.TestTitle,
        //            Score = score,
        //            TotalMarks = totalMarks
        //        };

        //        return View("TestResult", resultViewModel);
        //    }

        //    // If model state is not valid, return to the answer test view with the same model
        //    return View("AnswerTest", viewModel);
        //}
    }
}
