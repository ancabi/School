using school.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Web;
using System.Web.Hosting;
using System.Web.WebPages;

namespace school.Models
{
    public class Util
    {
        public static bool IsNumeric(string input)
        {
            int test;
            return int.TryParse(input, out test);
        }

        public static string getSQL(MySql.Data.MySqlClient.MySqlCommand cmd)
        {
            string sql = cmd.CommandText;

            foreach (MySql.Data.MySqlClient.MySqlParameter par in cmd.Parameters)
            {
                if (par.Value is string)
                {
                    sql = sql.Replace(par.ParameterName, "'" + par.Value + "'");
                }
                else if (par.Value is System.DateTime)
                {
                    sql = sql.Replace(par.ParameterName, "'" + Convert.ToDateTime(par.Value).ToMySqlDate() + "'");
                }
                else if (par.Value is DateTime)
                {
                    sql = sql.Replace(par.ParameterName, "'" + Convert.ToDateTime(par.Value).ToMySqlDateTime() + "'");
                }
                else if (par.Value == DBNull.Value)
                {
                    sql = sql.Replace(par.ParameterName, "NULL");
                }
                else if (IsNumeric(par.Value.ToString()))
                {
                    sql = sql.Replace(par.ParameterName, par.Value.ToString().Replace(",", "."));
                }
                else {
                    sql = sql.Replace(par.ParameterName.ToString(), par.Value.ToString());
                }
            }
            return sql.ToString();
        }

        public static string getSQL(System.Data.SqlClient.SqlCommand cmd)
        {
            string sql = cmd.CommandText;

            foreach (System.Data.SqlClient.SqlParameter par in cmd.Parameters)
            {
                if (par.Value is string)
                {
                    sql = sql.Replace(par.ParameterName, "'" + par.Value + "'");
                }
                else if (par.Value is System.DateTime)
                {
                    sql = sql.Replace(par.ParameterName, "'" + Convert.ToDateTime(par.Value).ToMySqlDate() + "'");
                }
                else if (par.Value is DateTime)
                {
                    sql = sql.Replace(par.ParameterName, "'" + Convert.ToDateTime(par.Value).ToMySqlDateTime() + "'");
                }
                else if (par.Value == DBNull.Value)
                {
                    sql = sql.Replace(par.ParameterName, "NULL");
                }
                else if (IsNumeric(par.Value.ToString()))
                {
                    sql = sql.Replace(par.ParameterName, par.Value.ToString().Replace(",", "."));
                }
                else {
                    sql = sql.Replace(par.ParameterName.ToString(), par.Value.ToString());
                }
            }
            return sql.ToString();
        }

        public static string getYearFromNumero(string numero)
        {
            return string.Concat("20", numero.TrimStart().Substring(0, 2));
        }

        public static RespGeneric sendEmail(string to, string subject, string body)
        {
            RespGeneric resp = new RespGeneric("OK");
            try
            {
                MailMessage email = new MailMessage();
                foreach (var address in to.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    email.To.Add(address);
                }
                email.From = new MailAddress("info@dged.es");
                email.Subject = subject;

                //LOGO
                Attachment logo = new Attachment(HostingEnvironment.MapPath("~/images/logo_activa.png"), "image/png");
                email.Attachments.Add(logo);

                email.Body = String.Format(body, logo.ContentId); ;
                email.IsBodyHtml = true;
                email.Priority = MailPriority.Normal;
                
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "mail.dged.es";
                smtp.Port = 25;
                smtp.EnableSsl = false;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential("info.dged", "2d2d1f2r1we2");

                smtp.Send(email);
                email.Dispose();

                return resp;

            }
            catch (Exception)
            {
                resp.cod = "KO";

                return resp;

            }

        }
    }
}