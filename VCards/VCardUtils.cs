using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Du57iN.VCards
{
    public static class VCardUtils
    {
        public static string DEFAULT_TEL_NUMBER_REGION = "CZ";
        public static PhoneNumbers.PhoneNumberFormat DEFAULT_TEL_NUMBER_FORMAT = PhoneNumbers.PhoneNumberFormat.E164;
                
        public static IEnumerable<IEnumerable<VCard>> GetSimilarRecords(IEnumerable<VCard> records,params Func<VCard,VCard,bool>[] sameDelegates)
        {
            List<List<VCard>> res=new List<List<VCard>>();

            List<VCard> unprocess = new List<VCard>(records);

            while (unprocess.Count>0)
            {
                VCard rec = unprocess[0];
                List<VCard> sameRec = new List<VCard>() { unprocess[0] };

                for (int i=1;i<unprocess.Count;i++)
                {
                    VCard recU = unprocess[i];
                    
                    bool isSame=false;
                    foreach (var sd in sameDelegates)
                    {
                        isSame = sd(rec, recU);
                        if (!isSame)
                            break;
                    }
                    
                    if (isSame)
                        sameRec.Add(recU);
                }

                sameRec.ForEach(x => unprocess.Remove(x));
                if(sameRec.Count>1)
                    res.Add(sameRec);
            }
            
            return res;
        }

        public static bool HasSameName(VCard rec1, VCard rec2)
        {
            if (rec1.N==null || rec1.N==null)
                return false;

            return string.Equals(rec1.N.Name, rec1.N.Name, StringComparison.InvariantCultureIgnoreCase)
                && string.Equals(rec1.N.GivenName, rec1.N.GivenName, StringComparison.InvariantCultureIgnoreCase)
                && string.Equals(rec1.N.AdditionalNames, rec1.N.AdditionalNames, StringComparison.InvariantCultureIgnoreCase)
                && string.Equals(rec1.N.HonorificPrefixes, rec1.N.HonorificPrefixes, StringComparison.InvariantCultureIgnoreCase)
                && string.Equals(rec1.N.HonorificSuffixes, rec1.N.HonorificSuffixes, StringComparison.InvariantCultureIgnoreCase);
        }

        public static bool HasSameTelNumber(VCard rec1,VCard rec2)
        {
            foreach(var tel1 in rec1.TelProperties)
            {
                if (rec2.TelProperties.Any(x => x.NormalizedNumber == tel1.NormalizedNumber))
                    return true;
            }
            return false;
        }

        public static string NormalizeNumber(string num)
        {
            try
            {
                var pnui = PhoneNumbers.PhoneNumberUtil.GetInstance();
                var n = pnui.Parse(num, DEFAULT_TEL_NUMBER_REGION);
                return pnui.Format(n, DEFAULT_TEL_NUMBER_FORMAT);
            }
            catch (PhoneNumbers.NumberParseException ex)
            {
                return PhoneNumbers.PhoneNumberUtil.Normalize(num);
            }
        }

    }
}
