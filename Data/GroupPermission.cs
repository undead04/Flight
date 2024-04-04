using Microsoft.AspNetCore.Identity;

namespace Flight.Data
{
    public class GroupPermission:IdentityRole
    {
        public string Note { get; set; } = string.Empty;
        public string UserId { get; set; }=string.Empty;
        public DateTime Create_Date { get; set; }
        public virtual ApplicationUser? ApplicationUser { get; set; }
    }
}
