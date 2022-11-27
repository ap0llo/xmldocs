using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grynwald.XmlDocReader;

public abstract class TextElement 
{
    public abstract override int GetHashCode();

    public abstract override bool Equals(object? obj);


}
