using NZWalks.API.Models.Domain;

namespace NZWalks.API.Models.DTO
{
    public class WalkDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public Double LengthInKm { get; set; }

        public string? WalkImageUrl { get; set; }

        //public Guid DifficultyId { get; set; }

        //public Guid RegionId { get; set; }

        //Above both is commented out because we are using navigation properties instead on below

        public RegionDto Region { get; set; }

        public DifficultyDto Difficulty { get; set; }
    }
}
