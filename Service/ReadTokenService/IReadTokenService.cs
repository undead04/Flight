namespace Flight.Service.ReadTokenService
{
    public interface IReadTokenService
    {
        Task<string> ReadJWT();
    }
}
