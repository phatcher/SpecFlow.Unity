using System;
using System.Linq;

using Microsoft.Practices.Unity;

using TechTalk.SpecFlow;

namespace SpecFlow.Unity
{
    public static class UnityExtensions
    {
        /// <summary>
        /// Registers classes with SpecFlow BindingAttribute into the container with default naming and container controller lifetime.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="container"></param>
        public static void RegisterStepDefinitions<T>(this IUnityContainer container)
        {
            container.RegisterTypes(typeof(T).Assembly.GetTypes().Where(t => Attribute.IsDefined(t, typeof(BindingAttribute))),
                                    WithMappings.FromMatchingInterface,
                                    WithName.Default,
                                    WithLifetime.ContainerControlled);
        }
    }
}