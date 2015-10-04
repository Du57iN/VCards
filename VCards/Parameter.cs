using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Du57iN.VCards
{
    public class Parameter
    {
        public Parameter(string name,string value):this(name,new string[]{value})
        {
        }

        public Parameter(string name, IEnumerable<string> values)
        {
            Name = name;
            Values = new List<string>(values);
        }
        public string Name { get; set; }

        public List<string> Values { get; set; }
        

    }
}
