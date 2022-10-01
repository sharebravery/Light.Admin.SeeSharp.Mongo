namespace Light.Admin.CSharp.Dto
{
    public class UserDto
    {
        /// <summary>
        /// 用户名/账号
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 用户名（显示名称）
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 再次输入密码
        /// </summary>
        public string ConfirmPassword { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 手机号码是否已验证
        /// </summary>
        public bool PhoneNumberConfirmed { get; set; }

        /// <summary>
        /// 邮件是否已验证
        /// </summary>
        public bool EmailConfirmed { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public GenderEnum Gender { get; set; }

        /// <summary>
        /// 年龄
        /// </summary>
        public int Age { get; set; }

        /// <summary>
        /// 锁定
        /// </summary>
        public bool LockoutEnabled { get; set; }

        /// <summary>
        /// 账号自动解锁时间
        /// </summary>  
        public DateTimeOffset? LockoutEnd { get; set; }

        /// <summary>
        /// 角色
        /// </summary>
        public List<string> Roles { get; set; }

        /// <summary>
        /// 所属部门
        /// </summary>
        public long Department { get; set; }
    }

    public enum GenderEnum
    {
        男,
        女
    }
}
