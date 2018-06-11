using AlexeysShopService.BindingModels;
using AlexeysShopService.Interfaces;
using System;
using System.Web.Http;

namespace AlexeysShopRestApi.Controllers
{
	public class BuilderController : ApiController
	{
		private readonly IBuilderService _service;

		public BuilderController(IBuilderService service)
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
		public void AddElement(BuilderBindingModel model)
		{
			_service.AddElement(model);
		}

		[HttpPost]
		public void UpdElement(BuilderBindingModel model)
		{
			_service.UpdElement(model);
		}

		[HttpPost]
		public void DelElement(BuilderBindingModel model)
		{
			_service.DelElement(model.Id);
		}
	}
}
