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

namespace school.Controllers
{
    [Authorize]
    public class MainController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AsociadosOnly()
        {
            return View();
        }

        [HttpPost]
        public JsonResult getEquipos()
        {
            RespGeneric resp = new RespGeneric("KO");
            DataTable dt = new DataTable();

            using (MySqlConnection con = new MySqlConnection(BD.CadConMySQL(BD.Server.BDLOCAL, "school")))
            {
                using (MySqlCommand cmd = new MySqlCommand(string.Empty, con))
                {
                    using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                    {
                        cmd.CommandText = "SELECT * FROM school.equipos where id_monitor=?id";
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
            return Json(resp);
        }



    }
}
