using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CoreCodeCamp.Data;
using CoreCodeCamp.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;

namespace src.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class CampsController : ControllerBase
    {
        private ICampRepository _campRepository;
        private IMapper _mapper;
        private LinkGenerator _linkGenerator;
        private ILogger _logger;

        public CampsController(ICampRepository campRepo
        , IMapper mapper
        , LinkGenerator linkGenerator
        , ILogger<CampsController> logger)
        {
            _campRepository = campRepo;
            _mapper = mapper;
            _logger = logger;
            _linkGenerator = linkGenerator;
        }


        [HttpGet]
        public async Task<ActionResult<CampDto[]>> GetCamps(bool includeTalks = false)
        {
            try
            {
                var result = await _campRepository.GetAllCampsAsync(includeTalks);
                CampDto[] camps = _mapper.Map<CampDto[]>(result);
                return camps;
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Failure retrieving camps\n" + ex.Message + "\n" + ex.InnerException ?? "");
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


        [HttpPost]
        public async Task<ActionResult<CampDto>> Post(CampDto model)
        {

            try
            {
                var location = _linkGenerator.GetPathByAction("GetCamp", "Camps", new {moniker=model.Moniker});
                 _logger.LogInformation($"{location}");
                 
                if(string.IsNullOrWhiteSpace(location)){
                    return BadRequest("Could not use current moniker");
                }
                _logger.LogInformation($"model.Moniker:{model.Moniker},  model.Name:{model.Name}");
                var camp = _mapper.Map<Camp>(model);
                _logger.LogInformation("Mapping done");
                _campRepository.Add(camp);
                if (await _campRepository.SaveChangesAsync())
                {
                    return Created("", _mapper.Map<CampDto>(camp));
                }
                return BadRequest();
            }
            catch (System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Failed to create Camp" + ex.StackTrace);
            }
        }
    }
}