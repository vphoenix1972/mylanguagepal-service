﻿using System.Web.Http;
using JetBrains.Annotations;
using MyLanguagePalService.BLL;
using MyLanguagePalService.Core;
using MyLanguagePalService.DAL;

namespace MyLanguagePalService.Areas.App.Controllers
{
    public abstract class AppApiControllerBase : WebApiControllerBase
    {
        protected AppApiControllerBase()
        {
            var db = new ApplicationDbContext();
            ServiceManager = new ServiceManager(db);
        }

        [NotNull]
        protected IServiceManager ServiceManager { get; }

        protected override void Dispose(bool disposing)
        {
            ServiceManager.Dispose();
            base.Dispose(disposing);
        }

        protected IHttpActionResult PhraseNotFound(int id)
        {
            return NotFound($"Phrase '{id}' does not exist");
        }
    }
}