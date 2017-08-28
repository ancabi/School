using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using MySql.Data.MySqlClient;
using school.Controllers;
using school.Helpers;
using school.Models;

namespace School.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {

		public ActionResult Usuarios()
		{
			return View();
		}


        [HttpPost]
        public JsonResult LoadUsuarios()
        {
            RespGeneric resp = new RespGeneric("KO");
            DataTable dt = new DataTable();
            try
            {
                using (MySqlConnection con = new MySqlConnection(BD.CadConMySQL(BD.Server.BDLOCAL, BD.schema)))
                {
                    using (MySqlCommand cmd = new MySqlCommand("SELECT * FROM usuarios", con))
                    {

                        using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                        {

                            da.Fill(dt);
                            if (dt.Rows.Count > 0)
                            {
                                resp.cod = "OK";
                                resp.d.Add("usuarios", dt.ToList());
                            }
                            else
                            {
                                resp.cod = "KO";
                                resp.msg = "Ningún usuario encontrado!";
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                resp.cod = "KO";
                resp.msg = e.Message;
            }
            return Json(resp);
        }

        [HttpPost]
        public JsonResult LoadHijos(int idPadre)
        {
            RespGeneric resp = new RespGeneric("KO");
            DataTable dt = new DataTable();
            try
            {
                using (MySqlConnection con = new MySqlConnection(BD.CadConMySQL(BD.Server.BDLOCAL, BD.schema)))
                {
                    using (
                        MySqlCommand cmd = new MySqlCommand("SELECT * FROM usuarios_hijos h WHERE idpadre=?idPadre", con)
                    )
                    {
                        cmd.Parameters.AddWithValue("?idPadre", idPadre);
                        using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                        {

                            da.Fill(dt);
                        }
                    }
                    if (dt.Rows.Count > 0)
                            {

                                List<Dictionary<string, object>> hijos = dt.ToList();
                                dt.Reset();

                                foreach (Dictionary<string, object> hijo in hijos)
                                {
                                    using (MySqlCommand cmd =new MySqlCommand("SELECT * FROM usuarios_hijos_deportes hd INNER JOIN deportes d ON d.id=hd.iddeporte WHERE idhijo=?idHijo",
                                                con))
                                    {
                                        cmd.Parameters.AddWithValue("?idHijo", hijo["id"]);
                                        using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                                        {
                                            da.Fill(dt);
                                        }
                                    }

                                    hijo.Add("deportes",dt.ToList());
                                }

                                resp.cod = "OK";
                                resp.d.Add("hijos", hijos);
                            }
                            else
                            {
                                resp.cod = "KO";
                                resp.msg = "Ningún hijo encontrado!";
                            }
                        }
                    
                
            }
            catch (Exception e)
            {
                resp.cod = "KO";
                resp.msg = e.Message;
            }
            return Json(resp);
        }

        [HttpPost]
        public JsonResult saveUsuario(int id, string dni, string nombre, string apellidos, string telefono, string telefonoAlt,string email, string usuario, string password,
            string autorizacion,int tipo)
        {

            RespGeneric resp = new RespGeneric("KO");

            try
            {
                string query = "";

                if (password.IsEmpty())
                {
                    query =
                        "UPDATE usuarios SET dni = ?dni, nombre = ?nombre,apellidos = ?apellidos,telefono = ?telefono,email = ?email,usuario = ?usuario, " +
                        "tipo = ?tipo, autorizacion=?autorizacion WHERE id = ?id;";
                }
                else
                {
                    query =
                        "UPDATE usuarios SET dni = ?dni,nombre = ?nombre,apellidos = ?apellidos,telefono = ?telefono,telefonoalt = ?telefonoAlt,email = ?email,usuario = ?usuario,password = ?password," +
                        "tipo = ?tipo, autorizacion=?autorizacion WHERE id = ?id;";
                }

                using (MySqlConnection con = new MySqlConnection(BD.CadConMySQL(BD.Server.BDLOCAL, BD.schema)))
                {
                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {

                        cmd.Parameters.AddWithValue("?id", id);
                        cmd.Parameters.AddWithValue("?dni", dni);
                        cmd.Parameters.AddWithValue("?nombre", nombre);
                        cmd.Parameters.AddWithValue("?apellidos", apellidos);
                        cmd.Parameters.AddWithValue("?telefono", telefono);
                        cmd.Parameters.AddWithValue("?email", email);
                        cmd.Parameters.AddWithValue("?usuario", usuario);
                        if (!password.IsEmpty())
                        {
                            String salt = BD.CreateSalt(8);
                            String hashPass = BD.HashPassword(password, salt);
                            cmd.Parameters.AddWithValue("?password", hashPass);
                        }
                        cmd.Parameters.AddWithValue("?tipo", tipo);
                        cmd.Parameters.AddWithValue("?autorizacion", autorizacion);
                        
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }

                resp.cod = "OK";

                return Json(resp);
            }
            catch (Exception e)
            {
                resp.msg = e.Message;

                return Json(resp);
            }
        }

    }
}