using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;

namespace Core.Service
{
    /// <summary>
    /// 手机验证码类
    /// </summary>
    public class MobileCaptchas
    {
        HttpClient httpClient;
        private string token;

        public MobileCaptchas()
        {

            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("http://www.v8jm.com");
        }
        public bool IsLogin { get; set; } = false;
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="user">用户名</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public bool Login(string user, string password)
        {
            var content = httpClient.GetAsync($"api/login?userName={user}&password={password}").Result.Content.ReadAsStringAsync().Result;
            var msgs = content.Split("|");
            if (msgs[0] == "0")
            {
                token = msgs[2];
                IsLogin = true;
                return true;
            }
            return false;
        }
        /// <summary>
        /// 获取手机
        /// </summary>
        /// <param name="sid">获取手机</param>
        /// <returns></returns>
        public string GetPhone(string sid)
        {
            IsLogined();
            var content = httpClient.GetAsync($"api/getPhone?sid={sid}&token={token}").Result.Content.ReadAsStringAsync().Result;
            var msgs = content.Split("|");
            return msgs[1];

        }
        public string GetCAPTCHA(string sid, string phone)
        {
            IsLogined();
            var content = httpClient.GetAsync($"api/getMessage?sid={sid}&phone={phone}&token={token}").Result.Content.ReadAsStringAsync().Result;
            var msgs = content.Split("|");
            if (msgs[0] == "0")
            {

                return msgs[2];
            }
            return "";

        }
        /// <summary>
        /// 等待验证码
        /// </summary>
        /// <param name="sid">项目ID</param>
        /// <param name="phone">手机</param>
        /// <param name="second">等待多少秒</param>
        /// <returns></returns>
        public string WaitCAPTCHA(string sid, string phone, int second)
        {
            for (int i = 0; i < second; i++)
            {
                var CAPTCHA = GetCAPTCHA(sid, phone);
                if (!string.IsNullOrWhiteSpace(CAPTCHA))
                {
                    return CAPTCHA;
                }
                Thread.Sleep(1000);
            }
            throw new TimeoutException("获取验证码超时!");
        }

        /// <summary>
        /// 检测是否已登录
        /// </summary>
        private void IsLogined()
        {
            if (!IsLogin)
            {
                throw new Exception("请登录成功后尝试!");
            }
        }

        internal bool CancelRecv(string sid, string phone)
        {
            IsLogined();
            var content = httpClient.GetAsync($"api/cancelRecv?sid={sid}&phone={phone}&token={token}").Result.Content.ReadAsStringAsync().Result;
            var msgs = content.Split("|");
            if (msgs[0] == "0")
            {

                return true;
            }
            return false;
        }
    }
}
