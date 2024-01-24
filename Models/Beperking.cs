namespace AccessibilityModels
{

    public class Beperking
    {
        public int BeperkingId { get; set; }
        public string BeperkingNaam { get; set; }
         public List<GebruikerBeperkingen> GebruikerBeperkingen { get; set; }
    }

}