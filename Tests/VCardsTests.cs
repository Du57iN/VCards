using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Du57iN.VCards;
using System.Collections.Generic;
using System.Linq;

namespace Tests
{
    [TestClass]
    public class VCardsTests
    {
        string VCARDS = @"
BEGIN:VCARD
VERSION:3.0
N:Doe;John;test1;Mr.;San
FN:John Doe
TEL;TYPE=HOME:123456789
TEL;TYPE=Cell:987-654-321
END:VCARD

BEGIN:VCARD
VERSION:3.0
N:Doe2;;test2
FN:John Doe2
TEL;TYPE=CELL:222-222-222;
TEL;TYPE=HOME:+420111111111
END:VCARD


";


        [TestMethod]
        public void basicTests()
        {
            List<VCard> cards = new List<VCard>(VCardFileProcessor.ReadVCards(VCARDS));
            var newC = new VCard();
            newC.Properties.Add(new NProperty("doe", "john", "test1", "Mr.", "San"));
            newC.Properties.Add(new TelProperty("987654321", "Cell"));
            newC.Properties.Add(new TelProperty("+420123456789", "HOME"));
            cards.Add(newC);

            Assert.IsTrue(VCardUtils.HasSameTelNumber(cards[0], newC));
            Assert.IsTrue(VCardUtils.HasSameName(cards[0], newC));

            var simCards = VCardUtils.GetSimilarRecords(cards, VCardUtils.HasSameTelNumber);
            Assert.IsTrue(simCards.Count() == 1 && simCards.First().First() == cards[0] && simCards.First().Skip(1).First() == newC);
        }
    }
}
