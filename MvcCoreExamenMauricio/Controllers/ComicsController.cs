using Microsoft.AspNetCore.Mvc;
using MvcCoreExamenMauricio.Models;
using MvcCoreExamenMauricio.Repositories;

namespace MvcCoreExamenMauricio.Controllers
{
    public class ComicsController : Controller
    {
        private IRepositoryComics repo;

        public ComicsController(IRepositoryComics repo)
        {
            this.repo = repo;
        }

        public IActionResult Index()
        {
            List<Comic> comics = this.repo.GetAllComics();
            return View(comics);
        }

        public IActionResult Crear()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Crear(Comic comic)
        {
            this.repo.InsertComic(comic.Nombre, comic.Imagen, comic.Descripcion);
            return RedirectToAction("Index");
        }

        public IActionResult CrearSP()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CrearSP(Comic comic)
        {
            this.repo.InsertComicSP(comic.Nombre, comic.Imagen, comic.Descripcion);
            return RedirectToAction("Index");
        }

        public IActionResult DatosComic()
        {
            ViewData["COMICS"] = this.repo.GetAllComics();
            return View();
        }
        [HttpPost]
        public IActionResult DatosComic(int idcomic)
        {
            ViewData["COMICS"] = this.repo.GetAllComics();
            Comic comic = this.repo.FindComicById(idcomic);
            return View(comic);
        }

        public IActionResult Delete(int idcomic)
        {
            Comic comic = this.repo.FindComicById(idcomic);
            return View(comic);
        }

        public IActionResult DeleteConfirm(int idcomic)
        {
            this.repo.DeleteComicById(idcomic);
            return RedirectToAction("Index");
        }
    }
}
