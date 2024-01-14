namespace AccessibilityModels
{

    public class Onderzoek
    {
  

        public int OnderzoekId { get; set; }

        public string Titel { get; set; }

        public string KorteBeschrijving { get; set; }

        public DateTime? Datum { get; set; }

        public string? Locatie { get; set; }

        public string Beloning { get; set; }

        public string SoortOnderzoek { get; set; }//online of fysiek

        public string? StatusDeelname { get; set; }
        public Bedrijf UitvoerendBedrijf { get; set; }
        public string? UitvoerendBedrijfNaam { get; set; }
               public List<ErvaringsdeskundigenOnderzoeken>? ErvaringsdeskundigenOnderzoeken { get; set; }
//  public List<OnderzoekDeelname>? OnderzoekDeelnemers { get; set; }

//         public List<Interesse>? GeinteresseerdeErvaringsdeskundigen { get; set; }
        public string? TypeBeperking { get; set; }
        public string? PostcodeCriteria { get; set; }
        public int? MinLeeftijd { get; set; }
        public int? MaxLeeftijd { get; set; }

    }

}