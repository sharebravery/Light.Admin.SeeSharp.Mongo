using AutoMapper;
using Light.Admin.Database;
using Light.Admin.Dtos;
using Light.Admin.Mongo;
using Light.Admin.Mongo.Basics;
using Light.Admin.Mongo.IServices;
using Light.Admin.Mongo.Services;
using Light.Admin.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Xml.Linq;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace Light.Admin.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService accountService;

        public AccountController(
            IAccountService accountService
            )
        {
            this.accountService = accountService;
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public async Task SignIn(LoginViewModel model)
        {

            var user = await accountService.ValidateUser(model.Account, model.Password);

            if (user != null)
            {
                var token = await accountService.GetToken(user);
            }

        }
    }
}
