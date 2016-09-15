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

        // GET: api/LanguageApi
        public IQueryable<LanguageDal> GetLanguageDals()
        {
            return _db.Languages;
        }

        // GET: api/LanguageApi/5
        [ResponseType(typeof(LanguageDal))]
        public IHttpActionResult GetLanguageDal(int id)
        {
            var languageDal = _db.Languages.Find(id);
            if (languageDal == null)
            {
                return NotFound();
            }

            return Ok(languageDal);
        }

        // PUT: api/LanguageApi/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutLanguageDal(int id, LanguageDal languageDal)
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
                if (!LanguageDalExists(id))
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

        // POST: api/LanguageApi
        [ResponseType(typeof(LanguageDal))]
        public IHttpActionResult PostLanguageDal(LanguageDal languageDal)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _db.Languages.Add(languageDal);
            _db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = languageDal.Id }, languageDal);
        }

        // DELETE: api/LanguageApi/5
        [ResponseType(typeof(LanguageDal))]
        public IHttpActionResult DeleteLanguageDal(int id)
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

        private bool LanguageDalExists(int id)
        {
            return _db.Languages.Count(e => e.Id == id) > 0;
        }
    }
}