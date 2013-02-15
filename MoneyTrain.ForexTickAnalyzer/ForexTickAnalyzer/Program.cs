using NDde.Client;
using System;

namespace ForexTickAnalyzer
{
    class Program
    {
        private static QuoteBeat beat;
        private static LogDataWriter writer;

        static void Main(string[] args)
        {
            var client = new DdeClient("MT4", "QUOTE");
            beat = new QuoteBeat();

            writer = new LogDataWriter("EURUSD");

            client.Advise += client_Advise;
            client.Connect();
            client.StartAdvise("EURUSD", 1, true, 60000);

            Console.ReadLine();
            writer.Close();
        }

        static void client_Advise(object sender, DdeAdviseEventArgs e)
        {
            Quote quote = new Quote(e.Text);
            beat.Add(quote);

            string log = string.Format("{0}     {1} {2} {3} {4}", e.Text, beat.DeltaBid, beat.DeltaAsk, beat.MaxUp,
                                       beat.MaxDown);

            writer.WriteLog(log);

            //Console.WriteLine("Bid: {0} Ask: {1} Spread : {2}", quote.Bid, quote.Ask, quote.Spread);
            Console.WriteLine(beat.DeltaBid > 0 ? "CALL" : "PUT");
            Console.WriteLine("Delta Bid : {0}, Ask : {1}", beat.DeltaBid, beat.DeltaAsk);
            Console.WriteLine("Max Up : {0}, Down : {1}", beat.MaxUp, beat.MaxDown);
            Console.WriteLine();
        }
    }
}
