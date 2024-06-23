namespace API_LesChevaliersEmeraude.Models.Custom
{
    public class Volume
    {
        public int Id { get; set; }
        public string? Series { get; set; } = null!;
        public int Number { get; set; }
        public string? Title { get; set; } = null!;
        public string? Summary { get; set; } = null!;
        public int Page { get; set; }
        public DateOnly ReleaseDate { get; set; }
        public string? ReleaseLocation { get; set; } = null!;
        public string? Isbn { get; set; } = null!;
        public string? Author { get; set; } = null!;
        public string? Editor { get; set; } = null!;
    }
}
