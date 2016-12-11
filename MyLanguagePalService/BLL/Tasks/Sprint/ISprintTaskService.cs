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
        /// Gets data for a new task using the current settings (settings returned by GetSettings()).
        /// </summary>
        [NotNull]
        SprintTaskData GetDataForNewTask();
    }
}