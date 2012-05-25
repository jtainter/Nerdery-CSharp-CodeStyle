# Nerdery C# Code Style Rules

The goal of this project is to improve style within a project by setting flags that enforce consistency, but not necessarily a specific set of rules.

This project is intended to be a NuGet package that all projects can install and have a reasonable set of style guidelines and R# definitions for auto code formatting.

There's no right way to write code. Everyone has their own flavor and likes/dislikes when writing code, and this is not intended to be a bible or a strict enforcement of "this is the way that you have to code". We want you to take this library and use it as a starting point, customize the rules to your liking, and allow the StyleCop and R# tools to improve your coding experience.

##Quick Start

 1. If you've already installed StyleCop, **uninstall it**. This is a standalone library and the StyleCop installer does some very annoying stuff with the GAC and MSBuild targets. Unfortunately, the way that StyleCop finds external rule libraries will almost guarantee that all of our style rules won't get run (some of them will though). We are looking into a way to remove this step.
 1. Install the package from NuGet
```
Install-Package Nerdery.CSharpCodeStyle -Version 1.0.0-alpha -Pre
```
 1. Build your project, and start making your style consistent!
 
More information on how to customize, contribute, and find out about our style decisions can be found on our wiki 

<https://github.com/kensykora/Nerdery-CSharp-CodeStyle/wiki>

##Credits

This library is maintained by The Nerdery .NET Committee and .NET Principle Software Engineers, and we welcome your feedback. This undertaking would not be possible without the awesome work of the community developers for StyleCop.

Major props to the authors and contributors of these projects for making such awesome tools:

<http://stylecop.codeplex.com/>  
<http://stylecopplus.codeplex.com/>  
<https://bitbucket.org/adamralph/stylecop-msbuild/wiki/Home>

---

