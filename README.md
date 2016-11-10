SpecFlow Unity
================

SpecFlow plugin for using Autofac as a dependency injection framework for step definitions.

Currently supports

* SpecFlow v2.1
* Unity v4.0.1 or above

Available as a NuGet packages [SpecFlow.Unity](https://www.nuget.org/packages/SpecFlow.Unity/)

[![NuGet](https://img.shields.io/nuget/v/SpecFlow.Unity.svg)](https://www.nuget.org/packages/SpecFlow.Unity/)
[![Build status](https://ci.appveyor.com/api/projects/status/f85dv0joq14uyn31/branch/master?svg=true)](https://ci.appveyor.com/project/PaulHatcher/specflow-unity/branch/master)

Welcome to contributions from anyone.

There's a sample project at https://github.com/phatcher/SpecFlow.Unity/tree/master/sample/MyCalculator

You can see the version history [here](RELEASE_NOTES.md).

## Build the project
* Windows: Run *build.cmd*

I have my tools in C:\Tools so I use *build.cmd Default tools=C:\Tools encoding=UTF-8* 

## Library License

The library is available under the [MIT License](http://en.wikipedia.org/wiki/MIT_License), for more information see the [License file][1] in the GitHub repository.

 [1]: https://github.com/phatcher/SpecFlow.Unity/blob/master/License.md 

## Getting Started

This project was inspired by [SpecFlow.Autofac](https://github.com/gasparnagy/SpecFlow.Autofac) and follows the similar patterns as that project and the associated blog post http://gasparnagy.com/2016/08/specflow-tips-customizing-dependency-injection-with-autofac/

Install plugin from NuGet into your SpecFlow project.

    PM> Install-Package SpecFlow.Unity

Create a static method somewhere in the SpecFlow project (recommended to put it into the `Support` folder) that returns a Unity `IUnityContainer` and tag it with the `[ScenarioDependencies]` attribute. Configure your dependencies for the scenario execution within the method. You also have to register the step definition classes, that you can do by either registering all public types from the SpecFlow project:

    container.RegisterAssemblyTypes(typeof(YourClassInTheSpecFlowProject).Assembly, WithMappings.FromMatchingInterface, WithName.Default, WithLifetime.ContainerControlled);

or by registering all classes marked with the `[Binding]` attribute:

    builder.RegisterTypes(typeof(TestDependencies).Assembly.GetTypes().Where(t => Attribute.IsDefined(t, typeof(BindingAttribute))), WithMappings.FromMatchingInterface, WithName.Default, WithLifetime.ContainerControlled);

A typical container creation method probably looks like this:

    [ScenarioDependencies]
    public static IUnityContainer CreateContainer()
    {
      // create container with the runtime dependencies
      var container = Dependencies.CreateContainer();

      //TODO: add customizations, stubs required for testing

      // Registers the build steps, this gives us dependency resolution using the container.
      // NB If you need named parameters into the steps you should override specific registrations
      container.RegisterTypes(typeof(TestDependencies).Assembly.GetTypes().Where(t => Attribute.IsDefined(t, typeof(BindingAttribute))), WithMappings.FromMatchingInterface, WithName.Default, WithLifetime.ContainerControlled);
      
      return container;
    }

Also don't forget to modify your test App.config to refer to the plugin

    <specFlow>
        <!-- For additional details on SpecFlow configuration options see http://go.specflow.org/doc-config -->
        <unitTestProvider name="NUnit" />
        <runtime stopAtFirstError="false" missingOrPendingStepsOutcome="Inconclusive" />
        <plugins>
            <add name="SpecFlow.Unity" type="Runtime" />
        </plugins>
    </specFlow>

You must also have at least one step definition in the same assembly as the ScenarioDependencies method for the it to be found.