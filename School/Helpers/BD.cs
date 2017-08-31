using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;
using System.Data;
using System.Security.Cryptography;
using Medo.Security.Cryptography;

namespace school.Models
{
    public class BD
    {
        private const string USER_BD = "root";
        private const string PASS_BD = "comicStores1";
        public const string URL_INTRANET = "";
        public const string URL_DOWN_OFERTAS = "";
        public const string RUTA_UPLOAD = "~/uploads/";
        public const string school_CENTRAL = "43000474";
        public const string schema = "atabal";

        private static readonly int passIterations = 12000;


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
                    //cadCon.Server = "217.61.21.121";
                    cadCon.Server = "localhost";
                    cadCon.Port = 3306;
                    cadCon.UserID = "root";
                    cadCon.Password = "comicStores1";
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

        public static bool CheckPassword(string passwordHashed, string pass)
        {
            String salt = passwordHashed.Substring(20, 12);

            String hashed = HashPassword(pass, salt);

            if (hashed.Equals(passwordHashed))
            {
                return true;
            }

            return false;

        }

        public static string HashPassword(string pass, string salt)
        {
            var hashed = String.Empty;

            using (var hmac = new HMACSHA256())
            {
                var df = new Pbkdf2(hmac, pass, salt, passIterations);

                String passEncrypted = Convert.ToBase64String(df.GetBytes(32));

                hashed = String.Format("pbkdf2_sha256${0}${1}${2}", passIterations, salt, passEncrypted);

            }

            return hashed;
        }

        public static String CreateSalt(Int32 num)
        {
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            Byte[] buff = new Byte[num];
            rng.GetBytes(buff);
            return Convert.ToBase64String(buff);
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