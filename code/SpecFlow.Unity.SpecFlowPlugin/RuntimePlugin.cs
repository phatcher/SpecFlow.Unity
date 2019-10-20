using SpecFlow.Unity;
using TechTalk.SpecFlow.Infrastructure;
using TechTalk.SpecFlow.Plugins;
using TechTalk.SpecFlow.UnitTestProvider;
using Unity;

[assembly: RuntimePlugin(typeof(RuntimePlugin))]

namespace SpecFlow.Unity
{
    public class RuntimePlugin : IRuntimePlugin
    {
        public void Initialize(RuntimePluginEvents runtimePluginEvents, RuntimePluginParameters runtimePluginParameters,
            UnitTestProviderConfiguration unitTestProviderConfiguration)
        {
            runtimePluginEvents.CustomizeGlobalDependencies += (sender, args) =>
            {
                args.ObjectContainer.RegisterTypeAs<UnityBindingInstanceResolver, ITestObjectResolver>();
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