using System.Collections.Generic;
using JetBrains.Annotations;

namespace MyLanguagePalService.BLL.Tags
{
    public interface ITagsService
    {
        [NotNull]
        IList<Tag> GetTags();

        [CanBeNull]
        Tag GetTag(int id);

        [NotNull]
        Tag CreateTag([NotNull] Tag newTag);

        [CanBeNull]
        Tag UpdateTag(int id, [NotNull] Tag tag);

        [CanBeNull]
        Tag DeleteTag(int id);
    }
}