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
                            cmd.CommandText = "SELECT * FROM school.equipos_jugadores where id_equipo=?id";
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

    }
}
