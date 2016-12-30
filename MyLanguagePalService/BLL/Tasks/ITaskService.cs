using JetBrains.Annotations;

namespace MyLanguagePalService.BLL.Tasks
{
    public interface ITaskService
    {
        [NotNull]
        string Name { get; }

        [NotNull]
        object GetSettings();

        object SetSettings([NotNull] object settings);

        [NotNull]
        object RunNewTask([NotNull] object settings);

        object FinishTask([NotNull] object settings, [NotNull] object answers);
    }
}