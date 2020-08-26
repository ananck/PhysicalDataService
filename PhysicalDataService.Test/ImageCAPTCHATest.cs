using Core.Service;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace PhysicalDataService.Test
{
    public class ImageCAPTCHATest
    {
        [Test]
        [TestCase("ananck","Hou123456")]
        public void OcrTest(string user,string passWord) 
        {
            HttpClient httpClient = new HttpClient();
            byte[] VImage = httpClient.GetAsync("https://www.17suzao.com/vendor/captcha?w=280&h=80").Result.Content.ReadAsByteArrayAsync().Result;
            ImageCAPTCHA imageCAPTCHA = new ImageCAPTCHA(user,passWord);
            var captcha = imageCAPTCHA.Ocr(VImage);
            using (System.IO.FileStream fileStream = new System.IO.FileStream("test.gif", System.IO.FileMode.Create))
            {
                fileStream.Write(VImage, 0, VImage.Length);
            };
            
        }
    }
}
