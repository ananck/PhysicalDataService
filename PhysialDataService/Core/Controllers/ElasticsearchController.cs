using Core.Common;
using Core.IService;
using Core.IService.ViewModel;
using Core.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Core.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ElasticsearchController : ControllerBase
    {
        private readonly ManufacturerService studentService;

        public ElasticsearchController(ManufacturerService studentService)
        {
            this.studentService = studentService;
        }

        
        [HttpPost]
        public async Task<string> Insert([FromForm] ManufacturerModel model)
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
                return new Response() { Code = 500, Msg = e.Message }.ToJson();
            }
        }

       
    }
}