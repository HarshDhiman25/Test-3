
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;
using Tesseract;
using Optical_Character_Recognition.Models;
using Optical_Chracter_Recogntion.Models;
using System.Diagnostics;

namespace Optical_Character_Recognition.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        //public async Task<IActionResult> Index(IFormFile image)
        //{
        //    if (image != null && image.Length > 0)
        //    {
        //        try
        //        {
        //            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

        //            // Create directory if it doesn't exist
        //            if (!Directory.Exists(uploadsFolder))
        //            {
        //                Directory.CreateDirectory(uploadsFolder);
        //            }

        //            var fileName = Path.GetRandomFileName() + Path.GetExtension(image.FileName);
        //            var filePath = Path.Combine(uploadsFolder, fileName);

        //            using (var fileStream = new FileStream(filePath, FileMode.Create))
        //            {
        //                await image.CopyToAsync(fileStream);
        //            }

        //            var extractedText = ExtractTextFromImage(filePath);


        //            ViewBag.ImagePath = fileName;
        //            ViewBag.ExtractedText = extractedText;

        //            return View(); // Pass the model to the view
        //        }
        //        catch (Exception ex)
        //        {
        //            _logger.LogError($"Error uploading image: {ex.Message}");
        //            return View("Error");
        //        }
        //    }
        //    return RedirectToAction("Index");
        //}
        [HttpPost]
        public async Task<IActionResult> Index(IFormFile image)
        {
            if (image != null && image.Length > 0)
            {
                if (image.Length < 10240) 
                {
                    ModelState.AddModelError("image", "The image must be at least 10KB.");
                    return View();
                }

                try
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

                 
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    var fileName = Path.GetRandomFileName() + Path.GetExtension(image.FileName);
                    var filePath = Path.Combine(uploadsFolder, fileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await image.CopyToAsync(fileStream);
                    }

                    var extractedText = ExtractTextFromImage(filePath);

                    ViewBag.ImagePath = fileName;
                    ViewBag.ExtractedText = extractedText;

                    return View(); // Pass the model to the view
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error uploading image: {ex.Message}");
                    return View("Error");
                }
            }
            ModelState.AddModelError("image", "Please select a file.");
            return View();
        }


        private string ExtractTextFromImage(string imagePath)
        {
            try
            {
                var tessdataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tessdata");

                if (!Directory.Exists(tessdataPath))
                {
                    throw new DirectoryNotFoundException($"Tessdata directory '{tessdataPath}' not found.");
                }

                var language = "eng";
                var languageDataFile = Path.Combine(tessdataPath, $"{language}.traineddata");

                // Ensure that the language data file exists
                if (!System.IO.File.Exists(languageDataFile))
                {
                    throw new FileNotFoundException($"Language data file '{languageDataFile}' not found.");
                }

                using (var engine = new TesseractEngine(tessdataPath, language, EngineMode.Default))
                {
                    using (var image = Pix.LoadFromFile(imagePath))
                    {
                        if (image == null)
                        {
                            throw new InvalidOperationException($"Failed to load the image from '{imagePath}'.");
                        }

                        using (var page = engine.Process(image))
                        {
                            if (page == null)
                            {
                                throw new InvalidOperationException($"Failed to process the image '{imagePath}'.");
                            }

                            // Get the recognized text
                            string extractedText = page.GetText();

                            if (string.IsNullOrWhiteSpace(extractedText))
                            {
                                throw new InvalidOperationException($"No text was extracted from the image '{imagePath}'.");
                            }

                            return extractedText;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error extracting text from image: {ex.Message}");
                throw;
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}


        public IActionResult Error()
        {
            var requestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            var errorViewModel = new ErrorViewModel
            {
                RequestId = requestId,
                ShowRequestId = !string.IsNullOrEmpty(requestId)
            };

            return View(errorViewModel);
        }





    }
}
