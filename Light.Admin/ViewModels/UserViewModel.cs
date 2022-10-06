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

        /// <summary>
        /// 元数据
        /// </summary>
        public AuditMetadata AuditMetadata { get; set; }

        /// <summary>
        /// ObjectId
        /// </summary>
        public ObjectId Id { get; set; }
        /// <summary>
        /// 用户名/账户
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }
    }
}
