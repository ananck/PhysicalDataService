using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Core.Common;
using Core.Model;
using Core.Model.PhysicalDataSpiderData;
using HtmlAgilityPack;
using Newtonsoft.Json.Linq;
using Polly;

namespace Core.Service.SpiderServers
{
    /// <summary>
    /// 物性数据采集器
    /// </summary>
    public class PhysicalDataSpider
    {


        CookieContainer cookieContainer = new CookieContainer();
        public PhysicalDataSpider()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);



        }
        public event Action<SpiderDataInfo,bool> DataHand;
        /// <summary>
        /// 执行爬虫
        /// </summary>
        /// <returns></returns>
        public void Run(string username, string password)
        {
            /*if (!Login(username, password))
            {
                throw new Exception("登录失败!");
            }*/
            var content = HttpGet("https://www.17suzao.com/plastic").Content.ReadAsStringAsync().Result;
            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(content);
            var htmlnodes = htmlDocument.DocumentNode.SelectNodes("//*[@id='data_tbody']/tr");
            if (htmlnodes is null)
            {
                return;
            }
            //创建制造商列表
            List<string> MakerShortNames = new List<string>();
            Regex regex = new Regex(@"(?<MakerShortName>.*)\n\s*?(?<MakerEnglishName>.*)");
            SqlLiteContext sqlLiteContext = new SqlLiteContext();
            var locationRecords = sqlLiteContext.LocationRecords.FirstOrDefault();
            var lastx = locationRecords?.LastKey ?? "0";
            var lasti = locationRecords?.LastIndex ?? "0";
            bool isFast = true;
            foreach (var node in htmlnodes)
            {

                //SpiderDataInfo spiderDataInfo = new SpiderDataInfo();
                string temp = System.Web.HttpUtility.HtmlDecode(node.SelectSingleNode("td[1]")?.InnerText.Trim() ?? "");
                string makerShortName;
                string makerEnglishName;
                var match = regex.Match(temp);
                if (!match.Success)
                {
                    if (string.IsNullOrWhiteSpace(temp))
                    {
                        throw new Exception("制造商名称无效");
                    }
                    makerShortName = temp;
                    makerEnglishName = temp;
                }
                else
                {

                    makerShortName = match.Groups["MakerShortName"].Value.Trim();

                    makerEnglishName = match.Groups["MakerEnglishName"].Value.Trim();


                }
                if (!MakerShortNames.Contains(makerShortName))
                {
                    MakerShortNames.Add(makerShortName);
                }

            }
           

            for (int x = isFast ?Convert.ToInt32(lastx):0; x < MakerShortNames.Count; x++)
            {



                int total = 0;
                int nowPage = 1;
                int totalPage = 1;
                bool IsFast = true;
                do
                {

                    HttpRequestMessage httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.Headers.Add("x-requested-with", "XMLHttpRequest");
                    var keyContent = HttpGet(@$"https://www.17suzao.com/plastic/search?v=2&key={System.Web.HttpUtility.UrlEncode(MakerShortNames[x])}&start=0&length={total}&draw=1", httpRequestMessage).Content.ReadAsStringAsync().Result;
                    JObject data = JObject.Parse(keyContent);
                    if (IsFast)
                    {
                        total = data.SelectToken("recordsTotal").Value<int>();
                        IsFast = false;
                        totalPage = total / total;
                        continue;
                    }

                    var infoDatas = data.GetValue("data") as JArray;
                    for (int i = isFast?Convert.ToInt32(lasti):0; i < infoDatas.Count; i++)

                    {
                        var Retry = Policy.Handle<NotLoginException>().Retry(5, (ex, retryCount) =>
                        {
                            Policy.Handle<TimeoutException>().Retry(5).Execute(() =>
                            {
                                var userifno = RandomRegister();
                                username = userifno.UserName;
                                password = userifno.PassWord;
                            });
                            /* if (!Login(username, password))
                             {
                                 throw new Exception("登录失败!");
                             }*/
                        });
                        var Retry2 = Policy.Handle<AccountClosureException>().Retry(5, (ex, retryCount) =>
                        {
                            Policy.Handle<TimeoutException>().Retry(5).Execute(() =>
                            {
                                var userifno = RandomRegister();
                                username = userifno.UserName;
                                password = userifno.PassWord;
                            });
                        });
                        var commonlyRetry = Policy.Handle<ErroDataException>().Retry(1);
                        Policy.Wrap(Retry, Retry2, commonlyRetry).Execute(() =>
                        {
                            SpiderDataInfo spiderDataInfo = new SpiderDataInfo();

                            spiderDataInfo.MakerShortName = infoDatas[i][0][1].Value<string>();
                            spiderDataInfo.MakerEnglishName = infoDatas[i][0][2].Value<string>();
                            spiderDataInfo.TradeName = infoDatas[i][1][1].Value<string>();
                            spiderDataInfo.kind = infoDatas[i][2][1].Value<string>();
                            spiderDataInfo.Mark = infoDatas[i][3][1].Value<string>();
                            spiderDataInfo.MaterialIdentity = infoDatas[i][4].Value<string>();
                            string markID = infoDatas[i][3][0].Value<string>();
                            if (sqlLiteContext.RepeatingTables.FirstOrDefault(x => x.DetailID == markID) != null)
                            {
                                return;
                            }
                            string detailsUri = $"https://www.17suzao.com/plastic/detail?id={markID}";
                            spiderDataInfo.DetailsUrl = detailsUri;
                            string detailsContent = HttpGet(detailsUri).Content.ReadAsStringAsync().Result;
                            HtmlDocument detailsDoc = new HtmlDocument();
                            detailsDoc.LoadHtml(detailsContent);
                            string pageIdentity = detailsDoc.DocumentNode.SelectSingleNode("//*[@id='w-nav']/a[1]").InnerText;
                            if (pageIdentity == "登录")
                            {
                                throw new NotLoginException("登录信息过期!");
                            }
                            if (detailsContent.IndexOf("您没有足够的积分可用</p>") != -1)
                            {
                                throw new AccountClosureException("账号已经失效!");
                            }
                            spiderDataInfo.MakerFullName = detailsDoc.DocumentNode.SelectSingleNode("//table[tr/th[text()='产品描述']]/tr[td[text()='制造商']]/td[2]")?.InnerText;
                            spiderDataInfo.MaterialIdentity = System.Web.HttpUtility.HtmlDecode(detailsDoc.DocumentNode.SelectSingleNode("//table[tr/th[text()='产品描述']]/tr[td[text()='材料标示']]/td[2]")?.InnerText);
                            spiderDataInfo.Color = detailsDoc.DocumentNode.SelectSingleNode("//table[tr/th[text()='产品描述']]/tr[td[text()='颜色']]/td[2]")?.InnerText;
                            spiderDataInfo.FileNumber = detailsDoc.DocumentNode.SelectSingleNode("//table[tr/th[text()='产品描述']]/tr[td[text()='UL档案号']]/td[2]")?.InnerText.Trim();
                            spiderDataInfo.Purpose = detailsDoc.DocumentNode.SelectSingleNode("//table[tr/th[text()='产品描述']]/tr[td[text()='用途']]/td[2]")?.InnerText;
                            spiderDataInfo.Characteristic = detailsDoc.DocumentNode.SelectSingleNode("//table[tr/th[text()='产品描述']]/tr[td[text()='材料特性']]/td[2]")?.InnerText;
                            spiderDataInfo.Shape = detailsDoc.DocumentNode.SelectSingleNode("//table[tr/th[text()='产品描述']]/tr[td[text()='材料形状']]/td[2]")?.InnerText;
                            spiderDataInfo.ProcessingMethod = detailsDoc.DocumentNode.SelectSingleNode("//table[tr/th[text()='产品描述']]/tr[td[text()='加工方式']]/td[2]")?.InnerText;
                            var tableNodes = detailsDoc.DocumentNode.SelectNodes("//div[@class='data']/table") as HtmlNodeCollection;
                            if (tableNodes == null)
                            {
                                return;
                            }
                            foreach (var tableNode in tableNodes)
                            {
                                var trNodes = tableNode.SelectNodes("tr");
                                List<PhysicalInfo> physicalInfoList = new List<PhysicalInfo>();
                                string physicalName = null;
                                for (int i = 0; i < trNodes.Count; i++)
                                {
                                    if (i == 0)
                                    {
                                        physicalName = trNodes[i].SelectSingleNode("th[1]").InnerText;
                                        continue;
                                    }
                                    physicalInfoList.Add(new PhysicalInfo()
                                    {
                                        TypeName = trNodes[i].SelectSingleNode("td[1]").InnerText.Trim(),
                                        Condition = trNodes[i].SelectSingleNode("td[2]").InnerText.Trim(),
                                        TestStandard = trNodes[i].SelectSingleNode("td[3]").InnerText.Trim(),
                                        Data = trNodes[i].SelectSingleNode("td[4]").InnerText.Trim(),
                                        Unit = trNodes[i].SelectSingleNode("td[5]").InnerText.Trim()

                                    });
                                }
                                if (!string.IsNullOrWhiteSpace(physicalName))
                                {
                                    spiderDataInfo.PhysicalInfos.Add(physicalName, physicalInfoList);
                                }

                            }
                            bool isSaveManufacture = true;
                            if (sqlLiteContext.RepeatingTables.FirstOrDefault(x=>x.MakerFullName == spiderDataInfo.MakerFullName)!=null)
                            {
                                isSaveManufacture = false;
                            }
                            DataHand?.Invoke(spiderDataInfo, isSaveManufacture);
                            System.Diagnostics.Debug.WriteLine(spiderDataInfo.MakerFullName);

                            if (locationRecords == null)
                            {
                                locationRecords = new LocationRecords()
                                {
                                    ID = 0,
                                    LastIndex = i.ToString(),
                                    LastKey = x.ToString()
                                };
                                sqlLiteContext.LocationRecords.Add(locationRecords);
                            }
                            else
                            {
                                locationRecords.LastIndex = i.ToString();
                                locationRecords.LastKey = x.ToString();
                                sqlLiteContext.LocationRecords.Update(locationRecords);
                            }
                            sqlLiteContext.RepeatingTables.Add(new RepeatingTable()
                            {
                                DetailID = markID,
                                ItemInfo = infoDatas[i].ToString(),
                                Detail = detailsContent,
                                MakerFullName= spiderDataInfo.MakerFullName
                            });
                            sqlLiteContext.SaveChanges();
                        });


                        //spiderDataInfo.PhysicalInfos

                    }
                    nowPage++;
                } while (nowPage <= totalPage);


                isFast = false;
            }


        }

        public UserInfo RandomRegister()
        {
            cookieContainer = new CookieContainer();
            HttpGet("https://www.17suzao.com/reg");
            byte[] VImage = HttpGet("https://www.17suzao.com/vendor/captcha?w=280&h=80").Content.ReadAsByteArrayAsync().Result;
            ImageCAPTCHA imageCAPTCHA = new ImageCAPTCHA("ananck", "Hou123456");
            MobileCaptchas mobileCaptchas = new MobileCaptchas();
            var isLogin = mobileCaptchas.Login("ananck", "Hou123456");
            string sid = "337";
            var phone = mobileCaptchas.GetPhone(sid);
            var captcha = imageCAPTCHA.Ocr(VImage);
            ///验证手机是否使用
            var tempStr = HttpPost("https://www.17suzao.com/reg/checkCellphone", new FormUrlEncodedContent(new Dictionary<string, string>()
            {
                ["cellphone"] = phone
            })).Content.ReadAsStringAsync().Result;
            if (tempStr != "true")
            {
                throw new Exception("手机已被使用!");
            }
            //发送手机验证码到指定手机
            tempStr = HttpPost("https://www.17suzao.com/vendor/sendPhoneCaptcha", new FormUrlEncodedContent(new Dictionary<string, string>()
            {
                ["cellphone"] = phone,
                ["captcha"] = captcha,
                ["type"] = "reg"
            })).Content.ReadAsStringAsync().Result;
            //等待手机验证码
            var phoneCaptcha = mobileCaptchas.WaitCAPTCHA(sid, phone, 40);
            mobileCaptchas.CancelRecv(sid, phone);
            //提交手机验证码
            tempStr = HttpPost("https://www.17suzao.com/vendor/checkCaptcha?scene=reg", new FormUrlEncodedContent(new Dictionary<string, string>()
            {
                ["cellphone"] = phone,
                ["captcha"] = captcha,
                ["phone_captcha"] = phoneCaptcha
            })).Content.ReadAsStringAsync().Result;
            tempStr = HttpGet("https://www.17suzao.com/reg/byPhone").Content.ReadAsStringAsync().Result;
            string eMail = BuildEMail();
            //验证邮箱是否使用
            tempStr = HttpPost("https://www.17suzao.com/reg/checkEmail", new FormUrlEncodedContent(new Dictionary<string, string>()
            {
                ["email"] = eMail
            })).Content.ReadAsStringAsync().Result;
            //注册并提交用户信息
            string surname = NameUtility.GetSurname();
            Encoding gb = Encoding.GetEncoding("gb2312");

            //调用函数产生4个随机中文汉字编码
            object[] bytes = NameUtility.CreateRegionCode(2, true);
            string fame = string.Join("", bytes.Select(x => gb.GetString((byte[])Convert.ChangeType(x, typeof(byte[])))));
            string password = BuildPassWord();

            tempStr = HttpPost("https://www.17suzao.com/reg/byPhone", new FormUrlEncodedContent(new Dictionary<string, string>()
            {
                ["email"] = eMail,
                ["surname"] = surname,
                ["fame"] = fame,
                ["gender"] = "1",
                ["company"] = $"{surname + fame}注塑有限公司",
                ["password"] = password,
                ["password_rep"] = password
            })).Content.ReadAsStringAsync().Result;

            tempStr = HttpGet("https://www.17suzao.com/reg/byPhoneComplete").Content.ReadAsStringAsync().Result;

            tempStr = HttpGet("https://www.17suzao.com/").Content.ReadAsStringAsync().Result;
            if (tempStr.IndexOf(">退出</a>") == -1)
            {
                return default;
            }
            UserInfo userInfo = new UserInfo();
            userInfo.UserName = phone;
            userInfo.PassWord = password;
            File.AppendAllText("user.txt", $"{phone}----{password}\r\n");
            return userInfo;
        }

        private string BuildPassWord()
        {

            Random random = new Random();
            var rawPassword = "Zsyxgs" + random.Next(10000000, 99999999).ToString();
            return rawPassword;


        }

        /// <summary>
        /// 生成随机邮箱
        /// </summary>
        /// <returns></returns>
        private string BuildEMail()
        {
            Random random = new Random();

            return $"{random.Next(10000000, 99999999).ToString()}@163.com";
        }

        /// <summary>
        /// 登录目标网站
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public bool Login(string username, string password)
        {
            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
            cookieContainer = new CookieContainer();
            keyValuePairs.Add("username", username);
            keyValuePairs.Add("password", password);

            FormUrlEncodedContent formUrlEncodedContent = new FormUrlEncodedContent(keyValuePairs);
            var responseMessage = HttpPost("https://www.17suzao.com/login/ajax", formUrlEncodedContent);
            var content = responseMessage.Content.ReadAsStringAsync().Result;
            var state = content.ToJObject().GetValue("status").Value<int>();
            if (state == 1)
            {
                return true;
            }
            return false;
        }
        private HttpResponseMessage HttpGet(string requestUri, HttpRequestMessage httpRequestMessage = null)
        {
            HttpClientHandler httpClientHandler = new HttpClientHandler();
            httpClientHandler.AllowAutoRedirect = true;
            httpClientHandler.UseProxy = false;
            httpClientHandler.UseCookies = true;
            httpClientHandler.CookieContainer = cookieContainer;
            HttpClient httpClient = new HttpClient(httpClientHandler);

            HttpRequestMessage _httpRequestMessage;
            if (httpRequestMessage != null)
            {
                _httpRequestMessage = httpRequestMessage;
            }
            else
            {
                _httpRequestMessage = new HttpRequestMessage();
            }
            _httpRequestMessage.Method = HttpMethod.Get;
            _httpRequestMessage.RequestUri = new Uri(requestUri);

            var httpResponseMessage = httpClient.SendAsync(_httpRequestMessage).ConfigureAwait(false).GetAwaiter().GetResult();
            HTTPSetCookies(httpResponseMessage);
            return httpResponseMessage;
        }
        private HttpResponseMessage HttpPost(string requestUri, HttpContent content, HttpRequestMessage httpRequestMessage = null)
        {
            HttpClientHandler httpClientHandler = new HttpClientHandler();
            httpClientHandler.AllowAutoRedirect = true;
            httpClientHandler.UseProxy = true;
            httpClientHandler.UseCookies = true;
            httpClientHandler.CookieContainer = cookieContainer;
            HttpClient httpClient = new HttpClient(httpClientHandler);
            HttpRequestMessage _httpRequestMessage;
            if (httpRequestMessage != null)
            {
                _httpRequestMessage = httpRequestMessage;
            }
            else
            {
                _httpRequestMessage = new HttpRequestMessage();
            }
            _httpRequestMessage.Method = HttpMethod.Post;

            _httpRequestMessage.Content = content;
            _httpRequestMessage.RequestUri = new Uri(requestUri);
            var httpResponseMessage = httpClient.SendAsync(_httpRequestMessage).Result;
            HTTPSetCookies(httpResponseMessage);
            return httpResponseMessage;
        }
        /// <summary>
        /// 设置Cookie到CookieContainer
        /// </summary>
        /// <param name="httpResponseMessage">响应消息</param>
        private void HTTPSetCookies(HttpResponseMessage httpResponseMessage)
        {
            httpResponseMessage.Headers.TryGetValues("Set-Cookie", out var nowCookiesStr);

            if (nowCookiesStr is null)
            {
                return;
            }
            foreach (var cookieStr in nowCookiesStr)
            {
                cookieContainer.SetCookies(httpResponseMessage.RequestMessage.RequestUri, cookieStr);
            }
        }

    }
}
