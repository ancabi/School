using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;
using System.Data;

namespace school.Models
{
    public class BD
    {
        private const string USER_BD = "root";
        private const string PASS_BD = "";
        public const string URL_INTRANET = "";
        public const string URL_DOWN_OFERTAS = "";
        public const string RUTA_UPLOAD = "~/uploads/";
        public const string school_CENTRAL = "43000474";

        enum ewDB
        {
            Fiscal = 0,
            Comun = 1,
            Plantillas = 3
        }

        public enum Server
        {
            BDLOCAL = 0,
        }

        public static string CadConMySQL(BD.Server server, string database)
        {
            MySqlConnectionStringBuilder cadCon = new MySqlConnectionStringBuilder();

            switch (server)
            {
                case BD.Server.BDLOCAL:
                    cadCon.Server = "localhost";
                    cadCon.Port = 33062;
                    cadCon.UserID = "root";
                    cadCon.Password = "";
                    break;
            }

            cadCon.Database = database;
            cadCon.PersistSecurityInfo = true;
            cadCon.AllowZeroDateTime = false;
            cadCon.ConnectionTimeout = 200;
            cadCon.Pooling = true;
            cadCon.ConnectionLifeTime = 300;
            cadCon.MaximumPoolSize = 200;
            cadCon.CharacterSet = "utf8";
            cadCon.AllowUserVariables = true;
            cadCon.RespectBinaryFlags = false;

            return cadCon.ConnectionString;
        }

        //static void insertException(string moduleName, string functionName, string exMsg, string exStackTrace, string extraData)
        //{
        //    using (DataTable dt = new DataTable())
        //    {
        //        using (MySqlConnection con = new MySqlConnection(BD.CadConMySQL(Server.BDLOCAL, "intranet")))
        //        {
        //            using (MySqlCommand cmd = new MySqlCommand("INSERT INTO intranet.excepciones_no_controladas (module,function_name,ex_msg,ex_stacktrace,extra_data,fecha) VALUES (?module,?function_name,?ex_msg,?ex_stacktrace,?extra_data,NOW());", con))
        //            {
        //                // ERROR: Not supported in C#: WithStatement

        //                using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
        //                {
        //                    da.Fill(dt);
        //                }
        //            }
        //        }
        //    }
        //}
    }
}