using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using PairOfEmployees.Web.Models;
using PairOfEmployees.Web.Services;
using System.Diagnostics;
using X.PagedList;

namespace PairOfEmployees.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEmplyeePairParser _emplyeePairParser;
        private readonly IEmplyeePairService _employeePairService;
        private readonly IMemoryCache _memoryCache;
        private const string CacheKey = "employeePairs";
        private const int PageSize = 10;

        public HomeController(IEmplyeePairParser emplyeePairParser, IEmplyeePairService employeePairService, IMemoryCache memoryCache)
        {
            _emplyeePairParser = emplyeePairParser;
            _employeePairService = employeePairService;
            _memoryCache = memoryCache;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(UploadModel uploadViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(uploadViewModel);
            }

            var records = _emplyeePairParser.GetEmployeeProjects(uploadViewModel.File!);

            if (!records.Any())
            {
                ModelState.AddModelError(string.Empty, "No valid records found in the CSV.");
                return View(uploadViewModel);
            }

            var employeePairs = _employeePairService.GetResult(records);

            _memoryCache.Remove(CacheKey);
            _memoryCache.Set(CacheKey, employeePairs);

            uploadViewModel.Results = new StaticPagedList<PairResult>(employeePairs.Take(PageSize).ToArray(), 1, PageSize, employeePairs.Length);

            return View(uploadViewModel);
        }

        [HttpGet]
        public IActionResult Pairs(int page = 1)
        {
            if (!_memoryCache.TryGetValue(CacheKey, out PairResult[]? pairs) || pairs == null || pairs.Length == 0)
            {
                return RedirectToAction("Index");
            }

            var paged = new StaticPagedList<PairResult>(pairs.Skip((page - 1) * PageSize).Take(PageSize).ToArray(), page, PageSize, pairs.Length);
            var model = new UploadModel { Results = paged };

            return View("Index", model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
