using AlexeysShopService.BindingModels;
using AlexeysShopService.Interfaces;
using System;
using System.Web.Http;

namespace AlexeysShopRestApi.Controllers
{
	public class GeneralController : ApiController
	{
		private readonly IGeneralService _service;

		public GeneralController(IGeneralService service)
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
		public void CreateContract(ContractBindingModel model)
		{
			_service.CreateContract(model);
		}

		[HttpPost]
		public void TakeContractInWork(ContractBindingModel model)
		{
			_service.TakeContractInWork(model);
		}

		[HttpPost]
		public void FinishContract(ContractBindingModel model)
		{
			_service.FinishContract(model.Id);
		}

		[HttpPost]
		public void PayContract(ContractBindingModel model)
		{
			_service.PayContract(model.Id);
		}

		[HttpPost]
		public void PutPartOnStorage(StoragePartBindingModel model)
		{
			_service.PutPartOnStorage(model);
		}
	}
}
