using Microsoft.AspNetCore.Http.HttpResults;
using MvcCoreExamenMauricio.Models;
using System.Data;
using System.Data.SqlClient;
#region PROCEDURES
//create procedure SP_INSERT_COMIC
//(@nombre nvarchar(50), @imagen nvarchar(600), @descripcion nvarchar(500))
//as
//	declare @idcomic int
//	select @idcomic=MAX(IDCOMIC)+1 from COMICS
//	insert into COMICS values (@idcomic, @nombre, @imagen, @descripcion)
//go
#endregion
namespace MvcCoreExamenMauricio.Repositories
{
    public class RepositoryComicsSql : IRepositoryComics
    {
        SqlConnection cn;
        SqlCommand com;
        DataTable tablaComics;
        public RepositoryComicsSql()
        {
            string connectionString = @"Data Source=LOCALHOST\SQLEXPRESS;Initial Catalog=HOSPITAL;Persist Security Info=True;User ID=SA;Password=MCSD2023";
            this.cn = new SqlConnection(connectionString);
            this.com = new SqlCommand();
            this.com.Connection = this.cn;
            string sql = "select * from COMICS";
            SqlDataAdapter ad = new SqlDataAdapter(sql,this.cn);
            this.tablaComics = new DataTable();
            ad.Fill(this.tablaComics);
        }

        public List<Comic> GetAllComics()
        {
            var consulta = from datos in this.tablaComics.AsEnumerable()
                           select datos;
            List<Comic> comics = new List<Comic>();
            foreach (var row in consulta)
            {
                Comic comic = new Comic
                {
                    IdComic = row.Field<int>("IDCOMIC"),
                    Nombre = row.Field<string>("NOMBRE"),
                    Imagen = row.Field<string>("IMAGEN"),
                    Descripcion = row.Field<string>("DESCRIPCION")
                };
                comics.Add(comic);
            }
            return comics;
        }

        public int InsertComic(string nombre, string imagen, string descripcion)
        {
            var consulta = from datos in this.tablaComics.AsEnumerable()
                           select datos;
            int idcomic = consulta.Max(o=>o.Field<int>("IDCOMIC"))+1;
            string sql = "insert into COMICS values (@idcomic,@nombre,@imagen,@descripcion)";
            SqlParameter paramidcomic = new SqlParameter("@idcomic", idcomic);
            this.com.Parameters.Add(paramidcomic);
            SqlParameter paramnombre = new SqlParameter("@nombre", nombre);
            this.com.Parameters.Add(paramnombre);
            SqlParameter paramimagen = new SqlParameter("@imagen", imagen);
            this.com.Parameters.Add(paramimagen);
            SqlParameter paramdescripcion = new SqlParameter("@descripcion", descripcion);
            this.com.Parameters.Add(paramdescripcion);
            this.com.CommandType = CommandType.Text;
            this.com.CommandText = sql;
            this.cn.Open();
            int result = this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();
            return result;
        }

        public int InsertComicSP(string nombre, string imagen, string descripcion)
        {
            string sql = "SP_INSERT_COMIC";
            SqlParameter paramnombre = new SqlParameter("@nombre", nombre);
            this.com.Parameters.Add(paramnombre);
            SqlParameter paramimagen = new SqlParameter("@imagen", imagen);
            this.com.Parameters.Add(paramimagen);
            SqlParameter paramdescripcion = new SqlParameter("@descripcion", descripcion);
            this.com.Parameters.Add(paramdescripcion);
            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = sql;
            this.cn.Open();
            int result = this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();
            return result;
        }

        public Comic FindComicById(int idcomic)
        {
            var consulta = from datos in tablaComics.AsEnumerable()
                           where datos.Field<int>("IDCOMIC") == idcomic
                           select datos;
            if (consulta.Count()==0)
            {
                return null;
            }
            var row = consulta.First();
            Comic comic = new Comic
            {
                IdComic = row.Field<int>("IDCOMIC"),
                Nombre = row.Field<string>("NOMBRE"),
                Imagen = row.Field<string>("IMAGEN"),
                Descripcion = row.Field<string>("DESCRIPCION")
            };
            return comic;
        }

        public int DeleteComicById(int idcomic)
        {
            string sql = "delete from COMICS where IDCOMIC=@idcomic";
            SqlParameter paramidcomic = new SqlParameter("@idcomic",idcomic);
            this.com.Parameters.Add(paramidcomic);
            this.com.CommandType = CommandType.Text;
            this.com.CommandText = sql;
            this.cn.Open();
            int result = this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();
            return result;
        }

        

    }
}
