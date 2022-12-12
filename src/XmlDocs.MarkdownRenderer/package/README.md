# Grynwald.XmlDocs.MarkdownRenderer

A library to convert [C# XML Documentation Comments](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/xmldoc/) to Markdown based on [Markdown Generator](https://github.com/ap0llo/markdown-generator)

## Table of Contents

- [Overview](#overview)
- [Usage](#usage)
- [License](#license)

## Overview

C# source code can have structured comments to provide inline API documentation.
These comments are saved by the compiler as an *XML documentation file*.

While the `Grynwald.XmlDocs` package provides a library for parsing these comments into a .NET object model, this package adds the option to convert the XML documentation comments including the formatting tags to Markdown.

---

**⚠️ Note:** 
This library is not a complete documentation generator but is intended to serve as the basis of such a generator.
The XML documentation file does not contain all members of an assembly but only the members for which the compiler found any XML documentation comments.
To generate the full documentation of an assembly requires building a semantic model of that assembly which can be achieved using libraries like Mono.Cecil or Roslyn (Microsoft.CodeAnalyis).

The library will also not be able to resolve references to code element (like `<see cref="SomeClass"/>`).
However, the conversion to Markdown can be customized.

---

## Usage

To convert the contents of a XML documentation file, first reference the `Grynwald.XmlDocs.MarkdownRenderer` package in your project.

Load the documetnation file using the `DocumentationFile` class and then convert the documentation comments to a Markdown block using the `MarkdownConverter` class
(`MarkdownConverter` is based on the [Markdown Generator](https://github.com/ap0llo/markdown-generator) library):

```cs
using Grynwald.MarkdownGenerator;
using Grynwald.XmlDocs;
using Grynwald.XmlDocs.MarkdownRenderer;

// Load XML documentation file
var documentationFile = DocumentationFile.FromFile("./MyAssembly.xml");

// Convert the documentation file to a Markdown block
var converter = new MarkdownConverter();
var rootBlock = converter.ConvertToBlock(documentationFile);

// Create a Markdown document and save it to disk
var markdownDocument = new MdDocument(converter.ConvertToBlock(documentationFile));
markdownDocument.Save("./Documentation.md");
```

`MarkdownConverter` can either convert the entire documentation file or only indiviudal members or text blocks:

```cs
using Grynwald.MarkdownGenerator;
using Grynwald.XmlDocs;
using Grynwald.XmlDocs.MarkdownRenderer;

// Load XML documentation file
var documentationFile = DocumentationFile.FromFile("./MyAssembly.xml");

// Get a single text block from the documentation file
// (e.g. the summary of the first entry)
var summary = documentationFile.Members.First().Summary;

// Convert the summary to a Markdown block
var converter = new MarkdownConverter();
var rootBlock = converter.ConvertToBlock(summary);

// Create a Markdown document and save it to disk
var markdownDocument = new MdDocument(converter.ConvertToBlock(documentationFile));
markdownDocument.Save("./summary.md");
```

### Customizing the Markdown Output

Conversion to Markdown uses the Visitor pattern to traverse the structure of the documentation file.

To customize the Markdown output, you can create a customized visitor and then adapt `MarkdownConverter` to use the custom visitor:

```cs
// Create a custom visitor derived from ConvertToBlockVisitor (the default implementation)
// And override the Visit() method for the element you want to customized the output for
class CustomVisitor : ConvertToBlockVisitor
{
    public CustomVisitor(IMarkdownConverter markdownConverter) : base(markdownConverter)
    { }

    public override void Visit(ExceptionElement exception)
    {
        // TODO: Add customized logic, e.g. to resolve the reference to an exception type
        base.Visit(exception);
    }
}

// Create a custom converter derived from MarkdownConverter (the default implementation)
class CustomConverter : MarkdownConverter
{
    // Override the CreateConvertToBlockVisitor() method to use the custom visitor defined above
    protected override ConvertToBlockVisitor CreateConvertToBlockVisitor()
    {
        return new CustomVisitor(this);
    }
}
```

## License

Grynwald.XmlDocs.MarkdownRenderer is licensed under the MIT License.

For details see https://github.com/ap0llo/xmldocs/blob/master/LICENSE