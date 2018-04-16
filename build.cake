﻿#tool "nuget:?package=xunit.runner.console&version=2.3.0-beta5-build3769"
#tool "nuget:?package=nunit.runners&version=2.6.4"
#tool "nuget:?package=nunit.consolerunner&version=3.7.0"
#tool "nuget:?package=nspec&version=1.0.13"
#tool "nuget:?package=nspec&version=2.0.1"
#tool "nuget:?package=nspec&version=3.1.0"
#tool "nuget:?package=Machine.Specifications.Runner.Console&version=0.9.3"
#tool "nuget:?package=GitVersion.CommandLine"

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var toolpath = Argument("toolpath", @"");

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

// Define directories.
var buildDir = Directory("./Artifacts") + Directory(configuration);
GitVersion gitVersion = null; 

if (!FileExists("./tools/nspec.2.0.1/NSpec/tools/net451/NSpec.dll"))
{
    // NSpec2.0.1 does not have NSpec.dll in the test runner directory, which crashes the test runner.
    CopyFile("./tools/nspec.2.0.1/NSpec/lib/net451/NSpec.dll", "./tools/nspec.2.0.1/NSpec/tools/net451/NSpec.dll");
}

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() =>
{
    CleanDirectory(buildDir);
});

Task("GitVersion").Does(() => {
    gitVersion = GitVersion(new GitVersionSettings {
        UpdateAssemblyInfo = true,
	});
});

Task("Restore-NuGet-Packages")
    .IsDependentOn("Clean")
    .Does(() =>
{
	DotNetCoreRestore();

    NuGetRestore("./FluentAssertions.sln", new NuGetRestoreSettings 
	{ 
		NoCache = true,
		Verbosity = NuGetVerbosity.Detailed,
		ToolPath = "./build/nuget.exe"
	});
});

Task("Build")
    .IsDependentOn("Restore-NuGet-Packages")
	.IsDependentOn("GitVersion")
    .Does(() =>
{
    if(IsRunningOnWindows())
    {
      // Use MSBuild
      MSBuild("./FluentAssertions.sln", settings => {
		settings.ToolPath = String.IsNullOrEmpty(toolpath) ? settings.ToolPath : toolpath;
		settings.ToolVersion = MSBuildToolVersion.VS2017;
        settings.PlatformTarget = PlatformTarget.MSIL;
		settings.SetConfiguration(configuration);
	  });
    }
    else
    {
      // Use XBuild
      XBuild("./FluentAssertions.sln", settings =>
        settings.SetConfiguration(configuration));
    }
});

Task("Run-Unit-Tests")
    .Does(() =>
{
    XUnit2("./Tests/Net45.Specs/bin/Debug/**/*.Specs.dll", new XUnit2Settings { });
    XUnit2("./Tests/Net47.Specs/bin/Debug/**/*.Specs.dll", new XUnit2Settings { });
    DotNetCoreTool("./Tests/NetCore.Specs/NetCore.Specs.csproj", "xunit", "-configuration debug");
    DotNetCoreTool("./Tests/NetCore13.Specs/NetCore13.Specs.csproj", "xunit", "-configuration " + configuration);
    DotNetCoreTool("./Tests/NetCore20.Specs/NetCore.Specs20.csproj", "xunit", "-configuration debug");

    XUnit2("./Tests/TestFrameworks/XUnit.Net45.Specs/**/bin/" + configuration + "/*.Specs.dll", new XUnit2Settings { });
    XUnit2("./Tests/TestFrameworks/XUnit2.Net45.Specs/**/bin/" + configuration + "/*.Specs.dll", new XUnit2Settings { });
    NUnit("./Tests/TestFrameworks/NUnit2.Net45.Specs/**/bin/" + configuration + "/*.Specs.dll", new NUnitSettings { NoResults = true });
    NUnit3("./Tests/TestFrameworks/NUnit3.Net45.Specs/**/bin/" + configuration + "/*.Specs.dll", new NUnit3Settings { NoResults = true });

    StartProcess(Context.Tools.Resolve("nspec.1.*/**/NSpecRunner.exe"), "./Tests/TestFrameworks/NSpec.Net45.Specs/bin/" + configuration + "/NSpec.Specs.dll");
    StartProcess(Context.Tools.Resolve("nspec.2.*/**/NSpecRunner.exe"), "./Tests/TestFrameworks/NSpec2.Net45.Specs/bin/" + configuration + "/NSpec2.Specs.dll");
    StartProcess(Context.Tools.Resolve("nspec.3.*/**/NSpecRunner.exe"), "./Tests/TestFrameworks/NSpec3.Net45.Specs/bin/" + configuration + "/NSpec3.Specs.dll");
    StartProcess(Context.Tools.Resolve("mspec-clr4.exe"), "./Tests/TestFrameworks/MSpec.Net45.Specs/bin/" + configuration + "/MSpec.Specs.dll");
});

Task("Pack")
    .IsDependentOn("GitVersion")
    .Does(() => 
    {
      NuGetPack("./src/FluentAssertions.nuspec", new NuGetPackSettings {
        OutputDirectory = "./Artifacts",
        Version = gitVersion.NuGetVersionV2
      });  
    });

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
	.IsDependentOn("GitVersion")
    .IsDependentOn("Build")
    .IsDependentOn("Run-Unit-Tests")
    .IsDependentOn("Pack");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);
