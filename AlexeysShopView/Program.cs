using AlexeysShopService;
using AlexeysShopService.ImplementationsBD;
using AlexeysShopService.ImplementationsList;
using AlexeysShopService.Interfaces;
using System;
using System.Data.Entity;
using System.Windows.Forms;
using Unity;
using Unity.Lifetime;

namespace AlexeysShopView
{
	static class Program
	{
		/// <summary>
		/// Главная точка входа для приложения.
		/// </summary>
		[STAThread]
		static void Main()
		{
			var container = BuildUnityContainer();

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(container.Resolve<FormGeneral>());
		}

		public static IUnityContainer BuildUnityContainer()
		{
			var currentContainer = new UnityContainer();
			currentContainer.RegisterType<DbContext, AlexeysDbContext>(new HierarchicalLifetimeManager());
			currentContainer.RegisterType<ICustomerService, CustomerServiceBD>(new HierarchicalLifetimeManager());
			currentContainer.RegisterType<IPartService, PartServiceBD>(new HierarchicalLifetimeManager());
			currentContainer.RegisterType<IBuilderService, BuilderServiceBD>(new HierarchicalLifetimeManager());
			currentContainer.RegisterType<IArticleService, ArticleServiceBD>(new HierarchicalLifetimeManager());
			currentContainer.RegisterType<IStorageService, StorageServiceBD>(new HierarchicalLifetimeManager());
			currentContainer.RegisterType<IGeneralService, GeneralServiceBD>(new HierarchicalLifetimeManager());

			return currentContainer;
		}
	}
}
