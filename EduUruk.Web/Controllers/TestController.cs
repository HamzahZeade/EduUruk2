

using EduUruk.DAL.EnitityDAL;
using EduUruk.Models.Entities;
using EduUruk.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EduUruk.Web.Controllers
{
    public class TestController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public TestController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        // GET: Test
        public async Task<IActionResult> Index()
        {
            var tests = await _context.Tests.ToListAsync();
            return View(tests);
        }

        // GET: Test/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Test/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TestModal test)
        {
            if (ModelState.IsValid)
            {
                var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var model = new Test
                {
                    CreatedBy = userId,
                    ChangedBy = userId,
                    ChangedOn = DateTime.Now,
                    CreatedOn = DateTime.Now,
                    Mark = test.Mark,
                    Title = test.Title,
                    IsActive = test.IsActive == null ? false : test.IsActive
                };

                _context.Add(model);
                await _context.SaveChangesAsync();

                // Redirect to the CreateQuestion action with the newly created test's ID
                return RedirectToAction("CreateQuestion", "Test", new { testId = model.Id });
            }

            return View(test);
        }

        // GET: Test/CreateQuestion
        public IActionResult CreateQuestion(Guid testId)
        {
            var test = _context.Tests.Find(testId);

            if (test == null)
            {
                return NotFound();
            }

            var model = new QuestionViewModel
            {
                TestId = testId,
                TestName = test.Title // Assuming Test has a Title property

            };

            return View(model);
        }
        // GET: Test/Edit/5
        public async Task<IActionResult> Edit(Guid id)
        {
            var test = await _context.Tests
                .Include(t => t.Questions) // Include questions related to the test
                    .ThenInclude(q => q.Answers) // Include answers related to each question
                .FirstOrDefaultAsync(t => t.Id == id);

            if (test == null)
            {
                return NotFound();
            }

            return View(test);
        }


        // POST: Test/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Test test)
        {

            try
            {
                // Update test entity
                var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                test.CreatedBy = userId;
                test.ChangedBy = userId;
                test.ChangedOn = DateTime.Now;
                test.CreatedOn = DateTime.Now;
                test.Mark = test.Mark;
                test.IsActive = test.IsActive == null ? false : test.IsActive;

                _context.Tests.Update(test);

                // Update questions
                foreach (var question in test.Questions)
                {
                    var existingQuestion = await _context.Questions.FirstOrDefaultAsync(q => q.Id == question.Id);
                    if (existingQuestion != null)
                    {
                        existingQuestion.CreatedBy = "admin";
                        existingQuestion.ChangedBy = "admin";
                        existingQuestion.ChangedOn = DateTime.Now;
                        existingQuestion.CreatedOn = DateTime.Now;
                        existingQuestion.QuestionText = question.QuestionText;
                        existingQuestion.QuestionMark = question.QuestionMark;
                        // Update answer entity in the context
                        _context.Questions.Update(existingQuestion);
                    }
                    // Update answers
                    foreach (var answer in question.Answers)
                    {
                        // Retrieve the existing answer from the database
                        var existingAnswer = await _context.Answers.FindAsync(answer.AnswerId);
                        if (existingAnswer != null)
                        {
                            // Update answer properties
                            existingAnswer.IsCorrect = answer.IsCorrect;
                            // Update answer entity in the context
                            _context.Answers.Update(existingAnswer);
                        }
                    }
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TestExists(test.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateQuestion(QuestionViewModel model)
        {
            if (ModelState.IsValid)
            {
                var savedQuestionIds = new List<Guid>(); // To store the IDs of saved questions

                // Step 1: Save Questions
                foreach (var questionInput in model.Questions)
                {
                    var question = new Question
                    {
                        QuestionText = questionInput.QuestionText,
                        TestId = model.TestId,
                        CreatedBy = "admin",
                        ChangedBy = "admin",
                        ChangedOn = DateTime.Now,
                        CreatedOn = DateTime.Now
                        // You can add more properties as needed
                    };

                    _context.Questions.Add(question);
                    await _context.SaveChangesAsync(); // Save each question individually

                    // Store the ID of the saved question
                    savedQuestionIds.Add(question.Id);
                }

                // Step 2: Save Answers Linked to Questions
                for (int i = 0; i < model.Questions.Count; i++)
                {
                    var questionId = savedQuestionIds[i]; // Get the saved question ID
                    var correctAnswerIndex = model.Questions[i].CorrectAnswerIndex;

                    foreach (var answerInput in model.Questions[i].Answers)
                    {
                        var answer = new Answer
                        {
                            AnswerText = answerInput.AnswerText,
                            QuestionId = questionId, // Link answer to its respective question
                            IsCorrect = answerInput.AnswerText.Equals(model.Questions[i].Answers[correctAnswerIndex].AnswerText),
                            // You can add more properties as needed
                        };

                        _context.Answers.Add(answer);
                    }
                }

                await _context.SaveChangesAsync(); // Save all answers together
                return RedirectToAction("Index", "Test"); // Redirect to test index after adding questions and answers
            }

            // If model state is not valid, return to the create question view with the same model
            return View(model);
        }





        // GET: Test/Delete/5
        public async Task<IActionResult> Delete(Guid id)
        {
            var test = await _context.Tests.FirstOrDefaultAsync(m => m.Id == id);
            if (test == null)
            {
                return NotFound();
            }

            return View(test);
        }

        // POST: Test/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var test = await _context.Tests.FindAsync(id);
            _context.Tests.Remove(test);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TestExists(Guid id)
        {
            return _context.Tests.Any(e => e.Id == id);
        }
    }
}


