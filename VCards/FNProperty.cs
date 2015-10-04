using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Du57iN.VCards
{
    public class FNProperty:PropertyBase
    {
        public FNProperty():base(PropertyNames.FN)
        {
        }

        public FNProperty(string fullName):this()
        {
            Value = fullName;
        }
    }
}
