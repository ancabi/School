using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using MySql.Data.MySqlClient;
using school.Helpers;
using school.Models;

namespace School.Controllers
{
    public class ComunicadosController : Controller
    {
        // GET: Comunicados
        public ActionResult Enviar()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Enviar(string titulo, string descripcion, DateTime fecha, List<String> equipos )
        {
            RespGeneric resp = new RespGeneric("KO");
            int numReg = 0;
            string idequipos = "";
            List<String> emails=new List<string>();
            foreach (string equipo in equipos)
            {
                idequipos += equipo + ";";
                emails.AddRange(getEmails(equipo));
            }

            Extensiones.sendEmail(emails, titulo, descripcion, null);
            
            using (MySqlConnection con = new MySqlConnection(BD.CadConMySQL(BD.Server.BDLOCAL, BD.schema)))
            {
                using (MySqlCommand cmd = new MySqlCommand(string.Empty, con))
                {

                        con.Open();
                        cmd.CommandText = "INSERT INTO school.comunicados (titulo, descripcion, fecha, idequipos, idusuario) VALUES (?titulo, ?descripcion, ?fecha, ?idequipos, ?idusuario)";
                        cmd.Parameters.AddWithValue("?titulo", titulo);
                        cmd.Parameters.AddWithValue("?descripcion", descripcion);
                        cmd.Parameters.AddWithValue("?fecha", fecha);
                        cmd.Parameters.AddWithValue("?idequipos", idequipos);
                        cmd.Parameters.AddWithValue("?idusuario", Session["idusuario"]);
                        
                        numReg = (int)cmd.ExecuteNonQuery();
                        if (numReg == 1)
                        {
                            resp.cod = "OK";
                        }
                        else
                        {
                            resp.cod = "KO";
                        }
                        con.Close();
                    
                }
            }
            return Json(resp);
        }

        
        [HttpPost]
        public JsonResult getComunicados()
        {
            RespGeneric resp = new RespGeneric("KO");
            DataTable dt = new DataTable();

            using (MySqlConnection con = new MySqlConnection(BD.CadConMySQL(BD.Server.BDLOCAL, BD.schema)))
            {
                using (MySqlCommand cmd = new MySqlCommand(string.Empty, con))
                {
                    using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                    {
                        cmd.CommandText = "SELECT * FROM school.comunicados WHERE idusuario=?id";
                        cmd.Parameters.AddWithValue("?id", Session["idusuario"]);
                        da.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            resp.cod = "OK";
                            resp.d.Add("comunicados", dt.ToList());
                        }
                        else
                        {
                            resp.cod = "KO";
                            resp.msg = "Ningún comunicado encontrado!";
                        }
                    }
                }
            }
            return Json(resp);
        }

        private List<string> getEmails(string equipo)
        {
            DataTable dt = new DataTable();

            using (MySqlConnection con = new MySqlConnection(BD.CadConMySQL(BD.Server.BDLOCAL, BD.schema)))
            {
                using (MySqlCommand cmd = new MySqlCommand(string.Empty, con))
                {
                    using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                    {
                        cmd.CommandText = "SELECT email FROM school.usuarios u INNER JOIN equipos_jugadores j ON j.idusuario=u.id AND j.id_equipo=?idequipo";
                        cmd.Parameters.AddWithValue("?idequipo", equipo);
                        da.Fill(dt);
                    }
                }
            }
            return dt.ToSingleList();
        }
    }
}
