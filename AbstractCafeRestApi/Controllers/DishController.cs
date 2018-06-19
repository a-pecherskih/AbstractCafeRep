using AbstractCafeService.BindingModel;
using AbstractCafeService.Interfaces;
using System;
using System.Web.Http;

namespace AbstractCafeRestApi.Controllers
{
    public class DishController : ApiController
    {
        private readonly IDishService _service;

        public DishController(IDishService service)
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
        public void AddElement(DishBindingModel model)
        {
            _service.AddElement(model);
        }

        [HttpPost]
        public void UpdElement(DishBindingModel model)
        {
            _service.UpdElement(model);
        }

        [HttpPost]
        public void DelElement(DishBindingModel model)
        {
            _service.DelElement(model.Id);
        }
    }
}
