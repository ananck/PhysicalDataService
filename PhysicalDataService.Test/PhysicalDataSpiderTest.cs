using Core.Model.PhysicalDataSpiderData;
using Core.Service;
using Core.Service.SpiderServers;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Diagnostics;
using System.Net.Http;

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
        /// ��¼����
        /// </summary>
        [Test]
        public void LoginTest()
        {
            PhysicalDataSpider  physicalDataSpider = new PhysicalDataSpider();
            var result = physicalDataSpider.Login("zq523911141@qq.com", "Hou123456");
            //Console.WriteLine("222222222222");
            Assert.IsTrue(result, "��¼ʧ��!");
        }
        [Test]
        public void Register()
        {
            PhysicalDataSpider physicalDataSpider = new PhysicalDataSpider();
            var result = physicalDataSpider.RandomRegister();
            //Console.WriteLine("222222222222");
            //Assert.IsTrue(result, "��¼ʧ��!");
        }
        [Test]
        public void RunTest()
        {

            SaveDataService saveDataService = new SaveDataService();
            PhysicalDataSpider physicalDataSpider = new PhysicalDataSpider();
            physicalDataSpider.DataHand += (dataInfo , isSaveManufacture) =>
            {
                saveDataService.SaveData(dataInfo, isSaveManufacture);
                foreach (var item in dataInfo.PhysicalInfos)
                {
                    
                    Console.WriteLine($"{item.Key}");
                }
               
            };
            physicalDataSpider.Run("17111508082", "Zsyxgs83593420");
            //Assert.IsTrue(result, "��¼ʧ��!");
        }
        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="spiderDataInfo"></param>
        public void SaveData(SpiderDataInfo spiderDataInfo)
        {
            AppDomain.CurrentDomain.GetAssemblies();
            AppDomain.CurrentDomain.AssemblyLoad += (sender, args) =>
            {
                var assembly = args.LoadedAssembly;
            };
        }


    }
}