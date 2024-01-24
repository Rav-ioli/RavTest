using AccessibilityModels;
namespace AccessibilityModels;
public class GebruikerHulpmiddel
{
      public string ErvaringsdeskundigeId { get; set; }
        public Ervaringsdeskundige Ervaringsdeskundige { get; set; }

    public int HulpmiddelId { get; set; }
    public Hulpmiddel Hulpmiddel { get; set; }
}
