﻿using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web.Http;
using MyLanguagePalService.Areas.App.Models.Controller.Languages;
using MyLanguagePalService.DAL;
using MyLanguagePalService.DAL.Models;

namespace MyLanguagePalService.Areas.App.Controllers
{
    public class LanguagesController : ApiController
    {
        private readonly IApplicationDbContext _db;

        public LanguagesController()
        {
            _db = new ApplicationDbContext();
        }

        public IEnumerable<LanguageAm> GetAllLanguages()
        {
            Thread.Sleep(3000);
            return _db.Languages.Select(ToAm).ToList();
        }

        public IHttpActionResult GetLanguage(int id)
        {
            Thread.Sleep(3000);
            var languageDal = _db.Languages.Find(id);
            if (languageDal == null)
            {
                return NotFound();
            }
            return Ok(ToAm(languageDal));
        }

        private LanguageAm ToAm(LanguageDal languageDal)
        {
            return new LanguageAm()
            {
                Id = languageDal.Id,
                Name = languageDal.Name
            };
        }
    }
}