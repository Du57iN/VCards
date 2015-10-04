using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Du57iN.VCards
{
    public class PropertyBase:IProperty
    {
        public PropertyBase(string key)
        {
            Name = key;
            Params = new List<Parameter>();
        }
        
        public string Name { get; private set; }

        public string Group { get; set; }

        public virtual string Value { get; set; }

        public IList<Parameter> Params { get; set; }

        public void AddParam(string name,string value)
        {
            Parameter firstP=null;
            foreach (var p in Params.Where(x=>x.Name==name))
            {
                if (firstP == null)
                    firstP = p;
                if (p.Values.Contains(value))
                    return;
            }

            if (firstP!=null)
            {
                firstP.Values.Add(value);
            }
            else
            {
                Params.Add(new Parameter(name, value));
            }
            
        }

        public override string ToString()
        {
            return VCardFactory.CreateText(this,false);
        }
    }
}
