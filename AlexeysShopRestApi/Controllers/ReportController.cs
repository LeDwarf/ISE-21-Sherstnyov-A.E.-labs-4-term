using AlexeysShopService.BindingModels;
using AlexeysShopService.Interfaces;
using System;
using System.Web.Http;

namespace AlexeysShopRestApi.Controllers
{
	public class ReportController : ApiController
	{
		private readonly IReportService _service;

		public ReportController(IReportService service)
		{
			_service = service;
		}

		[HttpGet]
		public IHttpActionResult GetStoragesLoad()
		{
			var list = _service.GetStoragesLoad();
			if (list == null)
			{
				InternalServerError(new Exception("Нет данных"));
			}
			return Ok(list);
		}

		[HttpPost]
		public IHttpActionResult GetCustomerContracts(ReportBindingModel model)
		{
			var list = _service.GetCustomerContracts(model);
			if (list == null)
			{
				InternalServerError(new Exception("Нет данных"));
			}
			return Ok(list);
		}

		[HttpPost]
		public void SaveArticlePrice(ReportBindingModel model)
		{
			_service.SaveArticlePrice(model);
		}

		[HttpPost]
		public void SaveStoragesLoad(ReportBindingModel model)
		{
			_service.SaveStoragesLoad(model);
		}

		[HttpPost]
		public void SaveCustomerContracts(ReportBindingModel model)
		{
			_service.SaveCustomerContracts(model);
		}
	}
}
