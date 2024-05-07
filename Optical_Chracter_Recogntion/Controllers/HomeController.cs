using Microsoft.AspNetCore.Mvc;
using Optical_Chracter_Recogntion.Models;
using System.Diagnostics;
using Tesseract;

namespace Optical_Chracter_Recogntion.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        public async Task<IActionResult> Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(IFormFile image, [FromServices] IWebHostEnvironment hostingEnvironment)
        {
            if (image != null && image.Length > 0)
            {
                var uploads = Path.Combine(hostingEnvironment.WebRootPath, "uploads");
                var filePath = Path.Combine(uploads, image.FileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(fileStream);
                }
                return RedirectToAction("ExtractText", new { imagePath = filePath });
            }
            return RedirectToAction("Index");
        }

          public string ExtractText(string imagePath)
         {
        using (var engine = new TesseractEngine("./tessdata", "eng", EngineMode.Default))
        {
            using (var image = Pix.LoadFromFile(imagePath))
            {
                using (var page = engine.Process(image))
                {
                    return page.GetText();
                }
            }
        }
    }
        [HttpGet]
        public IActionResult ExtractText(string imagePath, [FromServices] IWebHostEnvironment hostingEnvironment)
        {
            var filePath = Path.Combine(hostingEnvironment.WebRootPath, imagePath);
            var extractedText = ExtractText(filePath);
            ViewBag.ExtractedText = extractedText;
            return View("Index");
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
