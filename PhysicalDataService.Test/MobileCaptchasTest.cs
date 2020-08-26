using Core.Service;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhysicalDataService.Test
{
    /// <summary>
    /// 手机验证码测试
    /// </summary>
    public class MobileCaptchasTest
    {
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        [Test]
        [TestCase("ananck", "Hou123456")]
        public void LoginTest(string user ,string password)
        {
            MobileCaptchas mobileCaptchas = new MobileCaptchas();
            var isLogin = mobileCaptchas.Login(user, password);
            Assert.IsTrue(isLogin);
        }
        [Test]
        [TestCase("ananck", "Hou123456","337")]
        public void GetPhoneTest(string user, string password,string sid)
        {
            MobileCaptchas mobileCaptchas = new MobileCaptchas();
            var isLogin = mobileCaptchas.Login(user, password);
            var phone = mobileCaptchas.GetPhone(sid);

        }
        [Test]
        [TestCase("ananck", "Hou123456", "337")]
        public void GetCAPTCHATest(string user, string password, string sid)
        {
            MobileCaptchas mobileCaptchas = new MobileCaptchas();
            var isLogin = mobileCaptchas.Login(user, password);
            var phone = mobileCaptchas.GetPhone(sid);
            var cAPTCHA = mobileCaptchas.GetCAPTCHA(sid, phone);

        }
        [Test]
        [TestCase("ananck", "Hou123456", "337")]
        public void WaitCAPTCHATest(string user, string password, string sid)
        {
            MobileCaptchas mobileCaptchas = new MobileCaptchas();
            var isLogin = mobileCaptchas.Login(user, password);
            var phone = mobileCaptchas.GetPhone(sid);
            var cAPTCHA = mobileCaptchas.WaitCAPTCHA(sid, phone,5);

        }
    }
}
