﻿using System;
using System.Web.Mvc;

namespace MyLanguagePalService.Controllers
{
    public class TestController : Controller
    {
        // GET: Crash
        public string Crash()
        {
            throw new Exception();
        }
    }
}