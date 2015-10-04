using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Du57iN.VCards
{
    public class VCard
    {
        public VCard()
        {
            Properties = new List<IProperty>();
        }

        public IList<IProperty> Properties { get; private set; }

        public NProperty N { get { return (NProperty)Properties.FirstOrDefault(x => x.Name == PropertyNames.N); } }
        public FNProperty FN { get { return (FNProperty)Properties.FirstOrDefault(x => x.Name == PropertyNames.FN); } }

        public IEnumerable<TelProperty> TelProperties
        {
            get { return Properties.Where(x => x.Name == PropertyNames.TEL).Cast<TelProperty>(); }
        }

      
        public override string ToString()
        {
            return VCardFactory.CreateText(this, false);
        }
    }

}

