namespace AccessibilityModels
{

    public class Hulpmiddel
    {
        public int HulpmiddelId { get; set; }
        public string HulpmiddelNaam { get; set; }
         public List<GebruikerHulpmiddel> GebruikerHulpmiddelen { get; set; }
    }

}