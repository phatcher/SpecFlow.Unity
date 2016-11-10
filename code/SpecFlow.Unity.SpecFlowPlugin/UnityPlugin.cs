using Microsoft.Practices.Unity;

using SpecFlow.Unity;

using TechTalk.SpecFlow.Infrastructure;
using TechTalk.SpecFlow.Plugins;

[assembly: RuntimePlugin(typeof(UnityPlugin))]

namespace SpecFlow.Unity
{
    public class UnityPlugin : IRuntimePlugin
    {
        public void Initialize(RuntimePluginEvents runtimePluginEvents, RuntimePluginParameters runtimePluginParameters)
        {
            runtimePluginEvents.CustomizeGlobalDependencies += (sender, args) =>
            {
                args.ObjectContainer.RegisterTypeAs<UnityBindingInstanceResolver, IBindingInstanceResolver>();
                args.ObjectContainer.RegisterTypeAs<ContainerFinder, IContainerFinder>();
            };

            runtimePluginEvents.CustomizeScenarioDependencies += (sender, args) =>
            {
                args.ObjectContainer.RegisterFactoryAs<IUnityContainer>(() =>
                {
                    var containerBuilderFinder = args.ObjectContainer.Resolve<IContainerFinder>();
                    var containerBuilder = containerBuilderFinder.GetCreateScenarioContainer();
                    var container = containerBuilder.Invoke();
                    return container;
                });
            };
        }
    }
}