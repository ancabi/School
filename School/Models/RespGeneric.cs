using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace school.Models
{
    public class RespGeneric
    {
        private string _cod = null;
        private string _msg = null;

        private Dictionary<string, object> _d;
        public RespGeneric()
        {
            _cod = string.Empty;
            _msg = string.Empty;
            _d = new Dictionary<string, object>();
        }

        public RespGeneric(string codigo)
        {
            _cod = codigo;
            _d = new Dictionary<string, object>();
        }

        public RespGeneric(string codigo, string message)
        {
            _cod = codigo;
            _msg = message;
            _d = new Dictionary<string, object>();
        }

        public string cod
        {
            get { return _cod; }
            set { _cod = value; }
        }
        public string msg
        {
            get { return _msg; }
            set { _msg = value; }
        }
        public Dictionary<string, object> d
        {
            get { return _d; }
            set { _d = value; }
        }
    }
}