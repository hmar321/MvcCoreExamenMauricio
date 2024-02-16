using MvcCoreExamenMauricio.Models;

namespace MvcCoreExamenMauricio.Repositories
{
    public interface IRepositoryComics
    {
        List<Comic> GetAllComics();
        int InsertComic(string nombre, string imagen, string descripcion);
        int InsertComicSP(string nombre, string imagen, string descripcion);
        Comic FindComicById(int idcomic);
        int DeleteComicById(int idcomic);
    }
}
