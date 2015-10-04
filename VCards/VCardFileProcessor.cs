using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Du57iN.VCards
{
    //http://tools.ietf.org/html/rfc2426#section-4
    public static class VCardFileProcessor
    {

        const int HTAB = 0x09;
        const int SEMICOLON = 0x3b;
        const int BACKSLASH = 0x5c;
        const int DDOT = 0x3a;
        const int PERIOD = 0x2c;
        const int SP = 0x20;
        const int CR = 0x0d;
        const int LF = 0x0a;

    
        enum State
        {
            Key,
            Params,
            Value,
            EndLine
        }

        public static IEnumerable<VCard> ReadVCards(Stream stream)
        {
            using (StreamReader r = new StreamReader(stream))
            {
                var parsedLines=GetParsedLines(r);
                return CreateVCards(parsedLines);
             }
        }

        public static IEnumerable<VCard> ReadVCards(string definition)
        {
            using (StringReader r = new StringReader(definition))
            {
                var parsedLines = GetParsedLines(r);
                return CreateVCards(parsedLines);
            }
        }

        public static void WriteVCards(Stream stream,IEnumerable<VCard> vCards)
        {
            using (StreamWriter w=new StreamWriter(stream))
            {
                foreach (var vc in vCards)
                {
                    w.Write(VCardFactory.CreateText(vc, true));
                }
            }
        }

        public static string WriteVCards(IEnumerable<VCard> vCards)
        {
            using (StringWriter w = new StringWriter())
            {
                foreach (var vc in vCards)
                {
                    w.Write(VCardFactory.CreateText(vc, true));
                }
                return w.ToString();
            }
        }

        private static IEnumerable<ParsedLine> GetParsedLines(TextReader r)
        {
            List<ParsedLine> parsedLines = new List<ParsedLine>();

            ParsedLine actParLine = new ParsedLine();
            State state = State.Key;

            int ch = r.Read();
            while (ch > -1)
            {
                switch (ch)
                {
                    case DDOT:
                        if (state == State.Key || state == State.Params)
                        {
                            state = State.Value;
                            ch = r.Read();
                        }
                        break;

                    case SEMICOLON:
                        if (state == State.Key)
                        {
                            state = State.Params;
                            ch = r.Read();
                        }
                        break;
                    case PERIOD:
                        if (state == State.Key && actParLine.Group == String.Empty)
                        {
                            actParLine.Group = actParLine.Name;
                            actParLine.Name = String.Empty;
                        }
                        break;
                    case CR:
                        ch = r.Read();
                        if (ch != LF)
                            throw new Exception("expected LF ");

                        //unfold
                        int peek = r.Peek();
                        if (peek == HTAB || peek == SP)
                        {
                            ch = r.Read();
                            ch = r.Read();
                        }
                        else
                            state = State.EndLine;
                        break;
                }

                if (state == State.EndLine)
                {
                    if (!string.IsNullOrEmpty(actParLine.Name))
                        parsedLines.Add(actParLine);
                    actParLine = new ParsedLine();
                    state = State.Key;
                }
                else if (ch > -1)
                {
                    switch (state)
                    {
                        case State.Key: actParLine.Name += (char)ch; break;
                        case State.Value: actParLine.Value += (char)ch; break;
                        case State.Params: actParLine.Params += (char)ch; break;
                    }
                }
                ch = r.Read();
            }

            return parsedLines;
        }


        private static IEnumerable<VCard> CreateVCards(IEnumerable<ParsedLine> parsedLines)
        {
            List<VCard> res = new List<VCard>();

            VCard rec = null;
            foreach (var pl in parsedLines)
            {
                if (pl.Name == "BEGIN" && pl.Value == "VCARD")
                    rec = new VCard();
                else if (pl.Name == "END" && pl.Value == "VCARD")
                {
                    res.Add(rec);
                }
                else if (rec != null)
                {
                    rec.Properties.Add(VCardFactory.CreateProperty(pl.Name, pl.Group, pl.Params, pl.Value));
                }
            }

            return res;
        }

       
    }
}
