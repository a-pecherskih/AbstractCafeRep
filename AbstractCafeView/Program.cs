using AbstractCafeService.ImplementatinsList;
using AbstractCafeService.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Unity;
using Unity.Lifetime;

namespace AbstractCafeView
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
            Application.Run(container.Resolve<FormMain>());
        }

        public static IUnityContainer BuildUnityContainer()
        {
            var currentContainer = new UnityContainer();
            currentContainer.RegisterType<ICustomerService, CustomerServiceList>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IDishService, DishServiceList>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IChefService, ChefServiceList>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IMenuService, MenuServiceList>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IKitchenService, KitchenServiceList>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IMainService, MainServiceList>(new HierarchicalLifetimeManager());

            return currentContainer;
        }

    }
}
