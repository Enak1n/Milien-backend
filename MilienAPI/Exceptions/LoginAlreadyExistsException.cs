namespace MilienAPI.Exceptions
{
    public class LoginAlreadyExistsException : Exception
    {
        public LoginAlreadyExistsException(string message) : base(message)
        {
        }
    }
}
