using AbstractCafeService.BindingModel;
using AbstractCafeService.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AbstractCafeRestApi.Controllers
{
    public class ReportController : ApiController
    {
        private readonly IReportService _service;

        public ReportController(IReportService service)
        {
            _service = service;
        }

        [HttpGet]
        public IHttpActionResult GetKitchensLoad()
        {
            var list = _service.GetKitchensLoad();
            if (list == null)
            {
                InternalServerError(new Exception("Нет данных"));
            }
            return Ok(list);
        }

        [HttpPost]
        public IHttpActionResult GetCustomerChoices(ReportBindingModel model)
        {
            var list = _service.GetCustomerChoices(model);
            if (list == null)
            {
                InternalServerError(new Exception("Нет данных"));
            }
            return Ok(list);
        }

        [HttpPost]
        public void SaveMenuPrice(ReportBindingModel model)
        {
            _service.SaveMenuPrice(model);
        }

        [HttpPost]
        public void SaveKitchensLoad(ReportBindingModel model)
        {
            _service.SaveKitchensLoad(model);
        }

        [HttpPost]
        public void SaveCustomerChoices(ReportBindingModel model)
        {
            _service.SaveCustomerChoices(model);
        }
    }
}
