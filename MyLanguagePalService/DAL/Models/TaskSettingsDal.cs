namespace MyLanguagePalService.DAL.Models
{
    public class TaskSettingsDal
    {
        public int Id { get; set; }

        public int TaskId { get; set; }

        public string SettingsJson { get; set; }
    }
}