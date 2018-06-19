using AbstractCafeService.BindingModel;
using AbstractCafeService.Interfaces;
using System;
using System.Web.Http;

namespace AbstractCafeRestApi.Controllers
{
    public class ChefController : ApiController
    {
        private readonly IChefService _service;

        public ChefController(IChefService service)
        {
            _service = service;
        }

        [HttpGet]
        public IHttpActionResult GetList()
        {
            var list = _service.GetList();
            if (list == null)
            {
                InternalServerError(new Exception("Нет данных"));
            }
            return Ok(list);
        }

        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            var element = _service.GetElement(id);
            if (element == null)
            {
                InternalServerError(new Exception("Нет данных"));
            }
            return Ok(element);
        }

        [HttpPost]
        public void AddElement(ChefBindingModel model)
        {
            _service.AddElement(model);
        }

        [HttpPost]
        public void UpdElement(ChefBindingModel model)
        {
            _service.UpdElement(model);
        }

        [HttpPost]
        public void DelElement(ChefBindingModel model)
        {
            _service.DelElement(model.Id);
        }
    }
}
