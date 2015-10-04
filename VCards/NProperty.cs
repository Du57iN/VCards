using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Du57iN.VCards
{
    public class NProperty :PropertyBase
    {
        //Family Name, Given Name, Additional Names, Honorific Prefixes, and Honorific Suffixes.
        private string[] _def = new string[5];

        public NProperty() : base(PropertyNames.N)
        { 
        }

        public NProperty(string familyName,string givenName,string additionalNames,string honoricPrefixes,string honoricSuffixes):this()
        {
            FamilyName = familyName;
            GivenName = givenName;
            AdditionalNames = additionalNames;
            HonorificPrefixes = honoricPrefixes;
            HonorificSuffixes = honoricSuffixes;
        }

        private void SetValue(string definition)
        {
            var split= definition.Split(';');
            for (int i = 0; i < 5; i++)
            {
                _def[i] = i < split.Length ? split[i] : null;
            }
        }

        private string GetValue()
        {
            return string.Join(";", _def);
        }

        public override string Value
        {
            get { return GetValue(); }
            set { SetValue(value); }
        }

        public string FamilyName { get { return _def[0]; } set { _def[0] = value; } }
        public string GivenName { get { return _def[1]; } set { _def[1] = value; } }
        public string AdditionalNames { get { return _def[2]; } set { _def[2] = value; } }
        public string HonorificPrefixes { get { return _def[3]; } set { _def[3] = value; } }
        public string HonorificSuffixes { get { return _def[4]; } set { _def[4] = value; } }


    }
}
