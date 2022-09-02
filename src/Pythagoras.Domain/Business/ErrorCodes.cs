namespace Saorsa.Pythagoras.Domain.Business;

public static class ErrorCodes
{
    public static class ApplicationExit
    {
        public const int GeneralError = 1;
    }
    
    public static class Auth
    {
        public const int Unauthorized = 401;
        public const int Forbidden = 403;
    }
}
