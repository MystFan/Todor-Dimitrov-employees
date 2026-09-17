using Microsoft.AspNetCore.Mvc;
using PairOfEmployees.Web.Models;
using PairOfEmployees.Web.Services;
using System.Diagnostics;

namespace PairOfEmployees.Web.Controllers
{
    public class HomeController(IEmplyeePairParser emplyeePairParser, IEmplyeePairService employeePairService) : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(UploadModel uploadViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(uploadViewModel);
            }

            var records = emplyeePairParser.GetEmployeeProjects(uploadViewModel.File!);

            if (!records.Any())
            {
                ModelState.AddModelError(string.Empty, "No valid records found in the CSV.");
                return View(uploadViewModel);
            }

            uploadViewModel.Results = employeePairService.GetResult(records);

            return View(uploadViewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
