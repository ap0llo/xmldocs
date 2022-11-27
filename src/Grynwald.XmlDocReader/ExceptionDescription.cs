namespace Grynwald.XmlDocReader
{
    public class ExceptionDescription
    {

        public string Cref { get; }

        //TODO: TextBlock description


        public ExceptionDescription(string cref)
        {
            if (String.IsNullOrWhiteSpace(cref))
                throw new ArgumentException("Value must not be null or whitespace", nameof(cref));

            Cref = cref;
        }


        public static ExceptionDescription FromXml(XElement xml)
        {
            xml.EnsureNameIs("exception");

            var cref = xml
                .RequireAttribute("cref")
                .RequireValue();

            return new ExceptionDescription(cref);
        }
    }
}
