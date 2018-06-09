using AlexeysShopService.BindingModels;
using AlexeysShopService.Interfaces;
using System;
using System.Web.Http;

namespace AlexeysShopRestApi.Controllers
{
	public class ArticleController : ApiController
	{
		private readonly IArticleService _service;

		public ArticleController(IArticleService service)
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
		public void AddElement(ArticleBindingModel model)
		{
			_service.AddElement(model);
		}

		[HttpPost]
		public void UpdElement(ArticleBindingModel model)
		{
			_service.UpdElement(model);
		}

		[HttpPost]
		public void DelElement(ArticleBindingModel model)
		{
			_service.DelElement(model.Id);
		}
	}
}
