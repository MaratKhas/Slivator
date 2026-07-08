using Microsoft.AspNetCore.Mvc;
using Slivator.Bases;
using Slivator.Models;

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

        private static readonly InMemoryCache<Guid, FileHolder> _cache = new InMemoryCache<Guid, FileHolder>();

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

        //public IActionResult Process(Guid fileId)
        //{
        //    _cache.TryGetValue(fileId, out FileHolder fileHolder);

        //    var rows = new BaseExcelReader<SlivatorModel>(fileHolder.FilePath())
        //        .ReadRows();
        //    return null;
        //}
    }
}
