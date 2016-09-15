using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using MyLanguagePalService.DAL;
using MyLanguagePalService.DAL.Models;

namespace MyLanguagePalService.Controllers
{
    public class LanguagesController : ApiController
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        // GET: api/Language
        public IQueryable<LanguageDal> GetLanguages()
        {
            return _db.Languages;
        }

        // GET: api/Language/5
        [ResponseType(typeof(LanguageDal))]
        public IHttpActionResult GetLanguage(int id)
        {
            var languageDal = _db.Languages.Find(id);
            if (languageDal == null)
            {
                return NotFound();
            }

            return Ok(languageDal);
        }

        // PUT: api/Language/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutLanguage(int id, LanguageDal languageDal)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != languageDal.Id)
            {
                return BadRequest();
            }

            _db.Entry(languageDal).State = EntityState.Modified;

            try
            {
                _db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LanguageExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Language
        [ResponseType(typeof(LanguageDal))]
        public IHttpActionResult PostLanguage(LanguageDal languageDal)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _db.Languages.Add(languageDal);
            _db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = languageDal.Id }, languageDal);
        }

        // DELETE: api/Language/5
        [ResponseType(typeof(LanguageDal))]
        public IHttpActionResult DeleteLanguage(int id)
        {
            var languageDal = _db.Languages.Find(id);
            if (languageDal == null)
            {
                return NotFound();
            }

            _db.Languages.Remove(languageDal);
            _db.SaveChanges();

            return Ok(languageDal);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool LanguageExists(int id)
        {
            return _db.Languages.Count(e => e.Id == id) > 0;
        }
    }
}