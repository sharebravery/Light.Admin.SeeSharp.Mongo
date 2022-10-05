using System.ComponentModel.DataAnnotations;

namespace Light.Admin.Dtos
{
    public class UserCreateDto
    {
        /// <summary>
        ///     用户名
        /// </summary>
        [Required(ErrorMessage = "用户名称不能为空")]
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        [Compare("Password",
        ErrorMessage = "密码与确认密码不一致，请重新输入.")]
        public string ConfirmPassword { get; set; }

        /// <summary>
        ///     电话号码
        /// </summary>
        public string? PhoneNumber { get; set; } = string.Empty;

        /// <summary>
        ///     密码
        /// </summary>
        public string? Email { get; set; }
    }
}
