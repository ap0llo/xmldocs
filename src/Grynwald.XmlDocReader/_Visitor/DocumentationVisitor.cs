namespace Grynwald.XmlDocReader
{
    /// <summary>
    /// Default implementation of <see cref="IDocumentationVisitor"/>
    /// </summary>
    public class DocumentationVisitor : IDocumentationVisitor
    {
        /// <inheritdoc />
        public virtual void Visit(DocumentationFile documentationFile)
        {
            foreach (var member in documentationFile.Members)
            {
                member.Accept(this);
            }
        }

        /// <inheritdoc />
        public virtual void Visit(MemberDescription member)
        {
            member.Summary?.Accept(this);
            member.Remarks?.Accept(this);
            member.Value?.Accept(this);
            member.Returns?.Accept(this);
            member.Example?.Accept(this);


            foreach (var parameter in member.Parameters)
            {
                parameter.Accept(this);
            }

            foreach (var typeParameter in member.TypeParameters)
            {
                typeParameter.Accept(this);
            }

            foreach (var seeAlso in member.SeeAlso)
            {
                seeAlso.Accept(this);
            }

            foreach (var exception in member.Exceptions)
            {
                exception.Accept(this);
            }
        }

        /// <inheritdoc />
        public virtual void Visit(TextBlock textBlock)
        {
            foreach (var element in textBlock.Elements)
            {
                element.Accept(this);
            }
        }

        /// <inheritdoc />
        public virtual void Visit(ParameterDescription parameter)
        {
            parameter.Text?.Accept(this);
        }

        /// <inheritdoc />
        public virtual void Visit(TypeParameterDescription typeParameter)
        {
            typeParameter.Text?.Accept(this);
        }

        /// <inheritdoc />
        public virtual void Visit(ExceptionDescription exception)
        {
            exception.Text?.Accept(this);
        }

        /// <inheritdoc />
        public virtual void Visit(ListElement list)
        {
            list.ListHeader?.Accept(this);
            foreach(var item in list.Items)
            {
                item.Accept(this);
            }
        }

        /// <inheritdoc />
        public virtual void Visit(CElement c)
        {
        }

        /// <inheritdoc />
        public virtual void Visit(CodeElement code)
        {
        }

        /// <inheritdoc />
        public virtual void Visit(ListItemElement item)
        {
            item.Term?.Accept(this);
            item.Description.Accept(this);
        }

        /// <inheritdoc />
        public virtual void Visit(ParagraphElement para)
        {
            para.Content?.Accept(this);
        }

        /// <inheritdoc />
        public virtual void Visit(ParameterReferenceElement paramRef)
        {
        }

        /// <inheritdoc />
        public virtual void Visit(PlainTextElement plainText)
        {
        }

        /// <inheritdoc />
        public virtual void Visit(SeeCodeReferenceElement see)
        {
            see.Text?.Accept(this);
        }

        /// <inheritdoc />
        public virtual void Visit(SeeUrlReferenceElement see)
        {
            see.Text?.Accept(this);            
        }

        /// <inheritdoc />
        public virtual void Visit(TypeParameterReferenceElement typeParamRef)
        {
        }

        /// <inheritdoc />
        public virtual void Visit(SeeAlsoUrlReferenceDescription seeAlso)
        {
            seeAlso.Text?.Accept(this);
        }

        /// <inheritdoc />
        public virtual void Visit(SeeAlsoCodeReferenceDescription seeAlso)
        {
            seeAlso.Text?.Accept(this);
        }
    }
}
