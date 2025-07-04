using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repository;
using System.Text.Json;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContextcs dbContext;

        private readonly IRegionRepository regionRepository;

        private readonly IMapper mapper;

        private readonly ILogger<RegionsController> logger;

        public RegionsController(NZWalksDbContextcs dbContext, IRegionRepository regionRepository, IMapper mapper,ILogger<RegionsController> logger)
        {
            this.dbContext = dbContext;
            this.regionRepository = regionRepository;
            this.mapper = mapper;
            this.logger = logger;
        }


        // Get All Regions
        [HttpGet]
        //[Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetAll()
        {
            logger.LogInformation("GetAll Region Action Method was invoked");
            var regionsDomain = await regionRepository.GetAllAsync(); // Get all regions from the repository

            //var regionsDTO = new List<RegionDto>();
            //foreach (var regionDomain in regionsDomain)
            //{
            //    regionsDTO.Add(new RegionDto()
            //    {
            //        Id = regionDomain.Id,
            //        Code = regionDomain.Code,
            //        Name = regionDomain.Name,
            //        RegionImageUrl = regionDomain.RegionImageUrl
            //    });
            //}

            //Map RegionsDomain to DTO using AutoMapper
            var regionDto = mapper.Map<List<RegionDto>>(regionsDomain);
            logger.LogInformation($"Finished GetAllRegions request with data: { JsonSerializer.Serialize(regionDto) }");
            return Ok(regionDto);
        }

        // Get Region by id
        [HttpGet]
        [Route("{id:Guid}")]   // Id is coming from the Route and GuId is the type
        [Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {   // id coming from the Route so added FromRoute attribute

            var region = await regionRepository.GetByIdAsync(id); // Get region by id from the repository

            if (region == null)
            {
                return NotFound();
            }
            else
            {
                var regionDto = mapper.Map<RegionDto>(region);
                return Ok(regionDto);
            }

        }


        [HttpPost]
        [ValidateModel]  // Custome validation Model
        [Authorize(Roles = "Writer")]

        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {

            var regionDomain = mapper.Map<Region>(addRegionRequestDto);

            // Add to DbContext
            regionDomain = await regionRepository.CreateAsync(regionDomain); // Use the repository to create the region

            var regionDto = mapper.Map<RegionDto>(regionDomain);
            // Return CreatedAtAction with the created region
            return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto);
        }

        // Update Region by id
        // PUT https://localhost:portnumber/api/regions/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel] // Custome validation Model
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {

            // Map DTO to Domain Model
            var regionDomainModel = mapper.Map<Region>(updateRegionRequestDto);

            regionDomainModel = await regionRepository.UpdateAsync(id, regionDomainModel);

            if (regionDomainModel == null)
            {
                return NotFound(); // If the region is not found, return NotFound
            }


            // Cort the updated domain model to DTO
            var regionDto = mapper.Map<RegionDto>(regionDomainModel);

            // Return the updated region
            return Ok(regionDto);
        }



        //Delete Region by id
        // DELETE https://localhost:portnumber/api/regions/{id}

        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer,Reader")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {

            var regionDomainModel = await regionRepository.DeleteAsync(id); // Use the repository to delete the region  
            if (regionDomainModel == null)
            {
                return NotFound();
            }

            // return delete region back
            // map domain model to DTO
            var regionDto = mapper.Map<RegionDto>(regionDomainModel);

            // Return the updated region
            return Ok(regionDto);

            //return Ok();
        }
    }
}
