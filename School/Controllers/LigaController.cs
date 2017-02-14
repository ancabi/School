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
        public JsonResult getLiga(int idEquipo)
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
                        cmd.Parameters.AddWithValue("?id", idEquipo);
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

        public JsonResult getJornadas(int idLiga)
        {

            RespGeneric resp = new RespGeneric("KO");
            DataTable dt = new DataTable();

            using (MySqlConnection con = new MySqlConnection(BD.CadConMySQL(BD.Server.BDLOCAL, "school")))
            {
                using (MySqlCommand cmd = new MySqlCommand(string.Empty, con))
                {
                    using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                    {
                        cmd.CommandText = "SELECT p.*, l.nombre AS 'local', v.nombre AS visitante FROM school.liga_partidos p INNER JOIN liga_equipos l ON l.id = p.id_local INNER JOIN liga_equipos v ON v.id = p.id_visitante where p.id_liga=?id";
                        cmd.Parameters.AddWithValue("?id", idLiga);
                        da.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            resp.cod = "OK";
                            resp.d.Add("jornadas", dt.ToList());
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

        [HttpPost]
        public JsonResult addJornada(List<Dictionary<string,object>> locales,List<Dictionary<string,object>> visitantes )
        {
            RespGeneric resp = new RespGeneric("KO");

            long rows = 0;

            string jornada = getSiguienteJornada(Int32.Parse(locales[0]["id_liga"].ToString()));

            using (MySqlConnection con = new MySqlConnection(BD.CadConMySQL(BD.Server.BDLOCAL, "school")))
            {
                using (MySqlCommand cmd = new MySqlCommand(string.Empty, con))
                {
                        
                        cmd.CommandText = "INSERT INTO liga_partidos(id_local,id_visitante,id_liga,jornada) VALUES(?local,?visitante,?liga,?jornada)";

                    for (int x = 0; x < locales.Count; x++)
                    {
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("?local", locales[x]["id"]);
                        cmd.Parameters.AddWithValue("?visitante", visitantes[x]["id"]);
                        cmd.Parameters.AddWithValue("?liga", visitantes[x]["id_liga"]);
                        cmd.Parameters.AddWithValue("?jornada", jornada);

                        con.Open();
                        rows+=cmd.ExecuteNonQuery();
                        con.Close();

                    }
                        if (rows == locales.Count)
                        {
                            resp.cod = "OK";
                        }
                        else
                        {
                            resp.cod = "KO";
                            resp.msg = "Error";
                        }
                    
                }
            }
            return Json(resp);
        }

        public string getSiguienteJornada(int idLiga)
        {

            RespGeneric resp = new RespGeneric("KO");
            DataTable dt = new DataTable();

            using (MySqlConnection con = new MySqlConnection(BD.CadConMySQL(BD.Server.BDLOCAL, "school")))
            {
                using (MySqlCommand cmd = new MySqlCommand(string.Empty, con))
                {
                    using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                    {
                        cmd.CommandText = "SELECT COALESCE(MAX(jornada)+1,1) AS jornada FROM liga_partidos WHERE id_liga = ?id";
                        cmd.Parameters.AddWithValue("?id", idLiga);
                        
                        da.Fill(dt);

                        
                        return dt.Rows[0]["jornada"].ToString();
                    }
                }
            }



        }

    }
}