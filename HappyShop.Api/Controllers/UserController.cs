using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HappyShop.Model;
using HappyShop.Request;
using HappyShop.Response;
using HappyShop.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace HappyShop.Api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserInfoService _userInfoService;

        /// <summary>
        /// 
        /// </summary>
        public UserController(ILogger<UserController> logger,
            IUserInfoService userInfoService)
        {
            _logger = logger;
            _userInfoService = userInfoService;
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ResultResponse<UserInfo>> Login([FromBody]LoginRequest request)
        {
            try
            {
                var user = await _userInfoService.LoginAsync(request);
                return new ResultResponse<UserInfo> { Detail = user, Result = new ResultStatus { Code = 0 } };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "用户登录异常", request);
                return new ResultResponse<UserInfo> { Result = new ResultStatus { Code = -1, Msg = ex.Message } };
            }
        }

        /// <summary>
        /// 注册用户
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ResultResponse<UserInfo>> Register([FromBody]LoginRequest request)
        {
            try
            {
                var user = await _userInfoService.RegisterAsync(request);
                return new ResultResponse<UserInfo> { Detail = user, Result = new ResultStatus { Code = 0 } };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "用户注册异常", request);
                return new ResultResponse<UserInfo> { Result = new ResultStatus { Code = -1, Msg = ex.Message } };
            }
        }

    }
}