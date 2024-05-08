////using Microsoft.AspNetCore.Mvc;
////using Microsoft.AspNetCore.Http;
////using Microsoft.Extensions.Logging;
////using System;
////using System.IO;
////using Tesseract;
////using System.Diagnostics;
////using Optical_Chracter_Recogntion.Models;

////namespace Optical_Character_Recognition.Controllers
////{
////    public class HomeController : Controller
////    {
////        private readonly ILogger<HomeController> _logger;

////        public HomeController(ILogger<HomeController> logger)
////        {
////            _logger = logger;
////        }

////        public IActionResult Index()
////        {
////            return View();
////        }

////        [HttpPost]
////        public async Task<IActionResult> Index(IFormFile image)
////        {
////            if (image != null && image.Length > 0)
////            {
////                var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

////                // Create directory if it doesn't exist
////                if (!Directory.Exists(uploads))
////                {
////                    Directory.CreateDirectory(uploads);
////                }

////                var fileName = Path.GetRandomFileName() + Path.GetExtension(image.FileName);
////                var filePath = Path.Combine(uploads, fileName);

////                using (var fileStream = new FileStream(filePath, FileMode.Create))
////                {
////                    await image.CopyToAsync(fileStream);
////                }

////                return RedirectToAction("ExtractText", new { imagePath = fileName });
////            }
////            return RedirectToAction("Index");
////        }

////        public IActionResult ExtractText(string imagePath)
////        {
////            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
////            var filePath = Path.Combine(uploadsFolder, imagePath);
////            _logger.LogInformation($"Extracting text from image: {filePath}");

////            var extractedText = ExtractTextFromImage(filePath); // Pass imagePath here
////            ViewBag.ExtractedText = extractedText;

////            // Save the extracted text to a file
////            var textFilePath = Path.Combine(uploadsFolder, "extracted_text.txt");
////            System.IO.File.WriteAllText(textFilePath, extractedText);

////            return File(textFilePath, "text/plain", "extracted_text.txt");
////        }

////        private string ExtractTextFromImage(string imagePath)
////        {
////            try
////            {
////                var tessdataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tessdata");
////                if (!Directory.Exists(tessdataPath))
////                {
////                    _logger.LogError($"Tessdata directory '{tessdataPath}' not found.");
////                    return "Error: Tessdata directory not found.";
////                }

////                var language = "eng";
////                var languageDataFile = Path.Combine(tessdataPath, $"{language}.traineddata");

////                if (!System.IO.File.Exists(languageDataFile))
////                {
////                    _logger.LogError($"Language data file '{languageDataFile}' not found.");
////                    return "Error: Language data file not found.";
////                }

////                using (var engine = new TesseractEngine(tessdataPath, language, EngineMode.Default))
////                {
////                    using (var image = Pix.LoadFromFile(imagePath))
////                    {
////                        if (image == null)
////                        {
////                            _logger.LogError($"Failed to load the image from '{imagePath}'.");
////                            return "Error: Failed to load the image.";
////                        }

////                        using (var page = engine.Process(image))
////                        {
////                            if (page == null)
////                            {
////                                _logger.LogError($"Failed to process the image '{imagePath}'.");
////                                return "Error: Failed to process the image.";
////                            }

////                            return page.GetText();
////                        }
////                    }
////                }
////            }
////            catch (Exception ex)
////            {
////                _logger.LogError($"Error extracting text from image: {ex.Message}");
////                return "Error: Failed to extract text from image.";
////            }
////        }


////        public IActionResult Privacy()
////        {
////            return View();
////        }

////        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
////        public IActionResult Error()
////        {
////            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
////        }
////    }
////}

//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Http;
//using Microsoft.Extensions.Logging;
//using System;
//using System.IO;
//using Tesseract;
//using System.Diagnostics;
//using Optical_Chracter_Recogntion.Models;

