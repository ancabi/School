using school.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Security;
using school.Helpers;

namespace school.Controllers
{
    public class AccountController : Controller
    {
        //
        // GET: /Index/
        private readonly long packMatricula = 25;
        private readonly long packMatriculaGratis = 46;
        private readonly long packInscripcionExtraBaby = 31;
        private readonly long packInscripcionExtraPreBenjamin = 30;
        private readonly long packInscripcionExtraCadete = 29;
        private readonly long packBasket = 28;
        private readonly long packFutbol = 23;

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            MainController.clearEquipo();
            Session.Clear();
            return RedirectToAction("Login");
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult LoginUser(string usuario, string pass)
        {

            RespGeneric resp = new RespGeneric("OK");
            DataTable dt = new DataTable();

            using (MySqlConnection con = new MySqlConnection(BD.CadConMySQL(BD.Server.BDLOCAL, BD.schema)))
            {
                using (MySqlCommand cmd = new MySqlCommand(string.Empty, con))
                {
                    using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                    {
                        //cmd.CommandText = "SELECT * FROM school.usuarios WHERE usuario=?usuario;";
                        //cmd.Parameters.AddWithValue("?usuario", usuario);
                        //da.Fill(dt);
                        //if (dt.Rows.Count > 0)
                        //{
                        //    DataRow rowUsuario = dt.Rows[0];

                        //    Session["idusuario"] = rowUsuario["id"];
                        //    Session["usuario"] = rowUsuario["usuario"];
                        //    Session["tipo"] = rowUsuario["tipo"].ToString().Trim();
                        //    Session["cif"] = rowUsuario["cif"];
                        //    Session["nombre"] = rowUsuario["nombre"].ToString();
                        //    Session["email"] = rowUsuario["email"].ToString();
                        //    Session["idultimo_equipo"] = rowUsuario["idultimo_equipo"].ToString();
                        //    FormsAuthentication.SetAuthCookie(usuario, false);
                        //    resp.cod = "OK";
                        //    resp.d.Add("idusuario", Session["idusuario"].ToString());
                        //    resp.d.Add("usuario", Session["usuario"].ToString());
                        //    resp.d.Add("url", FormsAuthentication.GetRedirectUrl(usuario, false));
                        //    resp.d.Add("tipo", Session["tipo"].ToString());
                        //    resp.d.Add("cif", Session["cif"].ToString());
                        //    resp.d.Add("nombre", Session["nombre"].ToString());
                        //    Session["limitedVersion"] = false;

                        //}
                        //else
                        //{
                            resp.cod = "KO";
                            resp.msg = "Código de artículo no encontrado en la base de datos!";
                        //}
                    }
                }
            }

            return Json(resp);
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult Registrar(string dni, string nombre, string apellidos,string email,string telefono, string telefonoAlt,string user, 
            string pass,bool autorizacion,List<Dictionary<String,object>> hijos)
        {
            Task.Run(async () => { Extensiones.sendTelegram("registro: " + dni+" "+nombre+" "+apellidos+" "+email+" "+telefono+" "+telefonoAlt+" "+user+" "+pass+" "+hijos.ToString()); }).Wait();
            RespGeneric resp = new RespGeneric("OK");
            try
            {
            
            string password = BD.HashPassword(pass,BD.CreateSalt(8));

            Dictionary<String, object> res=Prestashop.createUser(email, pass, nombre, apellidos);

            using (MySqlConnection con = new MySqlConnection(BD.CadConMySQL(BD.Server.BDLOCAL, BD.schema)))
            {
                using (MySqlCommand cmd = new MySqlCommand(string.Empty, con))
                {
                    
                        cmd.CommandText = "INSERT INTO atabal.usuarios (dni,nombre,apellidos,email,telefono,telefonoalt,usuario,pass,autorizacion,activo,pagado,idasociado,tipo,idultimo_equipo,fecha_registro,idtienda,secure_key)" +
                                          " VALUES (?dni,?nombre,?apellidos,?email,?telefono,?telefonoAlt,?usuario,?pass,?autorizacion,0,0,-1,3,-1,?fecha_registro,?idtienda,?secure_key);";
                        cmd.Parameters.AddWithValue("?dni", dni);
                        cmd.Parameters.AddWithValue("?nombre", nombre);
                        cmd.Parameters.AddWithValue("?apellidos", apellidos);
                        cmd.Parameters.AddWithValue("?telefono", telefono);
                        cmd.Parameters.AddWithValue("?telefonoAlt", telefonoAlt);
                        cmd.Parameters.AddWithValue("?email", email);
                        cmd.Parameters.AddWithValue("?usuario", user);
                        cmd.Parameters.AddWithValue("?pass", password);
                        cmd.Parameters.AddWithValue("?autorizacion", autorizacion);
                        cmd.Parameters.AddWithValue("?fecha_registro", DateTime.Now);
                        cmd.Parameters.AddWithValue("?idtienda", res["id"]);
                        cmd.Parameters.AddWithValue("?secure_key", res["secure_key"]);
                        con.Open();
                    long inserted = cmd.ExecuteNonQuery();
                    long id = cmd.LastInsertedId;
                    con.Close();

                    List<long> productos = new List<long>();

                    foreach (Dictionary<string, object> hijo in hijos)
                    {
                        long idHijo = registrarHijo(hijo, id);
                        int anio = Int32.Parse(hijo["nacimiento"].ToString().Substring(6));
                        int mes = Int32.Parse(hijo["nacimiento"].ToString().Substring(3, 2));
                        int dia = Int32.Parse(hijo["nacimiento"].ToString().Substring(0, 2));

                        DateTime fechaNacimiento = new DateTime(anio, mes, dia);

                        int edad = (int) (((DateTime.Now - fechaNacimiento).Days) / 365.25);
                        bool extraescolar = false;
                        bool pack = false;

                        if (hijo.ContainsKey("extraescolares"))
                        {
                            extraescolar = (bool)hijo["extraescolares"];
                        }

                        if (hijo.ContainsKey("pack"))
                        {
                            pack = (int)hijo["pack"]==1;
                        }
                        

                        if (edad == 4 || edad == 5)
                        {
                            productos.Add(packInscripcionExtraBaby);
                        }
                        else if (edad == 6 || edad == 7)
                        {

                            productos.Add(packInscripcionExtraPreBenjamin);
                            
                        }
                        else
                        {
                            
                            productos.Add(packInscripcionExtraCadete);
                            
                        }

                        if (!extraescolar)
                        {
                            productos.Add(packMatricula);
                        }
                        else
                        {
                            productos.Add(packMatriculaGratis);
                        }

                        if (pack)
                        {
                            if (hijo.ContainsKey("sports[1]") )
                            {
                                productos.Add(packFutbol);
                            }
                            else
                            {
                                    if (hijo.ContainsKey("sports[0]") && hijo["sports[0]"].Equals(1))
                                    {
                                        productos.Add(packFutbol);
                                    }
                                    else if(hijo.ContainsKey("sports[0]") && hijo["sports[0]"].Equals(2)){
                                        productos.Add(packBasket);
                                    }
                            }
                        }
                        int x = 0;
                        while (hijo.ContainsKey("sports[" + x + "]"))
                        {
                            insertHijoDeporte(idHijo, hijo["sports[" + x + "]"].ToString());
                            x++;
                        }

                    }

                    Prestashop.addCarrito(Int32.Parse(res["id"].ToString()), res["secure_key"].ToString(), productos);


                    if (inserted > 0)
                    {
                        Util.sendEmail(email, "Registro CLUB DEPORTIVO ATABAL",
                       "<div style='font-family:Arial;background:#203864;text-align:center;color:#fff'><br/><h3 style='margin:0'>Se ha registrado en El atabal</h3><br/></div><br/><div style='text-align: center;'><img src=\"cid:{0}\"/></div><br/>" +
                       "<p style='font-family:Arial'>Los datos de su hijo/a se han guardado correctamente</p><br><p>Los datos de acceso son:<br>Usuario:" + user + "<br>Contraseña:" + pass + "</p><br><p>Los datos de acceso para la <a href='http://elatabaltienda.dged.es/es/'>tienda on-line</a> son:<br>Usuario:" + email + "<br>Contraseña:" + pass + "</p>");

                        resp.cod = "OK";
                    }
                    else
                    {
                        resp.cod = "KO";
                    }
                }
            }
            }
            catch (Exception e)
            {
                Task.Run(async () => { Extensiones.sendTelegram("Registrar - Mensaje: " + e.Message); }).Wait();
                Task.Run(async () => { Extensiones.sendTelegram("StackTrace: " + e.StackTrace); }).Wait();
                Task.Run(async () => { Extensiones.sendTelegram("exception: " + e.ToString()); }).Wait();
            }

            return Json(resp);
        }

        private long registrarHijo(Dictionary<string, object> hijo,long idPadre)
        {
            
            try { 
            string talla = "";
            string observaciones = "";
            if (hijo.ContainsKey("talla"))
            {
                talla = hijo["talla"].ToString();
            }

            if (hijo.ContainsKey("observaciones"))
            {
                observaciones = hijo["observaciones"].ToString();
            }

            bool extraescolar =false;
            bool pack = false;

            if (hijo.ContainsKey("extraescolares"))
            {
                extraescolar = (bool)hijo["extraescolares"];
                }


            if (hijo.ContainsKey("pack"))
            {
                pack = (int)hijo["pack"]==1;
                }


            foreach(var k in hijo.Keys)
                {
                    Task.Run(async () => { Extensiones.sendTelegram(k+": "+hijo[k]); }).Wait();
                }

            int anio = Int32.Parse(hijo["nacimiento"].ToString().Substring(6));
            int mes = Int32.Parse(hijo["nacimiento"].ToString().Substring(3,2));
            int dia = Int32.Parse(hijo["nacimiento"].ToString().Substring(0,2));

            DateTime fechaNacimiento = new DateTime(anio,mes,dia);


            long id;

            using (MySqlConnection con = new MySqlConnection(BD.CadConMySQL(BD.Server.BDLOCAL, BD.schema)))
            {
                using (MySqlCommand cmd = new MySqlCommand(string.Empty, con))
                {

                    cmd.CommandText = "INSERT INTO atabal.usuarios_hijos (nombre,apellidos,fecha_nacimiento,sexo,extraescolares,pack,talla,numero,observaciones,idpadre)" +
                                      " VALUES (?nombre,?apellidos,?fecha_nacimiento,?sexo,?extraescolares,?pack,?talla,?numero,?observaciones,?idpadre);";
                    cmd.Parameters.AddWithValue("?idPadre", idPadre);
                    cmd.Parameters.AddWithValue("?nombre",hijo["nombre"]);
                    cmd.Parameters.AddWithValue("?apellidos", hijo["apellidos"]);
                    cmd.Parameters.AddWithValue("?fecha_nacimiento", fechaNacimiento);
                    cmd.Parameters.AddWithValue("?sexo", hijo["sexo"]);
                    cmd.Parameters.AddWithValue("?extraescolares", extraescolar);
                    cmd.Parameters.AddWithValue("?pack", pack);
                    cmd.Parameters.AddWithValue("?talla", talla);
                    cmd.Parameters.AddWithValue("?numero", hijo["numero"]);
                    cmd.Parameters.AddWithValue("?observaciones", observaciones);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    id = cmd.LastInsertedId;
                    con.Close();
                    
                }
            }

            return id;

            }
            catch (Exception e)
            {
                Task.Run(async () => { Extensiones.sendTelegram("registrarHijo Mensaje: " + e.Message); }).Wait();
                Task.Run(async () => { Extensiones.sendTelegram("StackTrace: " + e.StackTrace); }).Wait();
                Task.Run(async () => { Extensiones.sendTelegram("exception: " + e.ToString()); }).Wait();
                Task.Run(async () => { Extensiones.sendTelegram(hijo.Values.ToString()); }).Wait();
            }
            return -1;
        }

        private void insertHijoDeporte(long idHijo, string idDeporte)
        {
            try { 
            using (MySqlConnection con = new MySqlConnection(BD.CadConMySQL(BD.Server.BDLOCAL, BD.schema)))
            {
                using (MySqlCommand cmd = new MySqlCommand(string.Empty, con))
                {

                    cmd.CommandText = "INSERT INTO atabal.usuarios_hijos_deportes (idhijo,iddeporte)" +
                                      " VALUES (?idHijo, ?idDeporte);";
                    cmd.Parameters.AddWithValue("?idHijo", idHijo);
                    cmd.Parameters.AddWithValue("?idDeporte",idDeporte);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    
                }
            }

            }
            catch (Exception e)
            {
                Task.Run(async () => { Extensiones.sendTelegram("insertHijoDeporte Mensaje: " + e.Message); }).Wait();
                Task.Run(async () => { Extensiones.sendTelegram("StackTrace: " + e.StackTrace); }).Wait();
                Task.Run(async () => { Extensiones.sendTelegram("exception: " + e.ToString()); }).Wait();
            }


        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult ExisteUsuario(string usuario)
        {

            RespGeneric resp = new RespGeneric("OK");
            DataTable dt = new DataTable();

            using (MySqlConnection con = new MySqlConnection(BD.CadConMySQL(BD.Server.BDLOCAL, BD.schema)))
            {
                using (MySqlCommand cmd = new MySqlCommand(string.Empty, con))
                {
                    using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                    {
                        cmd.CommandText = "SELECT usuario FROM usuarios WHERE usuario=?usuario;";
                        cmd.Parameters.AddWithValue("?usuario", usuario);
                        da.Fill(dt);
                        if (dt.Rows.Count > 0)
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

            return Json(resp);
        }

        //private bool insertAcceso(string usuario, string ip, string status, string msg)
        //{
        //    int numReg = 0;

        //    using (MySqlConnection con = new MySqlConnection(BD.CadConMySQL(BD.Server.BDLOCAL, BD.schema)))
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
