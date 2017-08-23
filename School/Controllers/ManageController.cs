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

namespace School.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {

		public ActionResult Usuarios()
		{
			return View();
		}

    }
}