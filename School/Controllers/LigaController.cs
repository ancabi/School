using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MySql.Data.MySqlClient;
using school.Helpers;
using school.Models;

namespace School.Controllers
{
    [Authorize]
    [SessionState(System.Web.SessionState.SessionStateBehavior.Disabled)]
    public class LigaController : Controller
    {
        // GET: Liga
        public ActionResult Liga()
        {
            return View();
        }

        [HttpPost]
        public JsonResult getLiga()
        {

            RespGeneric resp = new RespGeneric("KO");
            DataTable dt = new DataTable();

            using (MySqlConnection con = new MySqlConnection(BD.CadConMySQL(BD.Server.BDLOCAL, "school")))
            {
                using (MySqlCommand cmd = new MySqlCommand(string.Empty, con))
                {
                    using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                    {
                        cmd.CommandText = "SELECT * FROM school.liga where id_equipo=?id";
                        cmd.Parameters.AddWithValue("?id", 1);
                        da.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            resp.cod = "OK";
                            resp.d.Add("liga", dt.ToList());
                        }
                        else
                        {
                            resp.cod = "KO";
                        }
                    }
                }
            }

            return Json(resp);

        }

        public JsonResult getLigaEquipos(int idLiga)
        {

            RespGeneric resp = new RespGeneric("KO");
            DataTable dt = new DataTable();

            using (MySqlConnection con = new MySqlConnection(BD.CadConMySQL(BD.Server.BDLOCAL, "school")))
            {
                using (MySqlCommand cmd = new MySqlCommand(string.Empty, con))
                {
                    using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                    {
                        cmd.CommandText = "SELECT * FROM school.liga_equipos where id_liga=?id";
                        cmd.Parameters.AddWithValue("?id", idLiga);
                        da.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            resp.cod = "OK";
                            resp.d.Add("ligaequipos", dt.ToList());
                        }
                        else
                        {
                            resp.cod = "KO";
                        }
                    }
                }
            }

            return Json(resp);

        }

        public JsonResult getLigaResultados(int idLiga, int jornada)
        {

            RespGeneric resp = new RespGeneric("KO");
            DataTable dt = new DataTable();

            using (MySqlConnection con = new MySqlConnection(BD.CadConMySQL(BD.Server.BDLOCAL, "school")))
            {
                using (MySqlCommand cmd = new MySqlCommand(string.Empty, con))
                {
                    using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                    {
                        cmd.CommandText = "SELECT t3.nombre local, t1.resultado_local, t2.nombre visitante, t1.resultado_visitante FROM school.liga_partidos t1 inner join school.liga_equipos t2 on t1.id_visitante = t2.id inner join school.liga_equipos t3 on t1.id_local = t3.id where t1.jornada = ?jornada and t1.id_liga = ?id";
                        cmd.Parameters.AddWithValue("?id", idLiga);
                        cmd.Parameters.AddWithValue("?jornada", jornada);
                        da.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            resp.cod = "OK";
                            resp.d.Add("ligaresultados", dt.ToList());
                        }
                        else
                        {
                            resp.cod = "KO";
                        }
                    }
                }
            }

            return Json(resp);

        }

    }
}