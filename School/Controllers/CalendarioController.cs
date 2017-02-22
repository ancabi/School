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

namespace school.Controllers
{
    [Authorize]
    [SessionState(System.Web.SessionState.SessionStateBehavior.Disabled)]
    public class CalendarioController : Controller
    {
        //
        // GET: /Calendario/

        public ActionResult Calendario()
        {
            return View();
        }

        [HttpPost]
        public JsonResult getCalendario()
        {
            RespGeneric resp = new RespGeneric("KO");
            DataTable dt = new DataTable();

            using (MySqlConnection con = new MySqlConnection(BD.CadConMySQL(BD.Server.BDLOCAL, "school")))
            {
                using (MySqlCommand cmd = new MySqlCommand(string.Empty, con))
                {
                    using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                    {
                        cmd.CommandText = "SELECT titulo, descripcion, fecha FROM school.comunicados";
                        da.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            resp.cod = "OK";
                            resp.d.Add("calendario", dt.ToList());
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
