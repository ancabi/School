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

namespace school.Controllers
{
    [Authorize]
    public class EntrenamientosController : Controller
    {
        //
        // GET: /Entrenamientos/

        public ActionResult Entrenamientos()
        {
            return View();
        }

        [HttpPost]
        public JsonResult getEntrenamientos()
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
                        cmd.CommandText = "SELECT * FROM school.entrenamientos WHERE idusuario=?id OR idusuario = -1";
                        cmd.Parameters.AddWithValue("?id", Session["idusuario"]);
                        da.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            resp.cod = "OK";
                            resp.d.Add("entrenamientos", dt.ToList());
                        }
                        else
                        {
                            resp.cod = "KO";
                            resp.msg = "Ningún entrenamiento encontrado!";
                        }
                    }
                }
            }
            return Json(resp);
        }
        [HttpPost]
        public JsonResult addEntrenamiento(string nombre, string tipo, string descripcion, int duracion)
        {
            RespGeneric resp = new RespGeneric("KO");
            int numReg = 0;

            try
            {

                using (MySqlConnection con = new MySqlConnection(BD.CadConMySQL(BD.Server.BDLOCAL, BD.schema)))
                {
                    using (MySqlCommand cmd = new MySqlCommand(string.Empty, con))
                    {
                        using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                        {


                            cmd.CommandText =
                                "INSERT INTO school.entrenamientos (nombre, tipo,descripcion,duracion,idusuario) VALUES (?nombre,?tipo,?descripcion,?duracion,?id)";
                            cmd.Parameters.AddWithValue("?nombre", nombre);
                            cmd.Parameters.AddWithValue("?tipo", tipo);
                            cmd.Parameters.AddWithValue("?descripcion", descripcion);
                            cmd.Parameters.AddWithValue("?duracion", duracion);
                            cmd.Parameters.AddWithValue("?id", Session["idusuario"]);

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
        public JsonResult saveEntrenamiento(int id, string nombre, string tipo, string descripcion, int duracion)
        {
            RespGeneric resp = new RespGeneric("KO");
            int numReg = 0;
            try
            {
                using (MySqlConnection con = new MySqlConnection(BD.CadConMySQL(BD.Server.BDLOCAL, BD.schema)))
                {
                    using (MySqlCommand cmd = new MySqlCommand(string.Empty, con))
                    {
                        using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                        {


                            cmd.CommandText = "UPDATE school.entrenamientos SET nombre=?nombre,tipo=?tipo,descripcion=?descripcion,duracion=?duracion WHERE id = ?id";
                            cmd.Parameters.AddWithValue("?nombre", nombre);
                            cmd.Parameters.AddWithValue("?tipo", tipo);
                            cmd.Parameters.AddWithValue("?descripcion", descripcion);
                            cmd.Parameters.AddWithValue("?duracion", duracion);
                            cmd.Parameters.AddWithValue("?id", id);

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
        public JsonResult deleteEntrenamiento(int id)
        {
            RespGeneric resp = new RespGeneric("KO");
            int numReg = 0;
            try
            {
                using (MySqlConnection con = new MySqlConnection(BD.CadConMySQL(BD.Server.BDLOCAL, BD.schema)))
                {
                    using (MySqlCommand cmd = new MySqlCommand(string.Empty, con))
                    {
                        using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                        {


                            cmd.CommandText = "DELETE FROM school.entrenamientos WHERE id = ?id";

                            cmd.Parameters.AddWithValue("?id", id);

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

    }
}