//namespace Optical_Character_Recognition.Controllers
//{
//    public class HomeController : Controller
//    {
//        private readonly ILogger<HomeController> _logger;

//        public HomeController(ILogger<HomeController> logger)
//        {
//            _logger = logger;
//        }

//        public IActionResult Index()
//        {
//            return View();
//        }

//        [HttpPost]
//        public async Task<IActionResult> Index(IFormFile image)
//        {
//            if (image != null && image.Length > 0)
//            {
//                try
//                {
//                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

//                    // Create directory if it doesn't exist
//                    if (!Directory.Exists(uploadsFolder))
//                    {
//                        Directory.CreateDirectory(uploadsFolder);
//                    }

//                    var fileName = Path.GetRandomFileName() + Path.GetExtension(image.FileName);
//                    var filePath = Path.Combine(uploadsFolder, fileName);

//                    using (var fileStream = new FileStream(filePath, FileMode.Create))
//                    {
//                        await image.CopyToAsync(fileStream);
//                    }

//                    return RedirectToAction("ExtractText", new { imagePath = fileName });
//                }
//                catch (Exception ex)
//                {
//                    _logger.LogError($"Error uploading image: {ex.Message}");
//                    return RedirectToAction("Index");
//                }
//            }
//            return RedirectToAction("Index");
//        }

//        public IActionResult ExtractText(string imagePath)
//        {
//            try
//            {
//                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
//                var filePath = Path.Combine(uploadsFolder, imagePath);
//                _logger.LogInformation($"Extracting text from image: {filePath}");

//                var extractedText = ExtractTextFromImage(filePath); // Pass imagePath here
//                ViewBag.ExtractedText = extractedText;

//                // Save the extracted text to a file
//                var textFileName = "extracted_text.txt";
//                var textFilePath = Path.Combine(uploadsFolder, textFileName);
//                System.IO.File.WriteAllText(textFilePath, extractedText);

//                return PhysicalFile(textFilePath, "text/plain", textFileName);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError($"Error extracting text: {ex.Message}");
//                return RedirectToAction("Index");
//            }
//        }

//        private string ExtractTextFromImage(string imagePath)
//        {
//            try
//            {
//                var tessdataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tessdata");
//                if (!Directory.Exists(tessdataPath))
//                {
//                    _logger.LogError($"Tessdata directory '{tessdataPath}' not found.");
//                    return "Error: Tessdata directory not found.";
//                }

//                var language = "eng";
//                var languageDataFile = Path.Combine(tessdataPath, $"{language}.traineddata");

//                if (!System.IO.File.Exists(languageDataFile))
//                {
//                    _logger.LogError($"Language data file '{languageDataFile}' not found.");
//                    return "Error: Language data file not found.";
//                }

//                using (var engine = new TesseractEngine(tessdataPath, language, EngineMode.Default))
//                {
//                    using (var image = Pix.LoadFromFile(imagePath))
//                    {
//                        if (image == null)
//                        {
//                            _logger.LogError($"Failed to load the image from '{imagePath}'.");
//                            return "Error: Failed to load the image.";
//                        }

//                        using (var page = engine.Process(image))
//                        {
//                            if (page == null)
//                            {
//                                _logger.LogError($"Failed to process the image '{imagePath}'.");
//                                return "Error: Failed to process the image.";
//                            }

//                            return page.GetText();
//                        }
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError($"Error extracting text from image: {ex.Message}");
//                return "Error: Failed to extract text from image.";
//            }
//        }

//        public IActionResult Privacy()
//        {
//            return View();
//        }

//        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
//        public IActionResult Error()
//        {
//            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
//        }
//    }
//}

//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Http;
//using Microsoft.Extensions.Logging;
//using System;
//using System.IO;
//using Tesseract;
//using System.Diagnostics;
//using Optical_Chracter_Recogntion.Models;

//namespace Optical_Character_Recognition.Controllers
//{
//    public class HomeController : Controller
//    {
//        private readonly ILogger<HomeController> _logger;

