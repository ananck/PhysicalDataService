using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Core.Service
{
    public class ImageCAPTCHA
    {
        private string _user;
        private string _password;
        HttpClient _httpClient;
        public ImageCAPTCHA(string user, string password)
        {
            _user = user;

            _password = password;
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("http://api.ttshitu.com/");
        }
        string _typeId = "7";
        // 超时等待，单位：秒
        string timeOut = "60";
        public string Ocr(byte[] image)
        {
            using (MultipartFormDataContent multipartFormDataContent = new MultipartFormDataContent()) 
            {
                var byteArrayContent_username = new ByteArrayContent(Encoding.UTF8.GetBytes(_user));
                byteArrayContent_username.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                {
                    Name = "username"
                };
                multipartFormDataContent.Add(byteArrayContent_username);

                var byteArrayContent_PassWord = new ByteArrayContent(Encoding.UTF8.GetBytes(_password));
                byteArrayContent_PassWord.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                {
                    Name = "password"
                };
                multipartFormDataContent.Add(byteArrayContent_PassWord);

                var byteArrayContent_typeid = new ByteArrayContent(Encoding.UTF8.GetBytes(_typeId));
                byteArrayContent_typeid.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                {
                    Name = "typeid"
                };
                multipartFormDataContent.Add(byteArrayContent_typeid);

                var byteArrayContent_timeout = new ByteArrayContent(Encoding.UTF8.GetBytes(timeOut));
                byteArrayContent_timeout.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                {
                    Name = "timeout"
                };
                multipartFormDataContent.Add(byteArrayContent_timeout);

                var byteArrayContent_image = new ByteArrayContent(image);
                byteArrayContent_image.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                {
                    Name = "image",
                    FileName= "i.gif"
                };
                multipartFormDataContent.Add(byteArrayContent_image);
                string json = _httpClient.PostAsync("create.json", multipartFormDataContent).Result.Content.ReadAsStringAsync().Result;
                Newtonsoft.Json.Linq.JObject jObject = Newtonsoft.Json.Linq.JObject.Parse(json);
                string code = jObject.SelectToken("data.result").Value<string>();
                return code;
            }
                
            
        }
    }
}
