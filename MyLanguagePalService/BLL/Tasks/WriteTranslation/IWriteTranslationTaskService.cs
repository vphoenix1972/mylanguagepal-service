using JetBrains.Annotations;

namespace MyLanguagePalService.BLL.Tasks.WriteTranslation
{
    public interface IWriteTranslationTaskService
    {
        /// <summary>
        /// Returns settings for the task.
        /// </summary>
        [NotNull]
        WriteTranslationTaskSettingModel GetSettings();

        /// <summary>
        /// Sets settings for the task.
        /// </summary>
        void SetSettings([NotNull] WriteTranslationTaskSettingModel settings);

        /// <summary>
        /// Creates a model for a new sprint task using settings specified.
        /// </summary>        
        [NotNull]
        WriteTranslationTaskRunModel RunNewTask([NotNull] WriteTranslationTaskSettingModel settings);

        /// <summary>
        /// Writes statistics about finished sprint task.
        /// </summary>
        /// <param name="summary"></param>
        void FinishTask([NotNull] WriteTranslationTaskFinishedSummaryModel summary);
    }
}