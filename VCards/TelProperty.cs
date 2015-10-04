using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Du57iN.VCards
{
    public class TelProperty:PropertyBase
    {
        public TelProperty():base(PropertyNames.TEL)
        {
        }

        public TelProperty(string num):this()
        {
            Value = num;
        }

        public TelProperty(string num,string type): this(num)
        {
            AddParam("Type", type);
        }
        
        public override string Value
        {
            get
            {
                return base.Value;
            }
            set
            {
                base.Value = value;
                NormalizedNumber = VCardUtils.NormalizeNumber(value);
            }
        }
        
        public string NormalizedNumber
        {
            get;
            private set;
        }
    }
}
