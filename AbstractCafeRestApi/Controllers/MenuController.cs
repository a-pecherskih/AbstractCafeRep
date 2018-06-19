using AbstractCafeService.BindingModel;
using AbstractCafeService.Interfaces;
using System;
using System.Web.Http;

namespace AbstractCafeRestApi.Controllers
{
    public class MenuController : ApiController
    {
        private readonly IMenuService _service;

        public MenuController(IMenuService service)
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
        public void AddElement(MenuBindingModel model)
        {
            _service.AddElement(model);
        }

        [HttpPost]
        public void UpdElement(MenuBindingModel model)
        {
            _service.UpdElement(model);
        }

        [HttpPost]
        public void DelElement(MenuBindingModel model)
        {
            _service.DelElement(model.Id);
        }
    }
}
