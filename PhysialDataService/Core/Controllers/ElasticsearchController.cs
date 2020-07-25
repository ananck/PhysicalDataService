using Core.Common;
using Core.IService;
using Core.IService.ViewModel;
using Core.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Core.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    [Description("物性库接口")]
    public class ElasticsearchController : ControllerBase
    {
        private readonly IManufacturerService _manufacturerService;
        private readonly ITradeService _tradeService;

        public ElasticsearchController(IManufacturerService studentService, ITradeService tradeService)
        {
            _manufacturerService = studentService;
            _tradeService = tradeService;
        }

        [HttpPost]
        [Description("批量新增材料制造商接口")]
        public async Task<string> BatchInsertManufacture(List<ManufacturerModel> list)
        {
            try
            {
                var resultList = new List<Manufacturer>();
                foreach (var l in list)
                {
                    var manufacturer = new Manufacturer
                    {
                        FullName = l.FullName,
                        ShortName = l.ShortName,
                        EnglishName = l.EnglishName
                    };
                    await _manufacturerService.Insert(manufacturer);
                    resultList.Add(manufacturer);
                }


                return new Response() { data= resultList }.ToJson();
            }
            catch (Exception e)
            {
                return new Response() { code = 500, msg = e.Message }.ToJson();
            }
        }

        [HttpPost]
        [Description("批量新增材料商品名称")]
        public async Task<string> batchInsertCommodity(List<TradeModel> list)
        {
            try
            {
                var resultList = new List<Trade>();
                foreach (var l in list)
                {
                    var trade = new Trade
                    {
                        MaufactureId = l.MaufactureId,
                        TradeName = l.TradeName,
                        Kind = l.Kind,
                        Mark=l.Mark,
                        MaterialIdentity=l.MaterialIdentity
                    };
                    await _tradeService.Insert(trade);
                    resultList.Add(trade);
                }


                return new Response() { data = resultList }.ToJson();
            }
            catch (Exception e)
            {
                return new Response() { code = 500, msg = e.Message }.ToJson();
            }
        }
    }
}