# Grynwald.XmlDocs

## Table of Contents

- [Overview](#overview)
- [Usage](#usage)
- [License](#license)

## Overview

C# source code can have structured comments to provide inline API documentation.
These comments are saved by the compiler as an *XML documentation file*.

The `Grynwald.XmlDocs` package provides a library for parsing these comments into a .NET object model.

## Usage

To parse an XML documentation file, first reference the `Grynwald.XmlDocs` package in your project.

Load the documentation file using the `DocumentationFile` class:

```cs
using Grynwald.XmlDocs;

// Load XML documentation file
var documentationFile = DocumentationFile.FromFile("./MyAssembly.xml");
```

Alternatively, you can parse individual elements directly, e.g. to parse a `<member />` node from the documentation file, use

```cs
using Grynwald.XmlDocs;

var memberXml = """
    <member name="T:ExampleProject.ExampleClass">
        <summary>
            XML documentation file example
        </summary>            
    </member>
    """

var member = MemberElement.FromXml(memberXml);
```

To process the documentation file, the library provides support for the Visitor pattern.
To create a visitor, either implement `IDocumentationVisitor` directly or derive from the default implementation `DocumentationVisitor` and override the `Visit()` method for the documentation element you want to process.


## License

Grynwald.XmlDocs.MarkdownRenderer is licensed under the MIT License.

For details see https://github.com/ap0llo/xmldocs/blob/master/LICENSE