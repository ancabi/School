using school.Helpers;
using school.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace school.Controllers
{
    public class PartidosController : Controller
    {
        //
        // GET: /Partidos/

        public ActionResult Partidos()
        {
            return View();
        }
        [HttpPost]
        public JsonResult getPartidos()
        {
            RespGeneric resp = new RespGeneric("KO");
            DataTable dt = new DataTable();

            using (MySqlConnection con = new MySqlConnection(BD.CadConMySQL(BD.Server.BDLOCAL, "school")))
            {
                using (MySqlCommand cmd = new MySqlCommand(string.Empty, con))
                {
                    using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                    {
                        StringBuilder sbQuery = new StringBuilder();
                        cmd.CommandText = "SELECT * FROM school.partidos";
                        da.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            resp.cod = "OK";
                            resp.d.Add("partidos", dt.ToList());
                        }
                        else
                        {
                            resp.cod = "KO";
                            resp.msg = "Ningún partido encontrado!";
                        }
                    }
                }
            }
            return Json(resp);
        }
        [HttpPost]
        public JsonResult newPartido(Dictionary<object, string> partido)
        {
            RespGeneric resp = new RespGeneric("KO");
            int numReg = 0;
            DataTable dt = new DataTable();

            using (MySqlConnection con = new MySqlConnection(BD.CadConMySQL(BD.Server.BDLOCAL, "school")))
            {
                using (MySqlCommand cmd = new MySqlCommand(string.Empty, con))
                {
                    using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                    {
                        con.Open();
                        StringBuilder sbQuery = new StringBuilder();
                        cmd.CommandText = "INSERT partidos (id_equipo, contrario, liga, fecha, hora) VALUES (?id_equipo, ?contrario, ?liga, ?fecha, ?hora)";
                        cmd.Parameters.AddWithValue("?id_equipo", 1);
                        cmd.Parameters.AddWithValue("?contrario", partido["descripcion"]);
                        cmd.Parameters.AddWithValue("?liga", partido["tipo"]);
                        cmd.Parameters.AddWithValue("?fecha", partido["nivel"]);
                        cmd.Parameters.AddWithValue("?hora", partido["nivel"]);
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
            }
            return Json(resp);
        }
        [HttpPost]
        public JsonResult viewPartido(int id)
        {
            RespGeneric resp = new RespGeneric("KO");
            DataTable dt = new DataTable();

            using (MySqlConnection con = new MySqlConnection(BD.CadConMySQL(BD.Server.BDLOCAL, "school")))
            {
                using (MySqlCommand cmd = new MySqlCommand(string.Empty, con))
                {
                    using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                    {
                        StringBuilder sbQuery = new StringBuilder();
                        cmd.CommandText = "SELECT * FROM school.partidos where id=?id";
                        cmd.Parameters.AddWithValue("?id", id);
                        da.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            resp.cod = "OK";
                            resp.d.Add("partido", dt.ToList());
                        }
                        else
                        {
                            resp.cod = "KO";
                            resp.msg = "Ningún partido encontrado!";
                        }
                    }
                }
            }
            return Json(resp);
        }
        [HttpPost]
        public JsonResult editPartido(Dictionary<object, string> partido)
        {
            RespGeneric resp = new RespGeneric("KO");
            int numReg = 0;
            DataTable dt = new DataTable();

            using (MySqlConnection con = new MySqlConnection(BD.CadConMySQL(BD.Server.BDLOCAL, "school")))
            {
                using (MySqlCommand cmd = new MySqlCommand(string.Empty, con))
                {
                    using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                    {
                        con.Open();
                        StringBuilder sbQuery = new StringBuilder();
                        cmd.CommandText = "UPDATE school.eventos SET titulo = ?titulo, descripcion = ?descripcion, tipo = ?tipo, nivel = ?nivel, fecha_evento = ?fecha_evento, hora_desde = ?hora_desde, hora_hasta = ?hora_hasta WHERE id = ?id;";
                        cmd.Parameters.AddWithValue("?titulo", partido["titulo"]);
                        cmd.Parameters.AddWithValue("?descripcion", partido["descripcion"]);
                        cmd.Parameters.AddWithValue("?tipo", partido["tipo"]);
                        cmd.Parameters.AddWithValue("?nivel", partido["nivel"]);
                        cmd.Parameters.AddWithValue("?activo", 1);
                        cmd.Parameters.AddWithValue("?fecha_evento", partido["fecha_evento"]);
                        cmd.Parameters.AddWithValue("?hora_desde", partido["hora_desde"]);
                        cmd.Parameters.AddWithValue("?hora_hasta", partido["hora_hasta"]);
                        cmd.Parameters.AddWithValue("?id", partido["id"]);
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
            }
            return Json(resp);
        }
        [HttpPost]
        public JsonResult deletePartido(int id)
        {
            RespGeneric resp = new RespGeneric("KO");
            int numReg = 0;
            DataTable dt = new DataTable();

            using (MySqlConnection con = new MySqlConnection(BD.CadConMySQL(BD.Server.BDLOCAL, "school")))
            {
                using (MySqlCommand cmd = new MySqlCommand(string.Empty, con))
                {
                    using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                    {
                        con.Open();
                        StringBuilder sbQuery = new StringBuilder();
                        cmd.CommandText = "DELETE FROM school.partidos where id = ?id";
                        cmd.Parameters.AddWithValue("?id", id);
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
            }
            return Json(resp);
        }

    }
}
