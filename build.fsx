/// FAKE Build script

#r "packages/build/FAKE/tools/FakeLib.dll"
open Fake.Core
open Fake.Core.TargetOperators
open Fake.DotNet
open Fake.DotNet.Testing
open Fake.IO
open Fake.IO.FileSystemOperators
open Fake.IO.Globbing.Operators
open Fake.Tools

// Version info
let projectName = "SpecFlow.Unity"
let projectSummary = ""
let projectDescription = "SpecFlow plugin for using Unity as a dependency injection framework for step definitions."
let authors = ["Paul Hatcher"]

let release = ReleaseNotes.load "RELEASE_NOTES.md"

// Properties
let buildDir = "./build"
let toolsDir = Environment.environVarOrDefault "tools" "packages/build"
let solutionFile = "SpecFlow.Unity.sln"

let nunitPath = toolsDir @@ "/NUnit.ConsoleRunner/tools/nunit3-console.exe"

// Targets
Target.create "Clean" (fun _ ->
    Shell.cleanDirs [buildDir;]
)

Target.create "SetVersion" (fun _ ->
    let commitHash = 
        try 
            Git.Information.getCurrentHash()
        with
            | ex -> printfn "Exception! (%s)" (ex.Message); ""
    let infoVersion = String.concat " " [release.AssemblyVersion; commitHash]
    AssemblyInfoFile.createCSharp "./code/SolutionInfo.cs"
        [AssemblyInfo.Version release.AssemblyVersion
         AssemblyInfo.FileVersion release.AssemblyVersion
         AssemblyInfo.InformationalVersion infoVersion]
)

Target.create "Build" (fun _ ->
    let setParams (defaults:MSBuildParams) =
        { defaults with
            DoRestore = true
            Properties =
                [
                    "Platform", "Any CPU"
                    "PackageVersion", release.AssemblyVersion
                    "PackageReleaseNotes", release.Notes |> String.toLines
                    "IncludeSymbols", "true"
                ]
         }
    MSBuild.runRelease setParams buildDir "Build" [solutionFile]
    |> Trace.logItems "AppBuild-Output: "
)

Target.create "Test" (fun _ ->
    !! (buildDir + "/*.Test.dll")
    |> NUnit3.run (fun p ->
       {p with
          ToolPath = nunitPath
          // Oddity as this creates a build directory in the build directory
          WorkingDir = buildDir
          ShadowCopy = false})
)

Target.create "Release" (fun _ ->
    let tag = String.concat "" ["v"; release.AssemblyVersion] 
    Git.Branches.tag "" tag
    Git.Branches.pushTag "" "origin" tag
)

Target.create "Default" ignore

// Dependencies
"Clean"
    ==> "SetVersion"
    ==> "Build"
    // ==> "Test"
    ==> "Default"
    ==> "Release"

Target.runOrDefault "Default"