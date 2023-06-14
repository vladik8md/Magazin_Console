namespace Magazin
{
    public class Client
    {
        public string CodClient { get; set; }
        public string NumeClient { get; set; }
        public string PrenumeClient { get; set; }
        public double SumaClient { get; set; }

        public Client(string codClient, string numeClient, string prenumeClient, double sumaClient)
        {
            CodClient = codClient;
            NumeClient = numeClient;
            PrenumeClient = prenumeClient;
            SumaClient = sumaClient;
        }
    }
}