//        public HomeController(ILogger<HomeController> logger)
//        {
//            _logger = logger;
//        }

//        public IActionResult Index()
//        {
//            return View();
//        }

//        [HttpPost]
//        public async Task<IActionResult> Index(IFormFile image)
//        {
//            if (image != null && image.Length > 0)
//            {
//                try
//                {
//                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

//                    // Create directory if it doesn't exist
//                    if (!Directory.Exists(uploadsFolder))
//                    {
//                        Directory.CreateDirectory(uploadsFolder);
//                    }

//                    var fileName = Path.GetRandomFileName() + Path.GetExtension(image.FileName);
//                    var filePath = Path.Combine(uploadsFolder, fileName);

//                    using (var fileStream = new FileStream(filePath, FileMode.Create))
//                    {
//                        await image.CopyToAsync(fileStream);
//                    }

//                    var extractedText = ExtractTextFromImage(filePath);

//                    // Pass image path and extracted text to the view
//                    ViewBag.ImagePath = fileName;
//                    ViewBag.ExtractedText = extractedText;

//                    return View("Index");
//                }
//                catch (Exception ex)
//                {
//                    _logger.LogError($"Error uploading image: {ex.Message}");
//                    return RedirectToAction("Index");
//                }
//            }
//            return RedirectToAction("Index");
//        }

//        private string ExtractTextFromImage(string imagePath)
//        {
//            try
//            {
//                var tessdataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tessdata");
//                _logger.LogInformation($"Tessdata path: {tessdataPath}");

//                if (!Directory.Exists(tessdataPath))
//                {
//                    _logger.LogError($"Tessdata directory '{tessdataPath}' not found.");
//                    return "Error: Tessdata directory not found.";
//                }

//                var language = "eng";
//                var languageDataFile = Path.Combine(tessdataPath, $"{language}.traineddata");
//                _logger.LogInformation($"Language data file path: {languageDataFile}");

//                // Ensure that the language data file exists
//                if (!System.IO.File.Exists(languageDataFile))
//                {
//                    // Log the error and return an error message
//                    _logger.LogError($"Language data file '{languageDataFile}' not found.");
//                    return "Error: Language data file not found. Make sure to download and place the 'eng.traineddata' file in the 'tessdata' directory.";
//                }

//                using (var engine = new TesseractEngine(tessdataPath, language, EngineMode.Default))
//                {
//                    using (var image = Pix.LoadFromFile(imagePath))
//                    {
//                        if (image == null)
//                        {
//                            _logger.LogError($"Failed to load the image from '{imagePath}'.");
//                            return "Error: Failed to load the image.";
//                        }

//                        _logger.LogInformation($"Image loaded successfully: {imagePath}");

//                        using (var page = engine.Process(image))
//                        {
//                            if (page == null)
//                            {
//                                _logger.LogError($"Failed to process the image '{imagePath}'.");
//                                return "Error: Failed to process the image.";
//                            }

//                            // Get the recognized text
//                            string extractedText = page.GetText();

//                            if (string.IsNullOrWhiteSpace(extractedText))
//                            {
//                                _logger.LogWarning($"No text was extracted from the image '{imagePath}'.");
//                                return "Error: No text extracted from the image.";
//                            }

//                            // Log the extracted text
//                            _logger.LogInformation($"Extracted text: {extractedText}");

//                            return extractedText;
//                        }
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError($"Error extracting text from image: {ex.Message}");
//                return "Error: Failed to extract text from image.";
//            }
//        }

//        public IActionResult Privacy()
//        {
//            return View();
//        }

//        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
//        public IActionResult Error()
//        {
//            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
//        }
//    }
//}

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
        public async Task<IActionResult> Index(IFormFile image)
        {
            if (image != null && image.Length > 0)
            {
                try
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

                    // Create directory if it doesn't exist
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
            return RedirectToAction("Index");
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
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
