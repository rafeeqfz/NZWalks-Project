
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalksAPI.CustomActionFilters;
using NZWalksAPI.Data;
using NZWalksAPI.Models.Domain;
using NZWalksAPI.Models.DTO;
using NZWalksAPI.Repositories;
using System.Text.Json;

namespace NZWalksAPI.Controllers
{
    //https:2342/api/region
    [Route("api/[controller]")]

    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext _context;
        private readonly IRegionRepository _repositories;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        public RegionsController(NZWalksDbContext dbContext, IRegionRepository regionRepository, IMapper mapper, ILogger<RegionsController> logger)
        {
            this._context = dbContext;
            this._repositories = regionRepository;
            this._mapper = mapper;
            this._logger = logger;


        }
        [HttpGet]
        //  [Authorize(Roles ="Reader")]
        public async Task<IActionResult> GetAll()
        {
        
                //Get data from database -Domain model
               // throw new Exception("Custom excpetion from get all method------------");
                var regionsDomainModel = await _repositories.GetAllAsync();
                _logger.LogInformation($"Finished Get all Action method request data :{JsonSerializer.Serialize(regionsDomainModel)}");
                //map Domain models to DTO

                var regionDto = _mapper.Map<List<RegionDto>>(regionsDomainModel);
                //Return DTOs
                return Ok(regionDto);
           
        }

        [HttpGet]
        [Route("{id:Guid}")]
       // [Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var reg = await _repositories.GetByIdAsync(id);

            if (reg == null)
            {
                return NotFound();
            }
            var regionDto = _mapper.Map<RegionDto>(reg);

            return Ok(regionDto);
        }

        [HttpPost]
        [ValidateModel]
      //  [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            // map or convert dto to model domain
            var regionDomainModel = _mapper.Map<Region>(addRegionRequestDto);

            await _repositories.CreateAsync(regionDomainModel);
            _context.SaveChangesAsync();

            //convert domain to dto
            var regionDto = _mapper.Map<RegionDto>(regionDomainModel);
            return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto);

        }

        [HttpPut]
        [Route("{id:Guid}")]
       // [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            var regionDomainModel = _mapper.Map<Region>(updateRegionRequestDto);
            regionDomainModel = await _repositories.UpdateAsync(id, regionDomainModel);

            if (regionDomainModel == null)
            {
                return NotFound();
            }
            await _context.SaveChangesAsync();

            var regionDto = _mapper.Map<RegionDto>(regionDomainModel);
            return Ok(regionDto);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
     //   [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var regionDomainModel = await _repositories.DeleteAsync(id);
            if (regionDomainModel == null)
            {
                return NotFound();
            }
            var regionDto = _mapper.Map<RegionDto>(regionDomainModel);

            return Ok(regionDto);
        }

    }
}
