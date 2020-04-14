using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SlackOverload.Models;

namespace SlackOverload.Controllers
{
    public class QandAController : Controller
    {
        private DAL dal;

        public QandAController(IConfiguration config)
        {
            dal = new DAL(config.GetConnectionString("default"));
        }

        public IActionResult Index()
        {
            //get the most recent questions
            IEnumerable<Question> results = dal.GetQuestionsMostRecent();

            ViewData["Questions"] = results;

            return View();
        }

        [HttpGet]
        public IActionResult Add()
        {
            ViewData["username"] = HttpContext.Session.GetString("username");

            if (ViewData["username"] == null)
            {
                return RedirectToAction("LogIn", "Home");
            }

            return View(new Question());
        }

        [HttpPost]
        public IActionResult Add(Question q)
        {
            q.Username = HttpContext.Session.GetString("username");

            int result = dal.CreateQuestion(q);
            

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult AddAnswer(int id)
        {
            ViewData["username"] = HttpContext.Session.GetString("username");

            if (ViewData["username"] == null)
            {
                return RedirectToAction("LogIn", "Home");
            }

            ViewData["Question"] = dal.GetQuestionById(id);
            var ans = new Answer() { QuestionId = id };

            return View(ans);
        }

        [HttpPost]
        public IActionResult AddAnswer(Answer ans, int id)
        {
            ans.Username = HttpContext.Session.GetString("username");

            ans.QuestionId = id;

            int rows = dal.CreateAnswer(ans);

            return RedirectToAction("Detail", new { id = ans.QuestionId });
        }

        public IActionResult Delete(int id)
        {
            int rows = dal.DeleteQuestionByID(id);
            TempData["Thing Deleted"] = $"Deleted Question with ID {id}";
            return RedirectToAction("Index");
        }

        public IActionResult DeleteAnswer(int id)
        {
            int rows = dal.DeleteAnswerByID(id);
            TempData["Thing Deleted"] = $"Deleted Answer with ID {id}";
            return RedirectToAction("Index");
        }

        public IActionResult Detail(int id) {
            //first get the question detail
            Question question = dal.GetQuestionById(id);

            //then get the relevant answers
            IEnumerable<Answer> answers = dal.GetAnswersByQuestionId(id);

            ViewData["Answers"] = answers;

            ViewData["username"] = HttpContext.Session.GetString("username");
    
            return View(question);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            Question q = dal.GetQuestionById(id);

            return View(q);
        }

        [HttpPost]
        public IActionResult Edit(Question q)
        {
            int results = dal.UpdateQuestion(q);


            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult EditAnswer(int id)
        {
            Answer ans = dal.GetAnswerById(id);

            return View(ans);

        }

        [HttpPost]
        public IActionResult EditAnswer(Answer ans, int id)
        {
            ans.Id = id;

            ans = dal.UpdateAnswer(ans);

            return RedirectToAction("Detail", new { id = ans.QuestionId });

        }

        public IActionResult Search(string search)
        {
            IEnumerable<Question> results = dal.GetQuestionsBySearch(search);

            ViewData["Questions"] = results;

            return View("Index");
        }
    }
}