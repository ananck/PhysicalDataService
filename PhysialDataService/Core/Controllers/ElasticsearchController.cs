using Core.Common;
using Core.IService;
using Core.IService.ViewModel;
using Core.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Core.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    [Description("物性库接口")]
    public class ElasticsearchController : ControllerBase
    {
        private readonly ManufacturerService studentService;

        public ElasticsearchController(ManufacturerService studentService)
        {
            this.studentService = studentService;
        }

        [Description("批量新增材料制造商接口")]
        [HttpPost]
        public async Task<string> BatchInsertManufacture([FromForm] ManufacturerModel model)
        {
            try
            {
                var manufacturer = new Manufacturer
                {
                    FullName=model.FullName,
                    ShortName=model.ShortName,
                    EnglishName=model.EnglishName
                };
                await studentService.Insert(manufacturer);

                return new Response().ToJson();
            }
            catch (Exception e)
            {
                return new Response() { code = 500, msg = e.Message }.ToJson();
            }
        }

       
    }
}