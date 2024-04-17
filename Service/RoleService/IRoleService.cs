namespace Flight.Service.RoleService
{
    public interface IRoleService
    {
        Task AddMemberRole(string userId,string roleId);
        Task DeleteRoleMember(string userId);
    }
}
