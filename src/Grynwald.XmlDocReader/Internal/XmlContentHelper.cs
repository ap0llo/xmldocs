// The code in this file is derived from the "DocReader" class from the NuDoq project.
// The original version of this file was downloaded from
// https://github.com/kzu/NuDoq/blob/56ad8c508003490d859214753591440b123616f5/src/NuDoq/DocReader.cs
//
// See the original license below for details
//
#region Apache Licensed
/*
 Copyright 2013 Daniel Cazzulino

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

     http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
*/
#endregion

//TODO: Also link to MdDocs

namespace Grynwald.XmlDocReader.Internal;

internal class XmlContentHelper
{

    public static int GetIndentation(string content)
    {
        //  When inline documentation is written to a XML file by the compiler, 
        //  all text is indented by the same number of character:
        //
        //  Entire block is indented
        //    │
        //    │    <member name="F:DemoProject.DemoClass.Field1">             Empty leading line  
        //    │       <remarks>                                         <───────┘                        
        //    └───>     Remarks allow specification of more detailed information about a member, in this case a field
        //              supplementing the information specified in the summary
        //  ┌───>     </remarks>
        //  │     </member>
        //  │
        //  │
        //  Empty trailing line
        //
        //  In order to properly render the documentation, the leading whitespace
        //  needs to be removed. 
        //  
        //  This methods determines the number of character that have to be removed
        //  from all lines by 
        //    - removing the first line (probably only whitespace, as can be seen in the example above)
        //    - removing the last line (probably only whitespace, as can be seen in the example above)
        //    - counting the number of whitespace characters in the first non-whitespace line
        //


        var lines = content.Split(new[] { Environment.NewLine, "\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();

        if (lines.Count == 0)
            return 0;

        // Remove leading and trailing empty lines which are used for wrapping in the doc XML.
        if (lines[0].Trim().Length == 0)
            lines.RemoveAt(0);

        if (lines.Count == 0)
            return 0;

        if (lines[lines.Count - 1].Trim().Length == 0)
            lines.RemoveAt(lines.Count - 1);

        if (lines.Count == 0)
            return 0;

        // The indent of the first line of content determines the base 
        // indent for all the lines, which we should remove since it's just 
        // a doc gen artifact.            
        return lines[0].TakeWhile(c => Char.IsWhiteSpace(c)).Count();

    }

    /// <summary>
    /// Trims the text by removing new lines and trimming the indent.
    /// </summary>
    public static string TrimText(string content, int indent) => TrimLines(content, StringSplitOptions.RemoveEmptyEntries, " ", indent);

    /// <summary>
    /// Trims code by removing extra indent.
    /// </summary>
    public static string TrimCode(string content, int indent) => TrimLines(content, StringSplitOptions.None, Environment.NewLine, indent);


    public static string TrimLines(string content, StringSplitOptions splitOptions, string joinWith, int indent)
    {
        var lines = content.Split(new[] { Environment.NewLine, "\n" }, splitOptions).ToList();

        if (lines.Count == 0)
            return String.Empty;

        // Remove leading and trailing empty lines which are used for wrapping in the doc XML.
        if (lines[0].Trim().Length == 0)
            lines.RemoveAt(0);

        if (lines.Count == 0)
            return String.Empty;

        if (lines[^1].Trim().Length == 0)
            lines.RemoveAt(lines.Count - 1);

        if (lines.Count == 0)
            return String.Empty;

        // Indent in generated XML doc files is greater than 4 always. 
        // This allows us to optimize the case where the author actually placed 
        // whitespace inline in between tags.
        if (indent <= 4 && !String.IsNullOrEmpty(lines[0]) && lines[0][0] != '\t')
            indent = 0;

        return String.Join(joinWith, lines
            .Select(line =>
            {
                if (String.IsNullOrWhiteSpace(line))
                    return String.Empty;
                // remove indentation from line, when it starts with <indent> whitespace characters
                else if (indent > 0 && indent <= line.Length && line.Substring(0, indent).Trim().Length == 0)
                    return line.Substring(indent);
                // line has non-whitespace content within the indentation => return it unchanged
                else
                    return line;
            })
            .ToArray());
    }


    public static XDocument ParseXmlDocument(string xml)
    {
        XDocument document;
        try
        {
            document = XDocument.Parse(xml, LoadOptions.PreserveWhitespace | LoadOptions.SetLineInfo);
        }
        catch (XmlException ex)
        {
            throw new XmlDocReaderException("Failed to parse XML from string", ex);
        }
        return document;
    }

    public static XElement ParseXmlElement(string xml)
    {
        XElement parsedXml;
        try
        {
            parsedXml = XElement.Parse(xml, LoadOptions.PreserveWhitespace | LoadOptions.SetLineInfo);
        }
        catch (XmlException ex)
        {
            throw new XmlDocReaderException("Failed to parse XML from string", ex);
        }

        return parsedXml;
    }
}
