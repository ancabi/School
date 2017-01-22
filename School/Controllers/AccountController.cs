using school.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace school.Controllers
{
    public class AccountController : Controller
    {
        //
        // GET: /Index/

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            return RedirectToAction("Login");
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult LoginUser(string usuario, string pass)
        {

            RespGeneric resp = new RespGeneric("OK");
            DataTable dt = new DataTable();

            using (MySqlConnection con = new MySqlConnection(BD.CadConMySQL(BD.Server.BDLOCAL, "school")))
            {
                using (MySqlCommand cmd = new MySqlCommand(string.Empty, con))
                {
                    using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                    {
                        cmd.CommandText = "SELECT * FROM school.usuarios WHERE usuario=?usuario;";
                        cmd.Parameters.AddWithValue("?usuario", usuario);
                        da.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            DataRow rowUsuario = dt.Rows[0];

                            Session["idusuario"] = rowUsuario["id"];
                            Session["usuario"] = rowUsuario["usuario"];
                            Session["tipo"] = rowUsuario["tipo"].ToString().Trim();
                            Session["cif"] = rowUsuario["cif"];
                            Session["nombre"] = rowUsuario["nombre"].ToString();
                            Session["email"] = rowUsuario["email"].ToString();
                            FormsAuthentication.SetAuthCookie(usuario, false);
                            resp.cod = "OK";
                            resp.d.Add("idusuario", Session["idusuario"].ToString());
                            resp.d.Add("usuario", Session["usuario"].ToString());
                            resp.d.Add("url", FormsAuthentication.GetRedirectUrl(usuario, false));
                            resp.d.Add("tipo", Session["tipo"].ToString());
                            resp.d.Add("cif", Session["cif"].ToString());
                            resp.d.Add("nombre", Session["nombre"].ToString());
                            Session["limitedVersion"] = false;

                        }
                        else
                        {
                            resp.cod = "KO";
                            resp.msg = "Código de artículo no encontrado en la base de datos!";
                        }
                    }
                }
            }

            return Json(resp);
        }

        //private bool insertAcceso(string usuario, string ip, string status, string msg)
        //{
        //    int numReg = 0;

        //    using (MySqlConnection con = new MySqlConnection(BD.CadConMySQL(BD.Server.BDLOCAL, "school")))
        //    {
        //        using (MySqlCommand cmd = new MySqlCommand("INSERT INTO school.accesos (usuario,ip,fecha,status,status_msg) VALUES (@usuario,@ip,NOW(),@status,@status_msg)", con))
        //        {

        //            cmd.Parameters.AddWithValue("@usuario", usuario);
        //            cmd.Parameters.AddWithValue("@ip", ip);
        //            cmd.Parameters.AddWithValue("@status", status);
        //            cmd.Parameters.AddWithValue("@status_msg", msg);
        //            con.Open();
        //            numReg = cmd.ExecuteNonQuery();
        //            cmd.CommandText = "UPDATE _aspnetdb.my_aspnet_sessions SET usuario=?usuario,ip=?ip WHERE SessionId=?session_id;";
        //            cmd.Parameters.Clear();
        //            cmd.Parameters.AddWithValue("?usuario", usuario);
        //            cmd.Parameters.AddWithValue("?session_id", Session.SessionID);
        //            cmd.Parameters.AddWithValue("?ip", ip);
        //            cmd.ExecuteNonQuery();
        //            con.Close();
        //        }
        //    }
        //    return (numReg > 0);
        //}

        //private DataTable getUsuario(string usuario)
        //{
        //    DataTable dt = new DataTable();

        //    using (SqlConnection con = new SqlConnection(BD.CadConschool()))
        //    {
        //        using (SqlCommand cmd = new SqlCommand("SELECT c.codigo as usuario,cif,nombre,direccion,codpost,poblacion,provincia,m.valor as tipo,m1.valor as clave,c.bloq_cli as baja,m2.valor as ip,a.actividad as tipo_asociado,c.email FROM clientes c INNER JOIN multicam m ON c.codigo=m.codigo AND m.fichero='CLIENTES' AND m.campo='AYT' INNER JOIN multicam m1 ON c.codigo=m1.codigo AND m1.fichero='CLIENTES' AND m1.campo='AYP' LEFT JOIN multicam m2 ON c.codigo=m2.codigo AND m2.fichero='CLIENTES' AND m2.campo='IP1' LEFT JOIN acti_cli a ON c.codigo=a.cliente WHERE c.codigo=@usuario;", con))
        //        {
        //            cmd.Parameters.AddWithValue("@usuario", usuario);
        //            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
        //            {
        //                da.Fill(dt);
        //            }
        //        }
        //    }
        //    return dt;
        //}
    }
}
