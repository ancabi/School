using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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

    }
}
