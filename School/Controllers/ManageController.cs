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

        public ActionResult Equipos()
		{
			return View();
		}


        [HttpPost]
        public JsonResult LoadTiposUsuarios()
        {
            RespGeneric resp = new RespGeneric("KO");
            DataTable dt = new DataTable();
            try
            {
                using (MySqlConnection con = new MySqlConnection(BD.CadConMySQL(BD.Server.BDLOCAL, BD.schema)))
                {
                    using (MySqlCommand cmd = new MySqlCommand("SELECT * FROM usuarios_tipos", con))
                    {

                        using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                        {

                            da.Fill(dt);
                            if (dt.Rows.Count > 0)
                            {
                                resp.cod = "OK";
                                resp.d.Add("tipos", dt.ToList());
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
        public JsonResult LoadUsuarios()
        {
            RespGeneric resp = new RespGeneric("KO");
            DataTable dt = new DataTable();
            try
            {
                using (MySqlConnection con = new MySqlConnection(BD.CadConMySQL(BD.Server.BDLOCAL, BD.schema)))
                {
                    using (MySqlCommand cmd = new MySqlCommand("SELECT * FROM usuarios u LEFT JOIN usuarios_tipos t ON t.id=u.tipo", con))
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
            int autorizacion,int tipo, int pagado)
        {

            RespGeneric resp = new RespGeneric("KO");

            try
            {
                string query = "";

                if (password.IsEmpty())
                {
                    query =
                        "UPDATE usuarios SET dni = ?dni, nombre = ?nombre,apellidos = ?apellidos,telefono = ?telefono,telefonoalt = ?telefonoAlt,email = ?email,usuario = ?usuario, " +
                        "tipo = ?tipo, autorizacion=?autorizacion,pagado=?pagado WHERE id = ?id;";
                }
                else
                {
                    query =
                        "UPDATE usuarios SET dni = ?dni,nombre = ?nombre,apellidos = ?apellidos,telefono = ?telefono,telefonoalt = ?telefonoAlt,email = ?email,usuario = ?usuario,pass = ?password," +
                        "tipo = ?tipo, autorizacion=?autorizacion,pagado=?pagado WHERE id = ?id;";
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
                        cmd.Parameters.AddWithValue("?telefonoAlt", telefonoAlt);
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
                        cmd.Parameters.AddWithValue("?pagado", pagado);
                        
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

        [HttpPost]
        public JsonResult RefreshPagados()
        {
            RespGeneric resp = new RespGeneric("KO");
            DataTable dt = new DataTable();
            try
            {
                using (MySqlConnection con = new MySqlConnection(BD.CadConMySQL(BD.Server.BDTIENDA, BD.schemaTienda)))
                {
                    using (MySqlCommand cmd = new MySqlCommand("SELECT o.id_customer,SUBSTRING(message,INSTR(message,'Num Aut: ')+9,6) AS numaut FROM elatabaltienda.ps1_orders o INNER JOIN ps1_message m ON m.id_order=o.id_order WHERE current_state=9 GROUP BY id_customer;", con))
                    {
                        using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                        {
                            da.Fill(dt);
                        }
                        resp.cod = "OK";
                    }
                }

                List<Dictionary<string,object>> lista = dt.ToList();
                string query = "";

                foreach (Dictionary<string, object> l in lista)
                {
                    query += $"UPDATE usuarios SET pagado=1, numaut={l["numaut"]} WHERE idtienda={l["id_customer"]};";
                }


                using (MySqlConnection con = new MySqlConnection(BD.CadConMySQL(BD.Server.BDLOCAL, BD.schema)))
                {
                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                        resp.cod = "OK";
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
        public JsonResult ExportExcel()
        {
            RespGeneric resp = new RespGeneric("KO");
            DataTable dt = new DataTable();
            try
            {
                using (MySqlConnection con = new MySqlConnection(BD.CadConMySQL(BD.Server.BDLOCAL, BD.schema)))
                {
                    using (MySqlCommand cmd = new MySqlCommand("SELECT u.dni,u.nombre, u.apellidos,u.email,numaut,u.usuario,IF(u.autorizacion=1,'Si','No') AS autorizacion,IF(u.pagado=1,'Si','No') AS Pagado,u.fecha_registro,telefono,telefonoAlt,h.nombre,h.apellidos,h.fecha_nacimiento,h.sexo,IF(h.extraescolares=1,'Si','No') AS extraescolares,IF(h.pack=1,'Si','No') AS pack,talla,numero,h.observaciones,IF(d.iddeporte=1,'Futbol','Baloncesto') AS deporte FROM usuarios u INNER JOIN usuarios_hijos h ON u.id=h.idpadre INNER JOIN usuarios_hijos_deportes d ON d.idhijo=h.id;", con))
                    {
                        using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                        {
                            da.Fill(dt);
                        }
                        resp.cod = "OK";
                        resp.d.Add("excel",dt.ToList());
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

        #region equipos

        [HttpPost]
        public JsonResult LoadEquipos()
        {
            RespGeneric resp = new RespGeneric("KO");
            DataTable dt = new DataTable();
            try
            {
                using (MySqlConnection con = new MySqlConnection(BD.CadConMySQL(BD.Server.BDLOCAL, BD.schema)))
                {
                    using (MySqlCommand cmd = new MySqlCommand("SELECT e.*, d.nombre AS deporte FROM equipos e LEFT JOIN deportes d ON e.iddeporte=d.id", con))
                    {

                        using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                        {

                            da.Fill(dt);
                            if (dt.Rows.Count > 0)
                            {
                                resp.cod = "OK";
                                resp.d.Add("equipos", dt.ToList());
                            }
                            else
                            {
                                resp.cod = "KO";
                                resp.msg = "Ningún equipo encontrado!";
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
        public JsonResult LoadJugadores(int idEquipo)
        {
            RespGeneric resp = new RespGeneric("KO");
            DataTable dt = new DataTable();
            try
            {
                using (MySqlConnection con = new MySqlConnection(BD.CadConMySQL(BD.Server.BDLOCAL, BD.schema)))
                {
                    using (MySqlCommand cmd = new MySqlCommand("SELECT h.* FROM usuarios_hijos h INNER JOIN usuarios_hijos_deportes hd ON h.id=hd.idhijo INNER JOIN equipos e ON e.iddeporte=hd.iddeporte " +
                                                               "LEFT JOIN usuarios_hijos_equipos he ON he.idhijo = h.id WHERE e.id = ?idEquipo AND he.idhijo IS NULL; ", con))
                    {
                        cmd.Parameters.AddWithValue("?idEquipo", idEquipo);
                        using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                        {

                            da.Fill(dt);
                            if (dt.Rows.Count > 0)
                            {
                                resp.cod = "OK";
                                resp.d.Add("libres", dt.ToList());
                            }
                            else
                            {
                                resp.cod = "KO";
                                resp.msg = "Ningún equipo encontrado!";
                            }
                        }
                    }
                    dt.Reset();
                    using (MySqlCommand cmd = new MySqlCommand("SELECT h.* FROM usuarios_hijos h INNER JOIN usuarios_hijos_deportes hd ON h.id=hd.idhijo INNER JOIN equipos e ON e.iddeporte=hd.iddeporte " +
                                                               "LEFT JOIN usuarios_hijos_equipos he ON he.idhijo = h.id WHERE e.id = ?idEquipo AND he.idhijo IS NOT NULL; ", con))
                    {
                        cmd.Parameters.AddWithValue("?idEquipo", idEquipo);
                        using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                        {

                            da.Fill(dt);
                            if (dt.Rows.Count > 0)
                            {
                                resp.cod = "OK";
                                resp.d.Add("jugadoresEquipo", dt.ToList());
                            }
                            else
                            {
                                resp.cod = "KO";
                                resp.msg = "Ningún equipo encontrado!";
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
        #endregion

    }
}