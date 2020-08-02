using Core.Service.SpiderServers;
using NUnit.Framework;

namespace PhysicalDataService.Test
{
    [SingleThreaded]
    public class PhysicalDataSpiderTest
    {
      
       [SetUp]
        public void Setup()
        {
           
        }
        /// <summary>
        /// µÇÂ¼²âÊÔ
        /// </summary>
        [Test]
        public void LoginTest()
        {
            PhysicalDataSpider  physicalDataSpider = new PhysicalDataSpider();
            var result = physicalDataSpider.Login("zq522911141@qq.com", "Hou909222247");
            Assert.IsTrue(result, "µÇÂ¼Ê§°Ü!");
        }
        [Test]
        public void RunTest()
        {
            PhysicalDataSpider physicalDataSpider = new PhysicalDataSpider();
            physicalDataSpider.Run("zq522911141@qq.com", "Hou909222247");
            //Assert.IsTrue(result, "µÇÂ¼Ê§°Ü!");
        }
    }
}