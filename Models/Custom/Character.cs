using System.Net.Sockets;

namespace API_LesChevaliersEmeraude.Models.Custom
{
    public class Character
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Gender { get; set; } = null!;

        public Location? BirthPlace { get; set; }
        public Location? HomePlace { get; set; }

        public Volume? FirstAppearanceVolume { get; set; }
        public Volume? DeathVolume { get; set; }
    }
}
