using System;
using BoDi;
using TechTalk.SpecFlow.Infrastructure;
using Unity;

namespace SpecFlow.Unity
{
    public class UnityBindingInstanceResolver : ITestObjectResolver
    {
        public object ResolveBindingInstance(Type bindingType, IObjectContainer scenarioContainer)
        {
            var componentContext = scenarioContainer.Resolve<IUnityContainer>();
            return componentContext.Resolve(bindingType);
        }
    }
}