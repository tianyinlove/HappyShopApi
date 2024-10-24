using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HappyShop.Comm;
using HappyShop.Data;
using HappyShop.Model;
using HappyShop.Request;
using HappyShop.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Utility.NetCore;
using Utility.NetLog;

namespace HappyShop.Api.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class HomeController : ControllerBase
    {
        private readonly IUserInfoService _userInfoService;
        private readonly IMyFollowService _myFollowService;

        /// <summary>
        ///
        /// </summary>
        public HomeController(IUserInfoService userInfoService,
            IMyFollowService myFollowService)
        {
            _userInfoService = userInfoService;
            _myFollowService = myFollowService;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HttpPost]
        public async Task<string> ReceiveMessage(string msg_signature, string timestamp, string nonce, string echostr)
        {
            Logger.WriteLog(Utility.Constants.LogLevel.Debug, "微信传入参数", new { msg_signature, timestamp, nonce, echostr });

            //企业微信后台开发者设置的token, corpID, EncodingAESKey
            string sToken = "wKB3j3POS33LqEy2vTEhiTzh";
            string sCorpID = "ww1c5ca8f9af6164f4";
            string sEncodingAESKey = "sV96P5yUsG64zuAPQqLDgayL4jdvx7HIDJrlMf8jIWf";

            var wxcpt = new Tencent.WXBizMsgCrypt(sToken, sEncodingAESKey, sCorpID);

            int ret = 0;
            string result = "";
            if (string.Equals(Request.Method, "POST", StringComparison.OrdinalIgnoreCase))
            {
                string sReqData;
                using (var streamReader = new StreamReader(Request.Body))
                {
                    sReqData = await streamReader.ReadToEndAsync();
                }
                Logger.WriteLog(Utility.Constants.LogLevel.Debug, "微信接收数据", sReqData);
                ret = wxcpt.DecryptMsg(msg_signature, timestamp, nonce, sReqData, ref result);
                if (ret != 0)
                {
                    return "fail";
                }

                Logger.WriteLog(Utility.Constants.LogLevel.Debug, "微信解密数据", result);

                var gatewayData = new GatewayData(DataFormat.Xml, result);
                var content = gatewayData.GetValue<string>("Content");
                if (!string.IsNullOrEmpty(content))
                {
                    var message = "";
                    if (int.TryParse(content, out int id))
                    {
                        message = await _userInfoService.GetStockMessageByIdAsync(id);
                    }
                    else
                    {
                        string[] flags = new string[] { "|", ":", "_", "@", "$", "#" };
                        var contentList = content.Split(flags, StringSplitOptions.RemoveEmptyEntries);
                        if (contentList != null && contentList.Length > 1)
                        {
                            // 关注/取消关注@股票池名称@股票代码
                            message = "消息异常";
                            if (contentList[0] == "关注")
                            {
                                var saveResult = false;
                                if (contentList.Length == 2)
                                {
                                    saveResult = await _myFollowService.SaveUpdate(gatewayData.GetValue<string>("FromUserName"), contentList[1], "", true);
                                }
                                if (contentList.Length > 2)
                                {
                                    saveResult = await _myFollowService.SaveUpdate(gatewayData.GetValue<string>("FromUserName"), contentList[1], contentList[2], true);
                                }
                                message = saveResult ? "关注成功" : "关注失败";
                            }
                            else if (contentList[0] == "取消关注")
                            {
                                var saveResult = false;
                                if (contentList.Length == 2)
                                {
                                    saveResult = await _myFollowService.SaveUpdate(gatewayData.GetValue<string>("FromUserName"), contentList[1], "", false);
                                }
                                if (contentList.Length > 2)
                                {
                                    saveResult = await _myFollowService.SaveUpdate(gatewayData.GetValue<string>("FromUserName"), contentList[1], contentList[2], false);
                                }
                                message = saveResult ? "取消关注成功" : "取消关注失败";
                            }
                        }
                        else
                        {
                            message = await _userInfoService.GetStockTradeByNameAsync(content);
                        }
                    }
                    string sRespData = $"<xml><ToUserName><![CDATA[{gatewayData.GetValue<string>("FromUserName")}]]></ToUserName><FromUserName><![CDATA[{sCorpID}]]></FromUserName><CreateTime>{gatewayData.GetValue<string>("CreateTime")}</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[{message}]]></Content><MsgId>{gatewayData.GetValue<string>("MsgId")}</MsgId><AgentID>{gatewayData.GetValue<string>("AgentID")}</AgentID></xml>";
                    ret = wxcpt.EncryptMsg(sRespData, timestamp, nonce, ref result);
                    if (ret != 0)
                    {
                        return "fail";
                    }
                }
            }
            else
            {
                ret = wxcpt.VerifyURL(msg_signature, timestamp, nonce, echostr, ref result);
                if (ret != 0)
                {
                    return "fail";
                }
            }
            return result;
        }
    }
}