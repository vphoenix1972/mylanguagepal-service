namespace MyLanguagePal.Shared.Exceptions
{
    public class MlpNotFoundException : MlpException
    {
        protected MlpNotFoundException(int code) :
            base(code)
        {

        }

        protected MlpNotFoundException(int code, string message) :
            base(code, message)
        {

        }
    }
}