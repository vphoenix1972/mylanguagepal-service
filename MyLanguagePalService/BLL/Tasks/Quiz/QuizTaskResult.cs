namespace MyLanguagePalService.BLL.Tasks.Quiz
{
    public class QuizTaskResult<T>
    {
        public T Entity { get; set; }

        public bool IsCorrect { get; set; }

        public double Improvement { get; set; }
    }
}