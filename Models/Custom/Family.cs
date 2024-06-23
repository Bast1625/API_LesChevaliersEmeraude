namespace API_LesChevaliersEmeraude.Models.Custom
{
    public class Family
    {
        public SimpleCharacter Parent1 { get; set; } = null!;
        public SimpleCharacter? Parent2 { get; set; } = null!;
        public IEnumerable<SimpleCharacter> Children { get; set; } = new List<SimpleCharacter>(); 
    }
}
