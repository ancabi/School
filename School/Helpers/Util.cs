using school.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
    }
}