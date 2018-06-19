using AbstractCafeService;
using AbstractCafeService.ImplementatinsList;
using AbstractCafeService.ImplementationsBD;
using AbstractCafeService.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Unity;
using Unity.Lifetime;

namespace WpfAbstractCafe
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        [STAThread]
        static void Main()
        {
            var container = BuildUnityContainer();

            var application = new App();
            application.Run(container.Resolve<MainWindow>());
        }

        public static IUnityContainer BuildUnityContainer()
        {
            var currentContainer = new UnityContainer();
            currentContainer.RegisterType<DbContext, AbstractDbContext>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<ICustomerService, CustomerServiceBD>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IDishService, DishServiceBD>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IChefService, ChefServiceBD>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IMenuService, MenuServiceBD>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IKitchenService, KitchenServiceBD>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IMainService, MainServiceBD>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IReportService, ReportServiceBD>(new HierarchicalLifetimeManager());

            return currentContainer;
        }
    }
}
