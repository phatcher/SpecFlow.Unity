using Microsoft.Practices.Unity;

namespace MyCalculator
{
    public static class Dependencies
    {
        public static IUnityContainer CreateContainer()
        {
            var container = new UnityContainer();

            container.RegisterType<ICalculator, Calculator>(
                new HierarchicalLifetimeManager());

            return container;
        }
    }
}