using System;
using System.Linq;
using System.Reflection;
using TechTalk.SpecFlow.Bindings;
using Unity;

namespace SpecFlow.Unity
{
    /// <copydoc cref="IContainerFinder" />
    public class ContainerFinder : IContainerFinder
    {
        private readonly IBindingRegistry bindingRegistry;
        private readonly Lazy<Func<IUnityContainer>> createScenarioContainer;

        /// <summary>
        /// Creates a new instance of the <see cref="ContainerFinder"/> class.
        /// </summary>
        /// <param name="bindingRegistry"></param>
        public ContainerFinder(IBindingRegistry bindingRegistry)
        {
            this.bindingRegistry = bindingRegistry;
            createScenarioContainer = new Lazy<Func<IUnityContainer>>(FindCreateScenarioContainer, true);
        }

        /// <copydoc cref="IContainerFinder.GetCreateScenarioContainer" />
        public Func<IUnityContainer> GetCreateScenarioContainer()
        {
            var builder = createScenarioContainer.Value;
            if (builder == null)
            {
                throw new Exception("Unable to find scenario dependencies! Mark a static method that returns an IUnityContainer with [ScenarioDependencies]!");
            }
            return builder;
        }

        public Func<IUnityContainer> FindCreateScenarioContainer()
        {
            var assemblies = bindingRegistry.GetBindingAssemblies();
            foreach (var assembly in assemblies)
            {
                foreach (var type in assembly.GetTypes())
                {
                    foreach (var methodInfo in type.GetMethods(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public).Where(m => Attribute.IsDefined((MemberInfo)m, typeof(ScenarioDependenciesAttribute))))
                    {
                        return () => (IUnityContainer)methodInfo.Invoke(null, null);
                    }
                }
            }
            return null;
        }
    }
}
