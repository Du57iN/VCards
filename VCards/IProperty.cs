using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Du57iN.VCards
{
    public interface IProperty
    {
        string Name { get; }
        string Group { get; set; }
        string Value { get; set; }
        IList<Parameter> Params { get; set; }
    }
}
