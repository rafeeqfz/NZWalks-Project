using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using NZWalksAPI.CustomActionFilters;
using NZWalksAPI.Models.Domain;
using NZWalksAPI.Models.DTO;
using NZWalksAPI.Repositories;

namespace NZWalksAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IWalkRepository _walkRepository;
        public WalksController(IMapper mapper, IWalkRepository walkRepository)
        {
            this._mapper = mapper;
            this._walkRepository = walkRepository;
        }
        //Create walks
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] AddWalksRequestDto addWalksRequestDto)
        {
            //map dto to domain model
            var walkDomainModel = _mapper.Map<Walk>(addWalksRequestDto);

            await _walkRepository.CreateAsync(walkDomainModel);

            //map domain model to dto

            var walkDto = _mapper.Map<WalkDto>(walkDomainModel);

            return Ok(walkDto);

        }
        //Get All
        [HttpGet]
        //api/walks?filteron=name&&filterQuery=Track&sortBy=Name&Isassending=true&PageNumber=10
        public async Task<IActionResult> GetWalks([FromQuery] string? filterOn, [FromQuery] string? filterQuery,
            [FromQuery] string? sortBy, [FromQuery] bool? isAscendingOrder, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 1000)
        {
            //Domain model to dto
            var result = await _walkRepository.GetAllAsync(filterOn, filterQuery, sortBy, isAscendingOrder ?? true, pageNumber, pageSize);

            //create an excptoin 
          //  throw new Exception("this is a new excepto");

            _mapper.Map<List<WalkDto>>(result);

            return Ok(result);
        }

        //Get by Id
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var WalkDomainModel = await _walkRepository.GetByIdAsync(id);

            if (WalkDomainModel == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<WalkDto>(WalkDomainModel));

        }
        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        public async Task<IActionResult> Update([FromRoute] Guid id, UpdateWalkRequestDto updateWalkRequestDto)
        {
            var walkDomainModel = _mapper.Map<Walk>(updateWalkRequestDto);

            walkDomainModel = await _walkRepository.UpdateAsync(id, walkDomainModel);

            if (walkDomainModel == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<WalkDto>(walkDomainModel));
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var deleteWalksDomainModel = await _walkRepository.DeleteAsync(id);

            if (deleteWalksDomainModel == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<WalkDto>(deleteWalksDomainModel));
        }
    }
}
