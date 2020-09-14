using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HappyShop.Comm;
using HappyShop.Model;
using HappyShop.Request;
using HappyShop.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Utility.NetCore;

namespace HappyShop.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly IUserInfoService _userInfoService;
        public WeatherForecastController(IUserInfoService userInfoService)
        {
            _userInfoService = userInfoService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResult<UserInfo>> Get()
        {
            var data = await _userInfoService.LoginAsync(new LoginRequest { });
            return new ApiResult<UserInfo>(data);
        }
    }
}
