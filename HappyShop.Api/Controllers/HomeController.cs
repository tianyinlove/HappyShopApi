﻿using System;
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
using Utility.NetLog;

namespace HappyShop.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly IUserInfoService _userInfoService;
        public HomeController(IUserInfoService userInfoService)
        {
            _userInfoService = userInfoService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HttpPost]
        public object Get()
        {
            Logger.WriteLog(Utility.Constants.LogLevel.Info, "测试日志");
            return new ApiResult<string>("ok");
        }
    }
}
