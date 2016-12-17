using JetBrains.Annotations;

namespace MyLanguagePalService.BLL.Tasks.Sprint
{
    public interface ISprintTaskService
    {
        /// <summary>
        /// Returns settings for the task.
        /// </summary>
        [NotNull]
        SprintTaskSettingModel GetSettings();

        /// <summary>
        /// Sets settings for the task.
        /// </summary>
        void SetSettings([NotNull] SprintTaskSettingModel settings);

        /// <summary>
        /// Creates a model for a new sprint task using settings specified.
        /// </summary>        
        [NotNull]
        SprintTaskRunModel RunNewTask([NotNull] SprintTaskSettingModel settings);
    }
}