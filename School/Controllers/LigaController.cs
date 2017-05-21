using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using MySql.Data.MySqlClient;
using school.Controllers;
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

            using (MySqlConnection con = new MySqlConnection(BD.CadConMySQL(BD.Server.BDLOCAL, BD.schema)))
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

        public JsonResult getLigaEquipos()
        {

            RespGeneric resp = new RespGeneric("KO");
            DataTable dt = new DataTable();

            using (MySqlConnection con = new MySqlConnection(BD.CadConMySQL(BD.Server.BDLOCAL, BD.schema)))
            {
                using (MySqlCommand cmd = new MySqlCommand(string.Empty, con))
                {
                    using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                    {
                        cmd.CommandText = "SELECT * FROM school.liga_equipos where id_liga=?id";
                        cmd.Parameters.AddWithValue("?id", MainController.getIdLigaEquipoSelected());
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

        public List<Dictionary<string,object>> getListLigaEquipos()
        {


            DataTable dt = new DataTable();

            using (MySqlConnection con = new MySqlConnection(BD.CadConMySQL(BD.Server.BDLOCAL, BD.schema)))
            {
                using (MySqlCommand cmd = new MySqlCommand(string.Empty, con))
                {
                    using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                    {
                        cmd.CommandText = "SELECT * FROM school.liga_equipos where id_liga=?id";
                        cmd.Parameters.AddWithValue("?id", MainController.getIdLigaEquipoSelected());
                        da.Fill(dt);

                        return dt.ToList();

                    }
                }
            }

        }

        public JsonResult getJornadas(int idLiga)
        {

            RespGeneric resp = new RespGeneric("KO");
            DataTable dt = new DataTable();

            using (MySqlConnection con = new MySqlConnection(BD.CadConMySQL(BD.Server.BDLOCAL, BD.schema)))
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

            using (MySqlConnection con = new MySqlConnection(BD.CadConMySQL(BD.Server.BDLOCAL, BD.schema)))
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

        public JsonResult getLigaClasificacion()
        {

            RespGeneric resp = new RespGeneric("KO");
            DataTable dt = new DataTable();

            using (MySqlConnection con = new MySqlConnection(BD.CadConMySQL(BD.Server.BDLOCAL, BD.schema)))
            {
                using (MySqlCommand cmd = new MySqlCommand(string.Empty, con))
                {
                    using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                    {
                        cmd.CommandText = "SELECT c.*, e.nombre FROM liga_clasificacion c INNER JOIN liga_equipos e ON e.id=c.id_equipo WHERE c.id_liga=?id ORDER BY puntos DESC";
                        cmd.Parameters.AddWithValue("?id", MainController.getIdLigaEquipoSelected());
                        
                        da.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            resp.cod = "OK";
                            resp.d.Add("clasificacion", dt.ToList());
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

            using (MySqlConnection con = new MySqlConnection(BD.CadConMySQL(BD.Server.BDLOCAL, BD.schema)))
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

        [HttpPost]
        public JsonResult saveResultados(List<Dictionary<string,object>> jornada, bool edit)
        {
            RespGeneric resp = new RespGeneric("KO");

            long rows = 0;

            using (MySqlConnection con = new MySqlConnection(BD.CadConMySQL(BD.Server.BDLOCAL, BD.schema)))
            {
                using (MySqlCommand cmd = new MySqlCommand(string.Empty, con))
                {
                        
                    cmd.CommandText = "UPDATE liga_partidos SET resultado_local=?local, resultado_visitante=?visitante WHERE id=?id";

                    foreach (Dictionary<string, object> j in jornada)
                    {
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("?local", j["resultado_local"]);
                        cmd.Parameters.AddWithValue("?visitante", j["resultado_visitante"]);
                        cmd.Parameters.AddWithValue("?id", j["id"]);

                        con.Open();
                        rows+=cmd.ExecuteNonQuery();
                        con.Close();

                        if (!edit)
                        {
                            refreshClasificacion(j);
                        }

                    }

                    if (edit)
                    {
                        recalcularClasificacion();
                    }

                        if (rows == jornada.Count)
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

        private void recalcularClasificacion()
        {
            List<Dictionary<String, object>> jornadas = getJornadasPrivate();

            inicializarLiga();

            foreach(Dictionary<string,object> j in jornadas)
            {
                refreshClasificacion(j);
            }
        }

        private void refreshClasificacion(Dictionary<string, object> j)
        {
            using (MySqlConnection con = new MySqlConnection(BD.CadConMySQL(BD.Server.BDLOCAL, BD.schema)))
            {
                using (MySqlCommand cmd = new MySqlCommand(string.Empty, con))
                {
                    int gLocal = Int32.Parse(j["resultado_local"].ToString());
                    int gVisitante = Int32.Parse(j["resultado_visitante"].ToString());
                    string resultadoLocal = "";
                    string resultadoVisitante = "";
                    int puntosLocal = 0;
                    int puntosVisitante = 0;
                    if (gLocal > gVisitante)
                    {
                        resultadoLocal = "ganados=ganados+1";
                        resultadoVisitante = "perdidos=perdidos+1";
                        puntosLocal = 3;
                        puntosVisitante = 0;
                    }else if (gLocal < gVisitante)
                    {
                        resultadoLocal = "perdidos=perdidos+1";
                        resultadoVisitante = "ganados=ganados+1";
                        puntosLocal = 0;
                        puntosVisitante = 3;
                    }
                    else
                    {
                        resultadoLocal = "empatados=empatados+1";
                        resultadoVisitante = "empatados=empatados+1";
                        puntosLocal = 1;
                        puntosVisitante = 1;
                    }

                    cmd.CommandText = String.Format("UPDATE liga_clasificacion SET puntos=puntos+?puntos, partidos=partidos+1, goles_favor=goles_favor+?gFavor, goles_contra=goles_contra+?gContra, {0} WHERE id_liga=?idLiga AND id_equipo=?idEquipo",resultadoLocal);

                        cmd.Parameters.AddWithValue("?puntos", puntosLocal);
                        cmd.Parameters.AddWithValue("?gFavor", j["resultado_local"]);
                        cmd.Parameters.AddWithValue("?gContra", j["resultado_visitante"]);
                        cmd.Parameters.AddWithValue("?idLiga", j["id_liga"]);
                        cmd.Parameters.AddWithValue("?idEquipo", j["id_local"]);

                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();

                    
                    cmd.Parameters.Clear();

                    cmd.CommandText = String.Format("UPDATE liga_clasificacion SET puntos=puntos+?puntos, partidos=partidos+1, goles_favor=goles_favor+?gFavor, goles_contra=goles_contra+?gContra, {0} WHERE id_liga=?idLiga AND id_equipo=?idEquipo", resultadoVisitante);

                        cmd.Parameters.AddWithValue("?puntos", puntosVisitante);
                        cmd.Parameters.AddWithValue("?gFavor", j["resultado_visitante"]);
                        cmd.Parameters.AddWithValue("?gContra", j["resultado_local"]);
                        cmd.Parameters.AddWithValue("?idLiga", j["id_liga"]);
                        cmd.Parameters.AddWithValue("?idEquipo", j["id_visitante"]);

                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();

                    }
                    
                }
            
        }

        private List<Dictionary<String,object>> getJornadasPrivate()
        {
            DataTable dt = new DataTable();
            using (MySqlConnection con = new MySqlConnection(BD.CadConMySQL(BD.Server.BDLOCAL, BD.schema)))
            {
                using (MySqlCommand cmd = new MySqlCommand(string.Empty, con))
                {
                    using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                    {
                        cmd.CommandText = "SELECT p.*, l.nombre AS 'local', v.nombre AS visitante FROM school.liga_partidos p INNER JOIN liga_equipos l ON l.id = p.id_local INNER JOIN liga_equipos v ON v.id = p.id_visitante where p.id_liga=?id";
                        cmd.Parameters.AddWithValue("?id", MainController.getIdLigaEquipoSelected());
                        da.Fill(dt);
                       return dt.ToList();
                    }
                }
            }
        }

        private void inicializarLiga()
        {
            List<Dictionary<string,object>> equipos = getListLigaEquipos();

            using (MySqlConnection con = new MySqlConnection(BD.CadConMySQL(BD.Server.BDLOCAL, BD.schema)))
            {
                using (MySqlCommand cmd = new MySqlCommand(string.Empty, con))
                {
                    using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                    {
                        cmd.CommandText = "INSERT INTO liga_clasificacion (id_liga,id_equipo,puntos,partidos,goles_favor,goles_contra,ganados,empatados,perdidos) VALUES (?liga,?equipo,0,0,0,0,0,0,0) ON DUPLICATE KEY UPDATE puntos=0,partidos=0,goles_favor=0,goles_contra=0,ganados=0,empatados=0,perdidos=0; ";
                        foreach(Dictionary<string,object> e in equipos)
                        {
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("?liga", e["id_liga"]);
                            cmd.Parameters.AddWithValue("?equipo", e["id"]);
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                        

                    }
                }
            }
        }

        public string getSiguienteJornada(int idLiga)
        {

            RespGeneric resp = new RespGeneric("KO");
            DataTable dt = new DataTable();

            using (MySqlConnection con = new MySqlConnection(BD.CadConMySQL(BD.Server.BDLOCAL, BD.schema)))
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