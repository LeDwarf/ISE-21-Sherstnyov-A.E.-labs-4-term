using AlexeysShopService.ImplementationsList;
using AlexeysShopService.Interfaces;
using System;
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
            currentContainer.RegisterType<ICustomerService, CustomerServiceList>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IPartService, PartServiceList>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IBuilderService, BuilderServiceList>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IArticleService, ArticleServiceList>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IStorageService, StorageServiceList>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IGeneralService, GeneralServiceList>(new HierarchicalLifetimeManager());

            return currentContainer;
        }
    }
}
