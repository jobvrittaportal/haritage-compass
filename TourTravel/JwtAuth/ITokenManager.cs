namespace TourTravel.JwtAuth
{
    public interface ITokenManager
    {
        string NewToken(string ID);
    }
}
