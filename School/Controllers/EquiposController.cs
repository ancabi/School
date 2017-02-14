using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MySql.Data.MySqlClient;
using school.Helpers;
using school.Models;

namespace school.Controllers
{
    [Authorize]
    [SessionState(System.Web.SessionState.SessionStateBehavior.Disabled)]
    public class EquiposController : Controller
    {
        //
        // GET: /Equipos/

        public ActionResult Equipos()
        {
            return View();
        }

        [HttpPost]
        public JsonResult getJugadores(int idEquipo)
        {

                RespGeneric resp = new RespGeneric("KO");
                DataTable dt = new DataTable();

                using (MySqlConnection con = new MySqlConnection(BD.CadConMySQL(BD.Server.BDLOCAL, "school")))
                {
                    using (MySqlCommand cmd = new MySqlCommand(string.Empty, con))
                    {
                        using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                        {
                            cmd.CommandText = "SELECT * FROM school.equipos_jugadores j INNER JOIN usuarios u ON u.id=j.idusuario where id_equipo=?id";
                            cmd.Parameters.AddWithValue("?id", idEquipo);
                            da.Fill(dt);
                            if (dt.Rows.Count > 0)
                            {
                                resp.cod = "OK";
                                resp.d.Add("jugadores", dt.ToList());
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
        public JsonResult addEntrenamiento(int idEntrenamiento, DateTime fecha)
        {
            RespGeneric resp = new RespGeneric("KO");
            int numReg = 0;

            try
            {

                using (MySqlConnection con = new MySqlConnection(BD.CadConMySQL(BD.Server.BDLOCAL, "school")))
                {
                    using (MySqlCommand cmd = new MySqlCommand(string.Empty, con))
                    {
                        using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                        {


                            cmd.CommandText =
                                "INSERT INTO school.entrenamiento_equipo (idequipo, identrenamiento,fecha) VALUES (?idequipo,?identrenamiento,?fecha)";
                            cmd.Parameters.AddWithValue("?idequipo", MainController.getIdEquipoSelected());
                            cmd.Parameters.AddWithValue("?identrenamiento", idEntrenamiento);
                            cmd.Parameters.AddWithValue("?fecha", fecha);

                            con.Open();
                            numReg = (int)cmd.ExecuteNonQuery();
                            con.Close();
                            if (numReg == 1)
                            {
                                resp.cod = "OK";
                            }
                            else
                            {
                                resp.cod = "KO";
                            }

                        }
                    }
                }
            }
            catch (Exception e)
            {
                resp.msg = e.Message;
            }
            return Json(resp);
        }

        [HttpPost]
        public JsonResult getNextEntrenamiento()
        {
            RespGeneric resp = new RespGeneric("KO");
            int numReg = 0;
            DataTable dt = new DataTable();
            try
            {

                using (MySqlConnection con = new MySqlConnection(BD.CadConMySQL(BD.Server.BDLOCAL, "school")))
                {
                    using (MySqlCommand cmd = new MySqlCommand(string.Empty, con))
                    {
                        using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                        {
                            cmd.CommandText = "SELECT * FROM school.entrenamiento_equipo eq INNER JOIN entrenamientos en ON en.id =eq.identrenamiento where idequipo=?idequipo ORDER BY fecha LIMIT 1 ";
                            cmd.Parameters.AddWithValue("?idequipo", MainController.getIdEquipoSelected());
                            da.Fill(dt);
                            if (dt.Rows.Count > 0)
                            {
                                resp.cod = "OK";
                                resp.d.Add("jugadores", dt.ToList()[0]);
                            }
                            else
                            {
                                resp.cod = "KO";
                            }

                        }
                    }
                }
            }
            catch (Exception e)
            {
                resp.msg = e.Message;
            }
            return Json(resp);
        }

        public JsonResult getNextJornada()
        {

            RespGeneric resp = new RespGeneric("KO");
            DataTable dt = new DataTable();

            using (MySqlConnection con = new MySqlConnection(BD.CadConMySQL(BD.Server.BDLOCAL, "school")))
            {
                using (MySqlCommand cmd = new MySqlCommand(string.Empty, con))
                {
                    using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                    {
                        cmd.CommandText = "SELECT p.*, l.nombre AS 'local', v.nombre AS visitante FROM school.liga_partidos p INNER JOIN liga_equipos l ON l.id = p.id_local INNER JOIN liga_equipos v ON v.id = p.id_visitante where p.id_liga=?id AND resultado_local IS NULL";
                        cmd.Parameters.AddWithValue("?id", MainController.getIdLigaEquipoSelected());
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

    }
}
