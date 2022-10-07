using System.ComponentModel.DataAnnotations;

namespace Light.Admin.ViewModels
{
    public class LoginViewModel
    {
        /// <summary>
        /// 账户
        /// </summary>
        [Required]
        public string Account { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Required]
        public string Password { get; set; }

    }
}
