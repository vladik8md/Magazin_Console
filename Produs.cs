namespace Magazin
{
    public class Produs
    {
        public int CodProdus { get; set; }
        public string CategorieProdus { get; set; }
        public string DenumireProdus { get; set; }
        public double PretProdus { get; set; }

        public Produs(int codProdus, string categorieProdus, string denumireProdus, double pretProdus)
        {
            CodProdus = codProdus;
            CategorieProdus = categorieProdus;
            DenumireProdus = denumireProdus;
            PretProdus = pretProdus;
        }
    }
}