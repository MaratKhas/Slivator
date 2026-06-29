using Microsoft.AspNetCore.Mvc;

namespace Slivator.Controllers
{

    /// <summary>
    /// Задание
    /// 1) реализовать метод UplodadFile который будет подгружать файл на диск и сохранять данные о нем в InMemoryCache(см в Bases)
    /// 2) Index возвращает страницу с списком загруженных файлов на диск, который можно выбрать и айдишник передается в ListPartial
    /// 3) ListPartial принимает айдишник файла читает его с помощью BaseExcelReader и возвращает страницу ListPartial в которой в табличке раскиданы данные из исходного файла
    /// 4) страницу ListPartial нужно будет реализовать
    /// </summary>
    public class SlivatorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        //public IActionResult ListPartial(string filePath)
        //{ 
        //    // todo: на основе данных прочитанных с файла использовать BaseExcelReader
        //}

        //public ActionResult UplodadFile(IFile)
        //{

        //}
    }
}
