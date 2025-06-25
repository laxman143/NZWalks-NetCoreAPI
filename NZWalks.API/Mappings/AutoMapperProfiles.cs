using AutoMapper;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Mappings
{
    public class AutoMapperProfiles : Profile
    {

        public AutoMapperProfiles()
        {
            CreateMap<Region,RegionDto>().ReverseMap(); // Maps from Region to RegionDto and vice versa 
            CreateMap<AddRegionRequestDto,Region>().ReverseMap(); // Maps from AddRegionRequestDto to Region and vice versa
            CreateMap<UpdateRegionRequestDto, Region>().ReverseMap(); // Maps from UpdateRegionRequestDto to Region and vice versa

            CreateMap<AddWalkRequestDto,Walk>().ReverseMap(); // Maps from AddWalkRequestDto to Walk and vice versa
            CreateMap<Walk, WalkDto>().ReverseMap(); // Maps from Walk to WalkDto

            CreateMap<Difficulty, DifficultyDto>().ReverseMap(); // Maps from Difficulty to DifficultyDto and vice versa

            CreateMap<UpdateWalkRequestDto, Walk>().ReverseMap(); // Maps from Walk to AddWalkRequestDto and vice versa


        }
    }
}
