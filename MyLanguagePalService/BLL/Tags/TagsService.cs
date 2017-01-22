using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using MyLanguagePalService.Core;
using MyLanguagePalService.Core.Extensions;
using MyLanguagePalService.DAL;

namespace MyLanguagePalService.BLL.Tags
{
    public class TagsService : ITagsService
    {
        public const int MaxTagLength = 100;

        private readonly IApplicationDbContext _db;

        public TagsService([NotNull] IApplicationDbContext db)
        {
            if (db == null)
                throw new ArgumentNullException(nameof(db));

            _db = db;
        }

        public IList<Tag> GetTags()
        {
            return _db.Tags.Select(Tag.Map).ToList();
        }

        public Tag GetTag(int id)
        {
            var dal = _db.Tags.Find(id);
            if (dal == null)
                return null;

            return Tag.Map(dal);
        }

        public Tag CreateTag(Tag newTag)
        {
            AssertTag(newTag);

            newTag.Id = 0;

            var newDal = Tag.Map(newTag);

            _db.Tags.Add(newDal);

            _db.SaveChanges();

            newTag.Id = newDal.Id;

            return newTag;
        }

        public Tag UpdateTag(int id, Tag tag)
        {
            var dal = _db.Tags.Find(id);
            if (dal == null)
                return null;

            AssertTag(tag);

            tag.Id = id;

            Tag.Map(tag, dal);

            _db.MarkModified(dal);

            _db.SaveChanges();

            return tag;
        }

        public Tag DeleteTag(int id)
        {
            var dal = _db.Tags.Find(id);
            if (dal == null)
                return null;

            dal.Phrases.ForEach(p => p.Tags.Remove(dal));

            _db.Tags.Remove(dal);

            _db.SaveChanges();

            return Tag.Map(dal);
        }

        private void AssertTag(Tag tag)
        {
            if (tag == null)
                throw new ArgumentNullException(nameof(tag));

            if (string.IsNullOrWhiteSpace(tag.Name))
                throw new ValidationFailedException(nameof(tag.Name), "Tag's name cannot be empty");

            if (tag.Name.Length > MaxTagLength)
                throw new ValidationFailedException(nameof(tag.Name), $"Tag's name length cannot be more than {MaxTagLength} letters");
        }
    }
}