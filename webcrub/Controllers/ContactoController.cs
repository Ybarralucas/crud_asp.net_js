using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using webcrub.Models;
using System.Data.SqlClient;
using System.Data;
using System.Web.Services;
using System.Reflection;
using Microsoft.Ajax.Utilities;
using System.Drawing;

namespace webcrub.Controllers
{
    public class ContactoController : Controller
    {
        // GET: Contacto
        private static string conexion = ConfigurationManager.ConnectionStrings["cadena"].ToString();
        private static List<Contacto> olista = new List<Contacto>();  
        public ActionResult Inicio()
        {
            olista= new List<Contacto>();

            using(SqlConnection oconexion = new SqlConnection(conexion))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM CONTACTO", oconexion);
                cmd.CommandType = CommandType.Text;
                oconexion.Open();
                using(SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Contacto nuevoContacto = new Contacto();
                        nuevoContacto.IdContacto = Convert.ToInt32(dr["IdContacto"]);
                        nuevoContacto.Nombres= dr["Nombres"].ToString();
                        nuevoContacto.Apellidos = dr["Apellidos"].ToString();
                        nuevoContacto.Telefono = dr["Telefono"].ToString();
                        nuevoContacto.Correo = dr["Correo"].ToString();
                        olista.Add(nuevoContacto);
                    }
                }

            }
           
            return View(olista);
        }

        [HttpPost]
        public JsonResult agregar(Contacto cont)
        {

            using (SqlConnection oconexion = new SqlConnection(conexion))
            {
                SqlCommand cmd = new SqlCommand("sp_Registrar", oconexion);
                cmd.Parameters.AddWithValue("Nombres", cont.Nombres);
                cmd.Parameters.AddWithValue("Apellidos", cont.Apellidos);
                cmd.Parameters.AddWithValue("Telefono", cont.Telefono);
                cmd.Parameters.AddWithValue("Correo", cont.Correo);
                cmd.CommandType = CommandType.StoredProcedure;
                oconexion.Open();
                cmd.ExecuteNonQuery();  
              

            }
            string msg = "Agregado!";
            return Json(msg);
        }

        [HttpPost]
        public JsonResult editar(Contacto cont)
        {

            using (SqlConnection oconexion = new SqlConnection(conexion))
            {
                SqlCommand cmd = new SqlCommand("sp_Editar", oconexion);
                cmd.Parameters.AddWithValue("IdContacto", cont.IdContacto);
                cmd.Parameters.AddWithValue("Nombres", cont.Nombres);
                cmd.Parameters.AddWithValue("Apellidos", cont.Apellidos);
                cmd.Parameters.AddWithValue("Telefono", cont.Telefono);
                cmd.Parameters.AddWithValue("Correo", cont.Correo);
                cmd.CommandType = CommandType.StoredProcedure;
                oconexion.Open();
                cmd.ExecuteNonQuery();


            }
            string msg = "Listo :)!";
            return Json(msg);
        }

        [HttpPost]
        public JsonResult mostrar(Contacto idcont)
        {
           
            
            Contacto ocontacto = olista.Where(c=>c.IdContacto== idcont.IdContacto).FirstOrDefault();

            
            return Json(ocontacto);
        }


        [HttpPost]
        public JsonResult eliminar(Contacto idcont)
        {


            using (SqlConnection oconexion = new SqlConnection(conexion))
            {
                SqlCommand cmd = new SqlCommand("sp_Eliminar", oconexion);
                cmd.Parameters.AddWithValue("IdContacto", idcont.IdContacto);
              
                cmd.CommandType = CommandType.StoredProcedure;
                oconexion.Open();
                cmd.ExecuteNonQuery();


            }


            string msg = "Eliminar";
            return Json(msg);
        }
    }
}