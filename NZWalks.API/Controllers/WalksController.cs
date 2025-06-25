using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repository;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IWalkRepository walkRepository;

        public WalksController(IMapper mapper, IWalkRepository walkRepository)
        {
            this.mapper = mapper;
            this.walkRepository = walkRepository;
        }
        // CREATE Walk 
        // POST: /api/walks
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddWalkRequestDto addWalkRequestDto)
        {
            if (ModelState.IsValid) {
                // Map DTO to Domain Model
                var walkDomainModel = mapper.Map<Walk>(addWalkRequestDto);

                await walkRepository.CreateAsync(walkDomainModel);

                var walkDtoModel = mapper.Map<WalkDto>(walkDomainModel);
                // Map Domain Model to DTO
                return Ok(walkDtoModel);
            } else
            {
                return BadRequest(ModelState); // Return 400 Bad Request if model state is invalid
            }
        }


        [HttpGet]
        // GET: /api/walks?filterOn=Name&filterQuery=Track&SortBy=Name&isAcending=true&PageNumber=1&PageSize=10
        public async Task<IActionResult> GetAll([FromQuery] string? filterOn, 
            [FromQuery] string? filterQuery, [FromQuery] string? sortBy, [FromQuery] bool? isAscending,
            [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 1000)
        {
            // Get all walks from the repository
            var walksDomain = await walkRepository.GetAllAsync(filterOn,filterQuery,sortBy,isAscending ?? true, pageNumber, pageSize);
            // Map Domain Model to DTO
            var walksDto = mapper.Map<List<WalkDto>>(walksDomain);
            return Ok(walksDto);

        }

        // Get Walk by Id
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var walkDomain = await walkRepository.GetByIdAsync(id);
            if (walkDomain == null)
            {
                return NotFound(); // Return 404 if walk not found
            }
            // Map Domain Model to DTO
            var walkDto = mapper.Map<WalkDto>(walkDomain);
            return Ok(walkDto); // Return 200 OK with the walk DTO

        }

        // Update Walk by Id
        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, UpdateWalkRequestDto updateWalkRequestDto)
        {
            // Map DTO to Domain Model
            if (ModelState.IsValid)
            {
                var walkDomainModel = mapper.Map<Walk>(updateWalkRequestDto);
                walkDomainModel = await walkRepository.UpdateAsync(id, walkDomainModel);
                if (walkDomainModel == null)
                {
                    return NotFound();
                }
                return Ok(mapper.Map<WalkDto>(walkDomainModel));
            }
            return BadRequest(ModelState); // Return 400 Bad Request if model state is invalid

        }

        // Delete Walk by Id
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var walkDomainModel = await walkRepository.DeleteAsync(id);
            if (walkDomainModel == null)
            {
                return NotFound();
            }
            // Delete the walk
           
            return Ok(mapper.Map<WalkDto>(walkDomainModel)); // Return 200 OK on successful deletion
        }
    }
}
