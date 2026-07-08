using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Slivator.Bases;
using Slivator.Models;
using System.Diagnostics;


namespace Slivator.Controllers
{
    public class HomeController : Controller
    {
        private readonly InMemoryCache<string, object> _cache;

        public HomeController()
        {
            _cache = new InMemoryCache<string, object>(TimeSpan.FromMinutes(60));
        }

        private readonly string _uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "UserData/Files");

        public IActionResult Upload() => View();

        public IActionResult Index()
        {
            var files = Directory.Exists(_uploadPath)
                ? Directory.GetFiles(_uploadPath).Select(Path.GetFileName).ToList()
                : new List<string>();
            string directory = Directory.GetCurrentDirectory();
            //return View(files);
            TempData["Message"] = directory;
            return View(files);
        }

        [HttpPost]
        public IActionResult Process(List<string> selectedFileIds)
        {
            if (selectedFileIds != null && selectedFileIds.Any())
            {
                TempData["Message"] = $"Выбрано: {string.Join(", ", selectedFileIds)}";
            }
            else
            {
                TempData["Message"] = "Ничего не выбрано";
            }

            return RedirectToAction("Index");
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile(IFormFileCollection files)
        {
            if (files == null || files.Count == 0)
            {
                return BadRequest(new { error = "Файлы не выбраны или пусты" });
            }

            try
            {
                string uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "UserData/Files");

                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                var uploadedFiles = new List<FileHolder>();

                foreach (var file in files)
                {
                    // Сохраняем с оригинальным расширением
                    string originalFileName = Path.GetFileName(file.FileName);
                    string uniqueFileName = $"{Guid.NewGuid()}_{originalFileName}";
                    string filePath = Path.Combine(uploadPath, uniqueFileName);

                    // Сохраняем файл на диск
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }

                    var fileHolder = new FileHolder
                    {
                        Id = Guid.NewGuid(),
                        OriginalFileName = file.FileName,
                        SavedFileName = uniqueFileName,
                        FilePath = filePath,
                        FileSize = file.Length,
                        ContentType = file.ContentType,
                        UploadDate = DateTime.UtcNow
                    };

                    uploadedFiles.Add(fileHolder);

                    // Сохраняем в кэш
                    _cache.Set(fileHolder.Id.ToString(), fileHolder, TimeSpan.FromHours(24));
                }

                return Ok(new
                {
                    message = $"Успешно загружено {files.Count} файлов",
                    fileIds = uploadedFiles.Select(f => f.Id)
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"Ошибка при загрузке файла: {ex.Message}" });
            }
        }
        public IActionResult Table(string fileName = null)
        {
            try
            {
                string uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "UserData/Files");

                if (string.IsNullOrEmpty(fileName))
                {
                    var files = Directory.GetFiles(uploadPath, "*.xlsx")
                        .Concat(Directory.GetFiles(uploadPath, "*.xls"))
                        .Concat(Directory.GetFiles(uploadPath, "*.csv"))
                        .Select(Path.GetFileName)
                        .ToList();
                    Console.WriteLine(files);
                    return View("FileList", files);
                }

                string filePath = Path.Combine(uploadPath, fileName);

                if (!System.IO.File.Exists(filePath))
                    return NotFound($"Файл {fileName} не найден");

                // Выбираем метод чтения в зависимости от расширения
                var extension = Path.GetExtension(filePath).ToLower();
                List<Dictionary<string, string>> rows = null;

                if (extension == ".csv")
                {
                    var reader = new DynamicExcelReaderEPPlus();
                    rows = reader.ReadRows(filePath);
                }
                else if (extension == ".xlsx" || extension == ".xls")
                {
                    // Используйте один из Excel-ридеров
                    var reader = new DynamicExcelReaderEPPlus(); // или DynamicExcelReaderClosedXml
                    rows = reader.ReadRows(filePath);
                }
                else
                {
                    return BadRequest("Неподдерживаемый формат файла");
                }
                Console.WriteLine(rows);
                // Передаём данные в представление
                ViewBag.FileName = fileName;
                return View("TablePaint", rows);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка: {ex.Message}");
            }
        }
    }
}
