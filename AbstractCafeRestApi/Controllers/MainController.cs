using AbstractCafeRestApi.Services;
using AbstractCafeService.BindingModel;
using AbstractCafeService.Interfaces;
using System;
using System.Web.Http;

namespace AbstractCafeRestApi.Controllers
{
    public class MainController : ApiController
    {
        private readonly IMainService _service;

        public MainController(IMainService service)
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

        [HttpPost]
        public void CreateChoice(ChoiceBindingModel model)
        {
            _service.CreateChoice(model);
        }

        [HttpPost]
        public void TakeChoiceInWork(ChoiceBindingModel model)
        {
            _service.TakeChoiceInWork(model);
        }

        [HttpPost]
        public void FinishChoice(ChoiceBindingModel model)
        {
            _service.FinishChoice(model.Id);
        }

        [HttpPost]
        public void PayChoice(ChoiceBindingModel model)
        {
            _service.PayChoice(model.Id);
        }

        [HttpPost]
        public void PutDishOnKitchen(KitchenDishBindingModel model)
        {
            _service.PutDishOnKitchen(model);
        }

        [HttpGet]
        public IHttpActionResult GetInfo()
        {
            ReflectionService service = new ReflectionService();
            var list = service.GetInfoByAssembly();
            if (list == null)
            {
                InternalServerError(new Exception("Нет данных"));
            }
            return Ok(list);
        }
    }
}
