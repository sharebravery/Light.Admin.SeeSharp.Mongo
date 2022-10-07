using AutoMapper;
using Light.Admin.Database;
using Light.Admin.Mongo;
using Light.Admin.Mongo.Basics;
using Light.Admin.Mongo.IServices;
using Light.Admin.Mongo.Utils;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Light.Admin.Mongo.Services
{
    public class AccountService : IAccountService
    {
        private readonly IMapper mapper;
        readonly IMongoCollection<User> userCollection;
        private readonly IMongoDbContext db;
        private readonly JwtSettings jwtSettings;
        private readonly IHttpContextAccessor httpContextAccessor;



        public AccountService(
            IMapper mapper,
            IMongoDbContext db,
            IOptions<JwtSettings> options,
            IHttpContextAccessor httpContextAccessor
            )
        {
            this.db = db;
            this.userCollection = db.GetCollection<User>(nameof(User));
            this.mapper = mapper;
            this.httpContextAccessor = httpContextAccessor;
            this.jwtSettings = options.Value;
        }

        /// <summary>
        /// 验证用户是否存在
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<User> ValidateUser(string account, string password)
        {
            var user = (await userCollection.FindAsync(p => p.UserName == account))
                .FirstOrDefault();

            if (user == null)
            {
                throw new InvalidOperationException("账号错误");
            }

            if (user.PasswordHash != Hash.Sha256(password))
            {
                throw new InvalidOperationException("密码错误");
            }

            return user;
        }

        /// <summary>
        /// 生成Token
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<string> GetToken(User user)
        {
            // 1. 定义需要使用到的Claims,即payload有效载荷
            var claims = new Claim[]
           {
                        new Claim(ClaimTypes.Name,user.UserName),
                        new Claim(ClaimTypes.Email,user.Email)
             };

            //var principal = new ClaimsPrincipal(new ClaimsIdentity[]
            //    {
            //        new ClaimsIdentity(claims, "Default")
            //    });

            //await httpContextAccessor.HttpContext.SignInAsync(principal);

            // 2. 从 appsettings.json 中读取SecretKey,生成对称秘钥
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey));

            // 3. 选择加密算法,生成Credentials
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // 5. 根据以上，生成token
            var jwtSecurityToken = new JwtSecurityToken(jwtSettings.Issuer,
                jwtSettings.Audience,
                claims,
                DateTime.Now,
                DateTime.Now.AddMinutes(10),
                creds);

            // 6. 将token变为string
            string token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            return token;
        }
    }
}
