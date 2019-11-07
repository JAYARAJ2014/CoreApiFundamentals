using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CoreCodeCamp.Data;
using CoreCodeCamp.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace src.Controllers
{

    [Route("api/[controller]")]
    public class CampsController : ControllerBase
    {
        private ICampRepository _campRepository;
        private IMapper _mapper;

        public CampsController(ICampRepository campRepo, IMapper mapper)
        {
            _campRepository = campRepo;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<ActionResult<CampDto[]>> GetCamps()
        {
            try
            {
                var result = await _campRepository.GetAllCampsAsync();
                CampDto[] camps = _mapper.Map<CampDto[]>(result);
                return camps;
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Failure retrieving camps");
            }
        }

        [HttpGet("{moniker}")]
        public async Task<ActionResult<CampDto>> GetCamp(string moniker)
        {
            try
            {
                var result = await _campRepository.GetCampAsync(moniker);
                if (result == null) NotFound();
                return _mapper.Map<CampDto>(result);
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Failure retrieving camps");
            }
        }

        [HttpGet("search")]
        public async Task<ActionResult<CampDto[]>> SearchByDate(DateTime theDate, bool includeTalks = false)
        {

            try
            {
                var results = await _campRepository.GetAllCampsByEventDate(theDate, includeTalks);
                if (!results.Any()) return NotFound();

                return _mapper.Map<CampDto[]>(results);
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Failure retrieving camps");
            }
        }
    }
}