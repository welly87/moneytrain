using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ForexTickAnalyzer
{
    class Quote
    {
        public Quote(string quote)
        {
            string[] qt = quote.Split(' ');
            Bid = Convert.ToDouble(qt[2]);
            Ask = Convert.ToDouble(qt[3]);
            Spread = (Ask - Bid) * 10000;
            //Bid = Convert.ToDouble(qt)
        }

        public Quote()
        {
            // TODO: Complete member initialization
        }

        public double Bid { get; set; }

        public double Ask { get; set; }

        public double Spread { get; set; }
    }
}
