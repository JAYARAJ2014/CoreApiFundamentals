using System;
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

    [ApiController]
    [Route("api/camps/{moniker}/talks")]
    public class TalksController : ControllerBase
    {
        private ICampRepository _campRepository;
        private IMapper _mapper;
        private LinkGenerator _linkGenerator;
        private ILogger _logger;

        public TalksController(ICampRepository campRepo
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
        public async Task<ActionResult<TalkDto[]>> GetTalks(string moniker)
        {
            try
            {
                var talks = await _campRepository.GetTalksByMonikerAsync(moniker);
                return _mapper.Map<TalkDto[]>(talks);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Failure retrieving camps\n" + ex.Message + "\n" + ex.InnerException ?? "");
            }
        }

    }
}