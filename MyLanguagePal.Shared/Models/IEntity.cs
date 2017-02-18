namespace MyLanguagePal.Shared.Models
{
    public interface IEntity<T>
    {
        T Id { get; set; }
    }
}