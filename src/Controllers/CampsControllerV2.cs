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

namespace CoreCodeCamp.Controllers
{

    [Route("api/camps")]
    [ApiVersion("2.0")]
    [ApiController]
    public class CampsControllerV2 : ControllerBase
    {
        private ICampRepository _campRepository;
        private IMapper _mapper;
        private LinkGenerator _linkGenerator;
        private ILogger _logger;

        public CampsControllerV2(ICampRepository campRepo
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
        public async Task<IActionResult> GetCamps(bool includeTalks = false)
        {
            try
            {
                var results = await _campRepository.GetAllCampsAsync(includeTalks);
                var result = new {
                    Camps = _mapper.Map<CampDto[]>(results),
                    Count = results.Count()
                };
               
                return Ok(result);
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

                var existing = await _campRepository.GetCampAsync(model.Moniker);
                if (existing != null)
                {
                    return BadRequest("Moniker already in use");
                }
                var location = _linkGenerator.GetPathByAction("GetCamp", "Camps", new { moniker = model.Moniker });
                _logger.LogInformation($"{location}");

                if (string.IsNullOrWhiteSpace(location))
                {
                    return BadRequest("Could not use current moniker");
                }
                _logger.LogInformation($"model.Moniker:{model.Moniker},  model.Name:{model.Name}");
                var camp = _mapper.Map<Camp>(model);
                _logger.LogInformation("Mapping done");
                _campRepository.Add(camp);
                if (await _campRepository.SaveChangesAsync())
                {
                    return Created(location, _mapper.Map<CampDto>(camp));
                }
                return BadRequest();
            }
            catch (System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Failed to create Camp" + ex.StackTrace);
            }
        }




        [HttpPut("{moniker}")]
        public async Task<ActionResult<CampDto>> Put(String moniker, CampDto model)
        {
            try
            {
                var existing = await _campRepository.GetCampAsync(model.Moniker);
                if (existing == null)
                {
                    return NotFound($"Could not find Camp with Moniker: {moniker} ");
                }
                _mapper.Map(model, existing);
                if (await _campRepository.SaveChangesAsync())
                {
                    return _mapper.Map<CampDto>(existing);
                }

                return BadRequest("There was an issue in updating ");
            }
            catch (System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Failed to create Camp" + ex.StackTrace);
            }
        }


        [HttpDelete("{moniker}")]
        public async Task<IActionResult> Delete(String moniker)
        {
            try
            {
                var existing = await _campRepository.GetCampAsync(moniker);
                if (existing == null)
                {
                    return NotFound($"Could not find Camp with Moniker: {moniker} ");
                }
                _campRepository.Delete(existing);

                if (await _campRepository.SaveChangesAsync())
                {
                    return Ok();
                }

            }
            catch (System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Failed to create Camp" + ex.StackTrace);
            }
            return BadRequest("There was an issue in deleting ");
        }
    }
}