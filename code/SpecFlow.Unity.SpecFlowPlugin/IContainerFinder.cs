using System;
using Unity;

namespace SpecFlow.Unity
{
    /// <summary>
    /// Responsible for creating the container used by a scenario
    /// </summary>
    public interface IContainerFinder
    {
        /// <summary>
        /// Get or create the <see cref="IUnityContainer"/> to be used by a scenario.
        /// </summary>
        /// <returns></returns>
        Func<IUnityContainer> GetCreateScenarioContainer();
    }
}
