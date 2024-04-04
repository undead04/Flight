using Microsoft.AspNetCore.Identity;

namespace Flight.Data
{
    public class ApplicationUser:IdentityUser
    {
      
        public string Name { get; set; } = string.Empty;
        public string Avatar { get; set; } = string.Empty;
        public ICollection<DocumentFlight>? DocumentFlights { get; set; }
        public ICollection<DocumentType>? DocumentTypes { get; set; }
        public ICollection<GroupPermission>? GroupPermissions { get; set; }
    }
}
