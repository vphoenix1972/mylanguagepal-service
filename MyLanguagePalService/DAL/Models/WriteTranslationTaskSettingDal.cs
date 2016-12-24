namespace MyLanguagePalService.DAL.Models
{
    public class WriteTranslationTaskSettingDal
    {
        public int Id { get; set; }

        public int LanguageId { get; set; }

        public int CountOfWordsUsed { get; set; }

        public virtual LanguageDal Language { get; set; }
    }
}