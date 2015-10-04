using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Du57iN.VCards
{
    //http://tools.ietf.org/html/rfc2426
    public static class VCardFactory
    {
        const int FOLDLENGTH = 74;

        public static IProperty CreateProperty(string key)
        {
            switch (key)
            {
                case PropertyNames.N: return new NProperty();
                case PropertyNames.FN: return new FNProperty();
                case PropertyNames.TEL: return new TelProperty();
                default: return new PropertyBase(key);
            }
        }

        public static IProperty CreateProperty(string key,string keyGroup,string @params,string value)
        {
            IProperty prop=CreateProperty(key);
            prop.Group = keyGroup;
            prop.Value = value;

            if (!string.IsNullOrEmpty(@params))
            {
                foreach (var p in @params.Split(';'))
                {
                    var pv = p.Split('=');
                    prop.Params.Add(new Parameter(pv[0], pv[1].Split(',')));
                }
            }
        

            return prop;
        }


        public static string CreateText(IProperty prop, bool foldLines)
        {
            string res = (!string.IsNullOrWhiteSpace(prop.Group) ? prop.Group + "." : String.Empty)
                        + prop.Name +
                        string.Join(";",prop.Params.Select(x=>x.Name+"="+string.Join(",",x.Values)))
                        + ":"
                        + prop.Value;

            if (foldLines)
            {
                int i = 0;
                while (res.Length - i > FOLDLENGTH)
                {
                    res = res.Insert(i + FOLDLENGTH, "\r\n ");
                    i = i + FOLDLENGTH + 2;
                }
            }
            return res;
        }

        public static string CreateText(VCard rec, bool foldLines)
        {
            return "BEGIN:VCARD\r\n" +
                    string.Join("\r\n", rec.Properties.Select(x => CreateText(x, foldLines)))
                    + "\r\nEND:VCARD\r\n";
        }

       
    }
}
