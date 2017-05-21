using school.Helpers;
using school.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace school.Controllers
{
    [Authorize]
    [SessionState(System.Web.SessionState.SessionStateBehavior.Disabled)]
    public class EventosController : Controller
    {
        //
        // GET: /Eventos/

        public ActionResult Eventos()
        {
            return View();
        }

        [HttpPost]
        public JsonResult getEventos()
        {
            RespGeneric resp = new RespGeneric("KO");
            DataTable dt = new DataTable();

            using (MySqlConnection con = new MySqlConnection(BD.CadConMySQL(BD.Server.BDLOCAL, BD.schema)))
            {
                using (MySqlCommand cmd = new MySqlCommand(string.Empty, con))
                {
                    using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                    {
                        StringBuilder sbQuery = new StringBuilder();
                        cmd.CommandText = "SELECT * FROM school.eventos order by fecha desc";
                        da.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            resp.cod = "OK";
                            resp.d.Add("eventos", dt.ToList());
                        }
                        else
                        {
                            resp.cod = "KO";
                            resp.msg = "Ningún evento encontrado!";
                        }
                    }
                }
            }
            return Json(resp);
        }
        [HttpPost]
        public JsonResult newEvento(Dictionary<object,string> evento)
        {
            RespGeneric resp = new RespGeneric("KO");
            int numReg = 0;
            DataTable dt = new DataTable();

            using (MySqlConnection con = new MySqlConnection(BD.CadConMySQL(BD.Server.BDLOCAL, BD.schema)))
            {
                using (MySqlCommand cmd = new MySqlCommand(string.Empty, con))
                {
                    using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                    {
                        con.Open();
                        StringBuilder sbQuery = new StringBuilder();
                        cmd.CommandText = "INSERT INTO school.eventos (titulo, descripcion, tipo, nivel, activo, fecha_evento, hora_desde, hora_hasta, fecha) VALUES (?titulo, ?descripcion, ?tipo, ?nivel, ?activo, ?fecha_evento, ?hora_desde, ?hora_hasta, NOW())";
                        cmd.Parameters.AddWithValue("?titulo", evento["titulo"]);
                        cmd.Parameters.AddWithValue("?descripcion", evento["descripcion"]);
                        cmd.Parameters.AddWithValue("?tipo", evento["tipo"]);
                        cmd.Parameters.AddWithValue("?nivel", evento["nivel"]);
                        cmd.Parameters.AddWithValue("?activo", 1);
                        cmd.Parameters.AddWithValue("?fecha_evento", evento["fecha_evento"]);
                        cmd.Parameters.AddWithValue("?hora_desde", evento["hora_desde"]);
                        cmd.Parameters.AddWithValue("?hora_hasta", evento["hora_hasta"]);
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
        public JsonResult viewEvento(int id)
        {
            RespGeneric resp = new RespGeneric("KO");
            DataTable dt = new DataTable();

            using (MySqlConnection con = new MySqlConnection(BD.CadConMySQL(BD.Server.BDLOCAL, BD.schema)))
            {
                using (MySqlCommand cmd = new MySqlCommand(string.Empty, con))
                {
                    using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                    {
                        StringBuilder sbQuery = new StringBuilder();
                        cmd.CommandText = "SELECT * FROM school.eventos where id=?id";
                        cmd.Parameters.AddWithValue("?id", id);
                        da.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            resp.cod = "OK";
                            resp.d.Add("evento", dt.ToList());
                        }
                        else
                        {
                            resp.cod = "KO";
                            resp.msg = "Ningún evento encontrado!";
                        }
                    }
                }
            }
            return Json(resp);
        }
        [HttpPost]
        public JsonResult editEvento(Dictionary<object, string> evento)
        {
            RespGeneric resp = new RespGeneric("KO");
            int numReg = 0;
            DataTable dt = new DataTable();

            using (MySqlConnection con = new MySqlConnection(BD.CadConMySQL(BD.Server.BDLOCAL, BD.schema)))
            {
                using (MySqlCommand cmd = new MySqlCommand(string.Empty, con))
                {
                    using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                    {
                        con.Open();
                        StringBuilder sbQuery = new StringBuilder();
                        cmd.CommandText = "UPDATE school.eventos SET titulo = ?titulo, descripcion = ?descripcion, tipo = ?tipo, nivel = ?nivel, fecha_evento = ?fecha_evento, hora_desde = ?hora_desde, hora_hasta = ?hora_hasta WHERE id = ?id;";
                        cmd.Parameters.AddWithValue("?titulo", evento["titulo"]);
                        cmd.Parameters.AddWithValue("?descripcion", evento["descripcion"]);
                        cmd.Parameters.AddWithValue("?tipo", evento["tipo"]);
                        cmd.Parameters.AddWithValue("?nivel", evento["nivel"]);
                        cmd.Parameters.AddWithValue("?activo", 1);
                        cmd.Parameters.AddWithValue("?fecha_evento", evento["fecha_evento"]);
                        cmd.Parameters.AddWithValue("?hora_desde", evento["hora_desde"]);
                        cmd.Parameters.AddWithValue("?hora_hasta", evento["hora_hasta"]);
                        cmd.Parameters.AddWithValue("?id", evento["id"]);
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
        public JsonResult deleteEvento(int id)
        {
            RespGeneric resp = new RespGeneric("KO");
            int numReg = 0;
            DataTable dt = new DataTable();

            using (MySqlConnection con = new MySqlConnection(BD.CadConMySQL(BD.Server.BDLOCAL, BD.schema)))
            {
                using (MySqlCommand cmd = new MySqlCommand(string.Empty, con))
                {
                    using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                    {
                        con.Open();
                        StringBuilder sbQuery = new StringBuilder();
                        cmd.CommandText = "DELETE FROM school.eventos where id = ?id";
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
        [HttpPost]
        public JsonResult changeActive(int id)
        {
            RespGeneric resp = new RespGeneric("KO");
            int numReg = 0;
            DataTable dt = new DataTable();

            using (MySqlConnection con = new MySqlConnection(BD.CadConMySQL(BD.Server.BDLOCAL, BD.schema)))
            {
                using (MySqlCommand cmd = new MySqlCommand(string.Empty, con))
                {
                    using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                    {
                        con.Open();
                        StringBuilder sbQuery = new StringBuilder();
                        cmd.CommandText = "UPDATE school.eventos SET activo = !activo WHERE id = ?id";
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
