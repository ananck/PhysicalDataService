using Core.Model.PhysicalDataSpiderData;
using Core.Model.SaveModel;
using SnowFlakeID;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;

namespace Core.Service
{
    /// <summary>
    /// 保存数据服务
    /// </summary>
    public class SaveDataService
    {
        public SaveDataService()
        {

        }
        private const long datacenterId = 5;
        private const long machineId = 10;
        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="spiderDataInfo"></param>
        public void SaveData(SpiderDataInfo spiderDataInfo, bool isSaveManufacture = true)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("http://i.iplascloud.com/material-library");
            SnowFlake snowFlake = new SnowFlake(datacenterId, machineId);

            long id = snowFlake.nextId();
            Manufacture manufacture = new Manufacture()
            {
                id = id,
                englishName = spiderDataInfo.MakerEnglishName,
                fullName = spiderDataInfo.MakerFullName,
                shortName = spiderDataInfo.MakerShortName

            };
            long maufactureId = snowFlake.nextId();


            Commodity commodity = new Commodity()
            {
                id = maufactureId,
                kind = spiderDataInfo.kind,
                mark = spiderDataInfo.Mark,
                materialIdentity = spiderDataInfo.MaterialIdentity,
                maufactureId = id,
                tradeName = spiderDataInfo.TradeName
            };
            MaterialInfo materialInfo = new MaterialInfo()
            {
                id = maufactureId,
                testingStandard = null,
                manufacturerForShort = spiderDataInfo.MakerShortName,
                manufacturerForEnglish = spiderDataInfo.MakerEnglishName,
                materialIdentity = spiderDataInfo.MaterialIdentity,
                mark = spiderDataInfo.Mark,
                characteristic = spiderDataInfo.Characteristic,
                color = spiderDataInfo.Color,
                fn = spiderDataInfo.FileNumber,
                kind = spiderDataInfo.kind,
                manufacturerFullName = spiderDataInfo.MakerFullName,
                otherCertificate = spiderDataInfo.ProductInfo?.OtherCertificates,
                processingMethod = spiderDataInfo.ProcessingMethod,
                purpose = spiderDataInfo.Purpose,
                shape = spiderDataInfo.Shape,
                tradeName = spiderDataInfo.TradeName
            };
            if (spiderDataInfo.PhysicalInfos != null)
            {
                foreach (var physicalInfo in spiderDataInfo.PhysicalInfos)
                {
                    switch (physicalInfo.Key)
                    {
                        case "物理性能":
                            materialInfo.physicsProperties = new List<physicsPropertie>();
                            foreach (var item in physicalInfo.Value)
                            {
                                materialInfo.physicsProperties.Add(new physicsPropertie()
                                {
                                    condition = item.Condition,
                                    data = item.Data,
                                    propertyName = item.TypeName,
                                    testingStandard = item.TestStandard,
                                    unit = item.Unit
                                });
                            }
                            break;
                        case "硬度":
                            materialInfo.hardnessProperties = new List<hardnessPropertie>();
                            foreach (var item in physicalInfo.Value)
                            {
                                materialInfo.hardnessProperties.Add(new hardnessPropertie()
                                {
                                    condition = item.Condition,
                                    data = item.Data,
                                    propertyName = item.TypeName,
                                    testingStandard = item.TestStandard,
                                    unit = item.Unit
                                });
                            }
                            break;
                        case "机械性能":
                            materialInfo.machineProperties = new List<machinePropertie>();
                            foreach (var item in physicalInfo.Value)
                            {
                                materialInfo.machineProperties.Add(new machinePropertie()
                                {
                                    condition = item.Condition,
                                    data = item.Data,
                                    propertyName = item.TypeName,
                                    testingStandard = item.TestStandard,
                                    unit = item.Unit
                                });
                            }
                            break;
                        case "光学性能":
                            /*materialInfo.machineProperties = new List<machinePropertie>();
                            foreach (var item in physicalInfo.Value)
                            {
                                materialInfo.machineProperties.Add(new machinePropertie()
                                {
                                    condition = item.Condition,
                                    data = item.Data,
                                    propertyName = item.TypeName,
                                    testingStandard = item.TestStandard,
                                    unit = item.Unit
                                });
                            }*/
                            break;
                        case "阻燃性":
                            materialInfo.flameProperties = new List<flamePropertie>();
                            foreach (var item in physicalInfo.Value)
                            {
                                materialInfo.flameProperties.Add(new flamePropertie()
                                {
                                    condition = item.Condition,
                                    data = item.Data,
                                    propertyName = item.TypeName,
                                    testingStandard = item.TestStandard,
                                    unit = item.Unit
                                });
                            }
                            break;
                        case "热性能":
                            materialInfo.temperatureProperties = new List<temperaturePropertie>();
                            foreach (var item in physicalInfo.Value)
                            {
                                materialInfo.temperatureProperties.Add(new temperaturePropertie()
                                {
                                    condition = item.Condition,
                                    data = item.Data,
                                    propertyName = item.TypeName,
                                    testingStandard = item.TestStandard,
                                    unit = item.Unit
                                });
                            }
                            break;
                        case "其他性能":

                            break;
                        case "电气性能":

                            break;
                        default:
                            break;
                    }
                }
            }
            List<Manufacture> manufactures = new List<Manufacture>() { manufacture };
            List<Commodity> commodities = new List<Commodity>() { commodity };
            List<MaterialInfo> materialInfos = new List<MaterialInfo>() { materialInfo };
            string result;
            if (isSaveManufacture)
            {
                result = httpClient.PostAsJsonAsync<List<Manufacture>>("http://i.iplascloud.com/material-library/elasticsearch/batchInsertManufacture", manufactures).Result.Content.ReadAsStringAsync().Result;
            }
            result = httpClient.PostAsJsonAsync<List<Commodity>>("http://i.iplascloud.com/material-library/elasticsearch/batchInsertCommodity", commodities).Result.Content.ReadAsStringAsync().Result;
            result = httpClient.PostAsJsonAsync<List<MaterialInfo>>("http://i.iplascloud.com/material-library/elasticsearch/batchInsertMaterialInfo", materialInfos).Result.Content.ReadAsStringAsync().Result;
            //httpClient.PostAsync("elasticsearch/batchInsertCommodity", jsonContent);
        }
    }
}
