using Light.Admin.Mongo;
using Light.Admin.Mongo.Basics;
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
            Name = user.Name;
            Roles = user.Roles;
        }

        /// <summary>
        /// 元数据
        /// </summary>
        public AuditMetadata AuditMetadata { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public ObjectId Id { get; set; }

        /// <summary>
        /// 账户ID
        /// </summary>
        public ObjectId AccountId { get => Id; }

        /// <summary>
        /// 用户名/账户
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 用户名(显示)
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 角色
        /// </summary>
        public  List<string> Roles { get; set; }

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
