﻿using System.Web.Mvc;

namespace MyLanguagePalService.Controllers
{
    public class ErrorController : Controller
    {
        // Default
        public ActionResult Index()
        {
            return Http500();
        }

        // 401 - Unauthorized
        public ActionResult Http401()
        {
            if (Response.StatusCode == 200)
                Response.StatusCode = 401;

            return View();
        }

        // 403 - Forbidden
        public ActionResult Http403()
        {
            if (Response.StatusCode == 200)
                Response.StatusCode = 403;

            return View();
        }

        // 404 - Not found
        public ActionResult Http404()
        {
            if (Response.StatusCode == 200)
                Response.StatusCode = 404;

            return View();
        }

        // 500 - Internal server error
        public ActionResult Http500()
        {
            if (Response.StatusCode == 200)
                Response.StatusCode = 500;

            return View();
        }
    }
}