using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;

namespace school.Helpers
{
    public static class Extensiones
    {
        //        public static bool IsDebug(this HtmlHelper htmlHelper)
        //        {
        //#if DEBUG
        //            return true;
        //#else
        //                return false;
        //#endif
        //        }

        public static string ToMySqlDate(this DateTime d)
        {
            return d.ToString("yyyy-MM-dd");
        }

        public static string ToMySqlDateTime(this DateTime d)
        {
            return d.ToString("yyyy-MM-dd");
        }

        //public static List<object> ToList(this DataTable dt)
        //{
        //    List<object> lstRows = new List<object>();
        //    foreach (DataRow dr in dt.Rows)
        //    {
        //        Dictionary<string, object> row = new Dictionary<string, object>();
        //        foreach (DataColumn col in dt.Columns)
        //        {
        //            row.Add(col.ColumnName, dr[col]);
        //        }
        //        lstRows.Add(row);
        //    }

        //    return lstRows;
        //}

        public static List<Dictionary<string, object>> ToList(this DataTable dt)
        {
            List<Dictionary<string, object>> lstRows = new List<Dictionary<string, object>>();
            foreach (DataRow dr in dt.Rows)
            {
                Dictionary<string, object> row = new Dictionary<string, object>();
                foreach (DataColumn col in dt.Columns)
                {
                    if (dr[col].GetType() == typeof(System.String)){
                        row.Add(col.ColumnName.ToLower(), dr[col].ToString().Trim());
                    }
                    else
                    {
                        row.Add(col.ColumnName.ToLower(), dr[col]);
                    }                    
                }
                lstRows.Add(row);
            }
            return lstRows;
        }

        public static List<string> ToSingleList(this DataTable dt)
        {
            List<string> lstRows = new List<string>();
            foreach (DataRow dr in dt.Rows)
            {
                Dictionary<string, object> row = new Dictionary<string, object>();
                foreach (DataColumn col in dt.Columns)
                {
                    if (dr[col].GetType() == typeof(System.String))
                    {
                        lstRows.Add(dr[col].ToString().Trim());
                    }
                }
            }
            return lstRows;
        }        

        public static Dictionary<string, object> ToDictionary(this DataRow row)
        {
            Dictionary<string, object> dictRow = new Dictionary<string, object>();
            foreach (DataColumn col in row.Table.Columns)
            {
                if (row[col.ColumnName].GetType() == typeof(System.String))
                {
                    dictRow.Add(col.ColumnName.ToLower(), row[col.ColumnName].ToString().Trim());
                }
                else
                {
                    dictRow.Add(col.ColumnName.ToLower(), row[col.ColumnName]);
                }                
            }
            return dictRow;
        }

        public static string ToDblDot(this string d)
        {
            return d.ToString().Replace(",", ".");
        }

        public static string ToDblDot(this double d)
        {
            return d.ToString().Replace(",", ".");
        }

        public static string ToDblComma(this string d)
        {
            return d.ToString().Replace(".", ",");
        }

        public static bool IsNumber(this string input)
        {
            double test;
            return double.TryParse(input, out test);
        }

        public static bool IsDate(this string input)
        {
            DateTime test;
            return DateTime.TryParse(input, out test);
        }

        public static string toMySQLString(this List<string> lista, string columna, bool? insertAnd = true)
        {
            StringBuilder sbMySQL = new StringBuilder();

            if (lista.Count > 0)
            {
                if (insertAnd == true)
                {
                    sbMySQL.Append(" AND (");
                }
                else
                {
                    sbMySQL.Append(" (");
                }
                foreach (string s in lista)
                {
                    sbMySQL.Append(columna);
                    if (s.ToString().IsNumber())
                    {
                        sbMySQL.Append("=");
                    }
                    else {
                        sbMySQL.Append("='");
                    }
                    sbMySQL.Append(s);
                    if (s.ToString().IsNumber())
                    {
                        sbMySQL.Append(" OR ");
                    }
                    else {
                        sbMySQL.Append("' OR ");
                    }
                }
                sbMySQL.Remove(sbMySQL.Length - 4, 4).Append(") ");
            }
            return sbMySQL.ToString();
        }

        public static string ToSQL(this MySql.Data.MySqlClient.MySqlCommand cmd)
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
                else if (par.Value.ToString().IsNumber())
                {
                    sql = sql.Replace(par.ParameterName, par.Value.ToString().Replace(",", "."));
                }
                else {
                    sql = sql.Replace(par.ParameterName.ToString(), par.Value.ToString());
                }
            }
            return sql.ToString();
        }

        public static string ToSQL(this System.Data.SqlClient.SqlCommand cmd)
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
                else if (par.Value.ToString().IsNumber())
                {
                    sql = sql.Replace(par.ParameterName, par.Value.ToString().Replace(",", "."));
                }
                else {
                    sql = sql.Replace(par.ParameterName.ToString(), par.Value.ToString());
                }
            }
            return sql.ToString();
        }

        public static string sendEmail(List<string> to, string subject, string body, string imagePath)
        {

            try
            {
                MailMessage email = new MailMessage();
                foreach (var address in to)
                {
                    email.To.Add(address);
                }
                email.From = new MailAddress("alerta@kwsolutions.es");
                email.Subject = subject;
                email.Body = body;
                email.IsBodyHtml = true;
                email.Priority = MailPriority.Normal;
                if (!imagePath.IsEmpty())
                {
                    Attachment inline = new Attachment(imagePath);
                    inline.ContentDisposition.Inline = true;
                    inline.ContentDisposition.DispositionType = DispositionTypeNames.Inline;
                    inline.ContentType.MediaType = "image/png";
                    inline.ContentType.Name = Path.GetFileName(imagePath);

                    email.Attachments.Add(inline);
                }
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.kwsolutions.es";
                smtp.Port = 587;
                smtp.EnableSsl = false;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential("alerta@kwsolutions.es", "sCtH-3bKE#r8A");

                smtp.Send(email);
                email.Dispose();

                string res = "Correo electrónico enviado satisfactoriamente";

                return res;

            }
            catch (Exception)
            {
                string res = "Error enviando correo electrónico";

                return res;

            }

        }

        public static async void sendTelegram(string mensaje)
        {
            var Bot = new TelegramBotClient("389655189:AAGAkCGp1qq2vsle1lU0LaWn9h2wbFlT4Go");
            Bot.SendTextMessageAsync(7227471, mensaje);
        }

    }
}