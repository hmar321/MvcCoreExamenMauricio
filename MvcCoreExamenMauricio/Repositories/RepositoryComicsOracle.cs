using MvcCoreExamenMauricio.Models;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Collections.Generic;
#region PROCEDURE

//create or replace procedure sp_insert_comic
//(p_nombre COMICS.NOMBRE%TYPE, p_imagen COMICS.NOMBRE%TYPE, p_descripcion COMICS.DESCRIPCION%TYPE)
//as
//  p_idcomic int;
//begin
//    select MAX(IDCOMIC)+1 into p_idcomic from COMICS;
//insert into COMICS values (p_idcomic, p_nombre, p_imagen, p_descripcion);
//commit;
//end;

#endregion
namespace MvcCoreExamenMauricio.Repositories
{
    public class RepositoryComicsOracle : IRepositoryComics
    {
        OracleConnection cn;
        OracleCommand com;
        DataTable tablaComics;
        public RepositoryComicsOracle()
        {
            string connectionString = "Data Source=localhost:1521/XE;Persist Security Info=True; User Id=SYSTEM; Password=oracle";
            this.cn = new OracleConnection(connectionString);
            this.com = new OracleCommand();
            this.com.Connection = this.cn;
            string sql = "select * from COMICS";
            OracleDataAdapter ad = new OracleDataAdapter(sql, this.cn);
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
            int idcomic = consulta.Max(o => o.Field<int>("IDCOMIC")) + 1;
            string sql = "insert into COMICS values (:idcomic,:nombre,:imagen,:descripcion)";
            OracleParameter paramidcomic = new OracleParameter(":idcomic", idcomic);
            this.com.Parameters.Add(paramidcomic);
            OracleParameter paramnombre = new OracleParameter(":nombre", nombre);
            this.com.Parameters.Add(paramnombre);
            OracleParameter paramimagen = new OracleParameter(":imagen", imagen);
            this.com.Parameters.Add(paramimagen);
            OracleParameter paramdescripcion = new OracleParameter(":descripcion", descripcion);
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
            string sql = "sp_insert_comic";
            OracleParameter paramnombre = new OracleParameter(":nombre", nombre);
            this.com.Parameters.Add(paramnombre);
            OracleParameter paramimagen = new OracleParameter(":imagen", imagen);
            this.com.Parameters.Add(paramimagen);
            OracleParameter paramdescripcion = new OracleParameter(":descripcion", descripcion);
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
            if (consulta.Count() == 0)
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
            string sql = "delete from COMICS where IDCOMIC=:idcomic";
            OracleParameter paramidcomic = new OracleParameter(":idcomic", idcomic);
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
