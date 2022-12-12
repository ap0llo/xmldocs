# XmlDocs

[![Build Status](https://dev.azure.com/ap0llo/OSS/_apis/build/status/xmldocs?branchName=master)](https://dev.azure.com/ap0llo/OSS/_build/latest?definitionId=25&branchName=master)
[![Renovate](https://img.shields.io/badge/Renovate-enabled-brightgreen)](https://renovatebot.com/)
[![Conventional Commits](https://img.shields.io/badge/Conventional%20Commits-1.0.0-green.svg)](https://conventionalcommits.org)

| Package | NuGet.org | Azure Artifacts |
|-|-|-|
| Grynwald.XmlDocs | [![NuGet](https://img.shields.io/nuget/v/Grynwald.XmlDocs.svg?logo=nuget)](https://www.nuget.org/packages/Grynwald.XmlDocs) | [![Azure Artifacts](https://img.shields.io/badge/Azure%20Artifacts-prerelease-yellow.svg?logo=azuredevops)](https://dev.azure.com/ap0llo/OSS/_packaging?_a=feed&feed=PublicCI) |
| Grynwald.XmlDocs.MarkdownRenderer | [![NuGet](https://img.shields.io/nuget/v/Grynwald.XmlDocs.MarkdownRenderer.svg?logo=nuget)](https://www.nuget.org/packages/Grynwald.XmlDocs.MarkdownRenderer) | [![Azure Artifacts](https://img.shields.io/badge/Azure%20Artifacts-prerelease-yellow.svg?logo=azuredevops)](https://dev.azure.com/ap0llo/OSS/_packaging?_a=feed&feed=PublicCI) |

## Overview

A library for parsing  [C# XML Documentation files](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/xmldoc/) and a utility library to convert XML documentation comments to Markdown.

## Documentation

This repository hosts the following packages that provide functionality for processing XML documentation comments.

- `Grynwald.XmlDocs` is a library for parsing XML documentation comments into a .NET object model.
  - For details, please refer to the [package README](./src/XmlDocs/package/README.md)
- `Grynwald.XmlDocs.MarkdownRenderer` is provides a converter from XML documentation comments to Markdown
  - It is based on `Grynwald.XmlDocs` and [Markdown Generator](https://github.com/ap0llo/markdown-generator)
  - **Note:** The package can only convert the contents of the XML documentation file.
    It is not a full .NET documentation generator.
  - For details, please refer to the [package README](./src/XmlDocs.MarkdownRenderer/package/README.md)


## Installation

- Prerelease builds are available on [Azure Artifacts](https://dev.azure.com/ap0llo/OSS/_artifacts/feed/PublicCI)
- Release versions are available on NuGet.org:
  - [Grynwald.XmlDocs](https://www.nuget.org/packages/Grynwald.XmlDocs)
  - [Grynwald.XmlDocs.MarkdownRenderer](https://www.nuget.org/packages/Grynwald.XmlDocs.MarkdownRenderer)

## Building from source

Building from source requires the .NET 7 SDK (version 6.0.100 as specified in [global.json](./global.json)) and uses [Cake](https://cakebuild.net/) for the build.

To execute the default task, run

```ps1
.\build.ps1
```

This will build the project, run all tests and pack the NuGet package.

## Acknowledgments

XmlDocs was made possible through a number of libraries (aside from .NET itself):

- [Nerdbank.GitVersioning](https://github.com/dotnet/Nerdbank.GitVersioning/)
- [SourceLink](https://github.com/dotnet/sourcelink)
- [ReportGenerator](https://github.com/danielpalme/ReportGenerator)
- [Cake](https://cakebuild.net/)
- [Cake.BuildSystems.Module](https://github.com/cake-contrib/Cake.BuildSystems.Module)
- [xUnit](http://xunit.github.io/)
- [Coverlet](https://github.com/tonerdo/coverlet)


## Versioning and Branching

The version of this project is automatically derived from git and the information
in `version.json` using [Nerdbank.GitVersioning](https://github.com/dotnet/Nerdbank.GitVersioning):

- The master branch  always contains the latest version. 
  Packages produced from `master` are always marked as pre-release versions (using the `-pre` suffix).
- Stable versions are built from release branches. 
  Builds from release branches will have no `-pre` suffix.
- Builds from any other branch will have both the `-pre` prerelease tag and the git commit hash included in the version string.

To create a new release branch use the [`nbgv` tool](https://www.nuget.org/packages/nbgv/):

```ps1
dotnet tool restore
dotnet tool run nbgv -- prepare-release
```
