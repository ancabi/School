using school.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Newtonsoft.Json;
using school.Helpers;
using MySql.Data.MySqlClient;
using System.Text;
using Microsoft.Ajax.Utilities;

namespace school.Controllers
{
    [Authorize]
    public class MainController : Controller
    {
        private static JsonResult equipos = null;
        private static Dictionary<string, object> equipoSelected = null;

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult getEquipos()
        {
            if (equipos == null && Session.Count != 0)
            {
                RespGeneric resp = new RespGeneric("KO");
                DataTable dt = new DataTable();

                using (MySqlConnection con = new MySqlConnection(BD.CadConMySQL(BD.Server.BDLOCAL, BD.schema)))
                {
                    using (MySqlCommand cmd = new MySqlCommand(string.Empty, con))
                    {
                        using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                        {
                            cmd.CommandText = "SELECT * FROM school.liga_equipos where id_monitor=?id";
                            cmd.Parameters.AddWithValue("?id", Session["idusuario"]);
                            da.Fill(dt);
                            if (dt.Rows.Count > 0)
                            {
                                resp.cod = "OK";
                                resp.d.Add("equipos", dt.ToList());
                            }
                            else
                            {
                                resp.cod = "KO";
                            }
                        }
                    }
                }
                equipos = Json(resp);
            }
            return equipos;

        }

        [HttpPost]
        public void setEquipoSelected(Dictionary<string, object> e)
        {
            equipoSelected = e;

            using (MySqlConnection con = new MySqlConnection(BD.CadConMySQL(BD.Server.BDLOCAL, BD.schema)))
            {
                using (MySqlCommand cmd = new MySqlCommand(string.Empty, con))
                {
                    cmd.CommandText = "UPDATE school.usuarios SET idultimo_equipo=?id WHERE id=?id";
                    cmd.Parameters.AddWithValue("?id", Session["idusuario"]);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }

        [HttpPost]
        public JsonResult getEquipoSelected()
        {
            if (equipoSelected == null)
            {
                DataTable dt = new DataTable();
                using (MySqlConnection con = new MySqlConnection(BD.CadConMySQL(BD.Server.BDLOCAL, BD.schema)))
                {
                    using (MySqlCommand cmd = new MySqlCommand(string.Empty, con))
                    {
                        using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                        {
                            cmd.CommandText = "SELECT * FROM school.liga_equipos where id=?id";
                            cmd.Parameters.AddWithValue("?id", Session["idultimo_equipo"]);
                            da.Fill(dt);

                            equipoSelected = dt.ToList()[0];

                        }
                    }
                }
            }
            return Json(equipoSelected);
        }

        public static void clearEquipo()
        {

            equipoSelected = null;

            equipos = null;

        }

        public static int getIdEquipoSelected()
        {
            return Int32.Parse(equipoSelected["id"].ToString());
        }


        public static int getIdLigaEquipoSelected()
        {
            return Int32.Parse(equipoSelected["id_liga"].ToString());
        }




    }
}
