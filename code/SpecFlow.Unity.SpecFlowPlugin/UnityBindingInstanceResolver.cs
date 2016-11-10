using System;

using BoDi;

using Microsoft.Practices.Unity;

using TechTalk.SpecFlow.Infrastructure;

namespace SpecFlow.Unity
{
    public class UnityBindingInstanceResolver : IBindingInstanceResolver
    {
        public object ResolveBindingInstance(Type bindingType, IObjectContainer scenarioContainer)
        {
            var componentContext = scenarioContainer.Resolve<IUnityContainer>();
            return componentContext.Resolve(bindingType);
        }
    }
}