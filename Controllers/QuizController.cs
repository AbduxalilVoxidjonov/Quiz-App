using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Quiz_App.Data;
using Quiz_App.Models;

namespace Quiz_App.Controllers
{
    public class QuizController : Controller
    {
        private readonly ApplicationDbContext _context;

        public QuizController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Kategoriyalar sahifasi
        public IActionResult Category()
        {
            var userName = "Guest";
            var categories = _context.Categories
                .Include(c => c.Quizzes)
                .OrderByDescending(c => c.CreatedDate)
                .ToList();

            // Har bir kategoriya uchun barcha urinishlar
            var progresses = _context.UserProgresses
                .Where(p => p.UserName == userName)
                .OrderBy(p => p.AttemptDate)
                .ToList();

            ViewBag.UserProgresses = progresses;

            return View(categories);
        }

        // Kategoriya qo'shish
        [HttpPost]
        public IActionResult CategoryCreate(Category category)
        {
            if (ModelState.IsValid)
            {
                category.CreatedDate = DateTime.Now;
                category.IsActive = true;
                _context.Categories.Add(category);
                _context.SaveChanges();
                return RedirectToAction("Category");
            }
            return RedirectToAction("Category");
        }

        // Testlar ro'yxati
        public IActionResult TestList()
        {
            var quizzes = _context.Quizzes
                .Include(q => q.Category)
                .OrderByDescending(q => q.CreatedDate)
                .ToList();
            return View(quizzes);
        }

        // Test qo'shish sahifasi
        public IActionResult Add()
        {
            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }

        // Test saqlash
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Quiz quiz)
        {
            // Category navigation property validatsiyasini o'chirish
            ModelState.Remove("Category");

            if (ModelState.IsValid)
            {
                quiz.CreatedDate = DateTime.Now;
                _context.Quizzes.Add(quiz);
                _context.SaveChanges();
                return RedirectToAction("TestList");
            }

            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name");
            return View("Add", quiz);
        }

        // Test o'chirish
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var quiz = _context.Quizzes.Find(id);
            if (quiz != null)
            {
                _context.Quizzes.Remove(quiz);
                _context.SaveChanges();
            }
            return RedirectToAction("TestList");
        }

        // Kategoriya bo'yicha test boshlash
        public IActionResult StartQuiz(int categoryId)
        {
            var category = _context.Categories
                .Include(c => c.Quizzes)
                .FirstOrDefault(c => c.Id == categoryId);

            if (category == null || !category.Quizzes.Any())
            {
                return RedirectToAction("Category");
            }

            // Tasodifiy tartibda testlarni olish
            var quizzes = category.Quizzes.OrderBy(q => Guid.NewGuid()).ToList();

            // Session ga saqlash
            HttpContext.Session.SetString("CurrentQuizzes", System.Text.Json.JsonSerializer.Serialize(quizzes.Select(q => q.Id).ToList()));
            HttpContext.Session.SetInt32("CurrentQuizIndex", 0);
            HttpContext.Session.SetInt32("CorrectAnswers", 0);
            HttpContext.Session.SetInt32("CategoryId", categoryId);

            return RedirectToAction("TakeQuiz");
        }

        // Test ishlash sahifasi
        public IActionResult TakeQuiz()
        {
            var quizIdsJson = HttpContext.Session.GetString("CurrentQuizzes");
            var currentIndex = HttpContext.Session.GetInt32("CurrentQuizIndex") ?? 0;

            if (string.IsNullOrEmpty(quizIdsJson))
            {
                return RedirectToAction("Category");
            }

            var quizIds = System.Text.Json.JsonSerializer.Deserialize<List<int>>(quizIdsJson);

            // Barcha savollar tugasa - QuizResult ga o'tish
            if (currentIndex >= quizIds.Count)
            {
                return RedirectToAction("QuizResult");
            }

            var quiz = _context.Quizzes
                .Include(q => q.Category)
                .FirstOrDefault(q => q.Id == quizIds[currentIndex]);

            if (quiz == null)
            {
                return RedirectToAction("Category");
            }

            ViewBag.CurrentQuestion = currentIndex + 1;
            ViewBag.TotalQuestions = quizIds.Count;

            return View(quiz);
        }

        // Javobni tekshirish
        [HttpPost]
        public IActionResult CheckAnswer(int quizId, int selectedAnswer)
        {
            var quiz = _context.Quizzes.Find(quizId);
            var currentIndex = HttpContext.Session.GetInt32("CurrentQuizIndex") ?? 0;
            var correctAnswers = HttpContext.Session.GetInt32("CorrectAnswers") ?? 0;

            // Javobni tekshirish
            if (quiz != null && quiz.CorrectAnswer == selectedAnswer)
            {
                correctAnswers++;
                HttpContext.Session.SetInt32("CorrectAnswers", correctAnswers);
            }

            // Keyingi savolga o'tish
            HttpContext.Session.SetInt32("CurrentQuizIndex", currentIndex + 1);

            return RedirectToAction("TakeQuiz");
        }

        // Natijalar sahifasi
        public IActionResult QuizResult()
        {
            var correctAnswers = HttpContext.Session.GetInt32("CorrectAnswers") ?? 0;
            var quizIdsJson = HttpContext.Session.GetString("CurrentQuizzes");
            var categoryId = HttpContext.Session.GetInt32("CategoryId") ?? 0;

            if (string.IsNullOrEmpty(quizIdsJson))
            {
                return RedirectToAction("Category");
            }

            var totalQuestions = System.Text.Json.JsonSerializer.Deserialize<List<int>>(quizIdsJson).Count;
            var percentage = (correctAnswers * 100) / totalQuestions;

            // Progressni saqlash
            var userName = "Guest";
            var attemptCount = _context.UserProgresses
                .Count(p => p.UserName == userName && p.CategoryId == categoryId);

            var progress = new UserProgress
            {
                UserName = userName,
                CategoryId = categoryId,
                AttemptNumber = attemptCount + 1,
                Score = percentage,
                CorrectAnswers = correctAnswers,
                TotalQuestions = totalQuestions,
                AttemptDate = DateTime.Now
            };

            _context.UserProgresses.Add(progress);
            _context.SaveChanges();

            ViewBag.CorrectAnswers = correctAnswers;
            ViewBag.TotalQuestions = totalQuestions;
            ViewBag.Percentage = percentage;

            // Session ni tozalash
            HttpContext.Session.Remove("CurrentQuizzes");
            HttpContext.Session.Remove("CurrentQuizIndex");
            HttpContext.Session.Remove("CorrectAnswers");
            HttpContext.Session.Remove("CategoryId");

            return View();
        }

        // Progress sahifasi
        public IActionResult Progress()
        {
            var userName = "Guest";

            var progresses = _context.UserProgresses
                .Include(p => p.Category)
                .Where(p => p.UserName == userName)
                .OrderByDescending(p => p.AttemptDate)
                .ToList();

            return View(progresses);
        }
    }
}