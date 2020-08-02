using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Core.Common;
using Core.Model.PhysicalDataSpiderData;
using HtmlAgilityPack;
using Newtonsoft.Json.Linq;

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




        }

        /// <summary>
        /// 执行爬虫
        /// </summary>
        /// <returns></returns>
        public void Run(string username, string password)
        {
            if (!Login(username, password))
            {
                throw new Exception("登录失败!");
            }
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
            foreach (var MakerShortName in MakerShortNames)
            {


                HttpRequestMessage httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Headers.Add("x-requested-with", "XMLHttpRequest");
                int total = 0;
                int nowLocation = 1;
                do
                {
                    var keyContent = HttpGet(@$"https://www.17suzao.com/plastic/search?v=2&key={System.Web.HttpUtility.UrlEncode(MakerShortName)}&start=0&length=5&draw=1", httpRequestMessage).Content.ReadAsStringAsync().Result;
                    JObject data = JObject.Parse(keyContent);
                    var infoDatas = data.GetValue("data") as JArray;
                    foreach (var infoData in infoDatas)
                    {
                        SpiderDataInfo spiderDataInfo = new SpiderDataInfo();
                        spiderDataInfo.MakerShortName = infoData[0][1].Value<string>();
                        spiderDataInfo.MakerEnglishName = infoData[0][2].Value<string>();
                        spiderDataInfo.TradeName = infoData[1][1].Value<string>();
                        spiderDataInfo.kind= infoData[2][1].Value<string>();
                        spiderDataInfo.Mark = infoData[3][1].Value<string>();
                        spiderDataInfo.MaterialIdentity = infoData[4].Value<string>();
                        string markID = infoData[3][0].Value<string>();
                        string detailsUri = $"https://www.17suzao.com/plastic/detail?id={markID}";
                        string detailsContent = HttpGet(detailsUri).Content.ReadAsStringAsync().Result;
                        HtmlDocument detailsDoc = new HtmlDocument();
                        detailsDoc.LoadHtml(detailsContent);
                        spiderDataInfo.MakerFullName = detailsDoc.DocumentNode.SelectSingleNode("//table[tr/th[text()='产品描述']]/tr[td[text()='制造商']]/td[2]")?.InnerText;
                        spiderDataInfo.MaterialIdentity = detailsDoc.DocumentNode.SelectSingleNode("//table[tr/th[text()='产品描述']]/tr[td[text()='材料标示']]/td[2]")?.InnerText;
                        spiderDataInfo.MaterialIdentity = detailsDoc.DocumentNode.SelectSingleNode("//table[tr/th[text()='产品描述']]/tr[td[text()='颜色']]/td[2]")?.InnerText;
                        spiderDataInfo.MaterialIdentity = detailsDoc.DocumentNode.SelectSingleNode("//table[tr/th[text()='产品描述']]/tr[td[text()='UL档案号']]/td[2]")?.InnerText;
                        spiderDataInfo.MaterialIdentity = detailsDoc.DocumentNode.SelectSingleNode("//table[tr/th[text()='产品描述']]/tr[td[text()='用途']]/td[2]")?.InnerText;
                        spiderDataInfo.MaterialIdentity = detailsDoc.DocumentNode.SelectSingleNode("//table[tr/th[text()='产品描述']]/tr[td[text()='材料特性']]/td[2]")?.InnerText;
                        spiderDataInfo.MaterialIdentity = detailsDoc.DocumentNode.SelectSingleNode("//table[tr/th[text()='产品描述']]/tr[td[text()='材料形状']]/td[2]")?.InnerText;
                        spiderDataInfo.MaterialIdentity = detailsDoc.DocumentNode.SelectSingleNode("//table[tr/th[text()='产品描述']]/tr[td[text()='加工方式']]/td[2]")?.InnerText;
                    }
                } while (nowLocation >= total);



            }


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

            var httpResponseMessage = httpClient.SendAsync(_httpRequestMessage).Result;
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
