namespace API_LesChevaliersEmeraude.Models.Custom
{
    public class SimpleCharacter
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Gender { get; set; } = null!;
        public string? BirthPlace { get; set; } = null!;
        public string? HomePlace { get; set; } = null!;
    }
}
