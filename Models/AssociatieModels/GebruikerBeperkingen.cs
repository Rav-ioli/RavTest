using AccessibilityModels;
namespace AccessibilityModels;
public class GebruikerBeperkingen
{
    public string ErvaringsdeskundigeId { get; set; }
        public Ervaringsdeskundige Ervaringsdeskundige { get; set; }

    public int BeperkingId { get; set; }
    public Beperking Beperking { get; set; }
}
