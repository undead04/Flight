namespace Flight.Service.ChangeOwnerService
{
    public interface IChangeOwnerService
    {
        Task<bool> ChangeOwerSystem(string userId);
        Task<bool> ComfimChangOwerSystem(string userId,string password);
    }
}
