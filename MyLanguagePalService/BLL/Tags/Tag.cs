using System;
using JetBrains.Annotations;
using MyLanguagePalService.DAL.Models;

namespace MyLanguagePalService.BLL.Tags
{
    public class Tag
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public static Tag Map([NotNull] TagDal from, [NotNull] Tag to)
        {
            if (from == null)
                throw new ArgumentNullException(nameof(from));
            if (to == null)
                throw new ArgumentNullException(nameof(to));

            to.Id = from.Id;
            to.Name = from.Name;

            return to;
        }

        public static Tag Map([NotNull] TagDal @from)
        {
            return Map(@from, new Tag());
        }

        [NotNull]
        public static TagDal Map([NotNull] Tag from, [NotNull] TagDal to)
        {
            if (from == null)
                throw new ArgumentNullException(nameof(from));
            if (to == null)
                throw new ArgumentNullException(nameof(to));

            to.Id = from.Id;
            to.Name = from.Name;

            return to;
        }

        [NotNull]
        public static TagDal Map([NotNull] Tag tag)
        {
            return Map(tag, new TagDal());
        }
    }
}