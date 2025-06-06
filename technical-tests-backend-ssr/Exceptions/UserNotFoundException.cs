namespace TechnicalTests.Backend.SSR.Exceptions
{
    public class UserNotFoundException : ValidacionException
    {
        public UserNotFoundException() : base("El usuario no existe") { }
    }
}
