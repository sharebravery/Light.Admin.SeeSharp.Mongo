using Light.Admin.Models;
using Light.Admin.Models.Basics;
using MongoDB.Bson;

namespace Light.Admin.ViewModels
{
    public class UserViewModel
    {
        public UserViewModel(User user)
        {
            Id = user.Id;
            UserName = user.UserName;
            PhoneNumber = user.PhoneNumber;
            Email = user.Email;
        }

        public AuditMetadata AuditMetadata { get; set; }

        public ObjectId Id { get; set; }

        public string UserName { get; set; }

        public string PhoneNumber { get; set; }
        public string Email { get; set; }

    }
}
