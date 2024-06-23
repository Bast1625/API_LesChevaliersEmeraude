namespace API_LesChevaliersEmeraude.Models.Custom
{
    public class Royalty
    {
        public Location Location { get; set; } = null!;
        public SimpleCharacter? Sovereign1 { get; set; } = null!;
        public SimpleCharacter? Sovereign2 { get; set; } = null!;
        public int SuccessionOrder { get; set; }
    }
}
