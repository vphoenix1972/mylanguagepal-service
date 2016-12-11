namespace MyLanguagePalService.BLL
{
    public abstract class ServiceBase
    {
        protected string GetLanguageNotExistString(int id)
        {
            return $"Language with id {id} does not exist";
        }
    }
}