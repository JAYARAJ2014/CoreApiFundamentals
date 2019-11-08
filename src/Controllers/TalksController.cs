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

        [HttpGet("{id:int}")]
        public async Task<ActionResult<TalkDto>> GetTalk(string moniker, int id)
        {
            try
            {
                var talk = await _campRepository.GetTalkByMonikerAsync(moniker, id);
                return _mapper.Map<TalkDto>(talk);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Failure retrieving camps\n" + ex.Message + "\n" + ex.InnerException ?? "");
            }
        }

        [HttpPost]
        public async Task<ActionResult<TalkDto>> Post(string moniker, TalkDto model)
        {
            try
            {
                var camp = await _campRepository.GetCampAsync(moniker);
                if (camp == null) return BadRequest("Camp does not exist!");

                var talk = _mapper.Map<Talk>(model);
                talk.Camp = camp;

                if (model.Speaker == null) return BadRequest("Speaker Id is rquired");
                var speaker = await _campRepository.GetSpeakerAsync(model.Speaker.SpeakerId);
                if (speaker == null) return BadRequest("Speaker could not be found");
                talk.Speaker = speaker;

                _campRepository.Add(talk);
                _logger.LogInformation("Talk added to collection");

                if (await _campRepository.SaveChangesAsync())
                {
                    var url = _linkGenerator.GetPathByAction(HttpContext, "GetTalk", values: new { moniker, id = talk.TalkId });
                    return Created(url, _mapper.Map<TalkDto>(talk));
                }
                else
                {
                    return BadRequest("Failed to save talk");
                }
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Failure retrieving camps\n" + ex.Message + "\n" + ex.InnerException ?? "");
            }
        }


        [HttpPut("{id}")]
        public async Task<ActionResult<TalkDto>> Put(string moniker, int id, TalkDto model)
        {
            try
            {
                var talk = await _campRepository.GetTalkByMonikerAsync(moniker, id, true);
                if (talk == null) return BadRequest("Talk does not exist!");

                if (model.Speaker != null)
                {
                    var speaker = await _campRepository.GetSpeakerAsync(model.Speaker.SpeakerId);

                    if (speaker != null)
                    {
                        talk.Speaker = speaker;
                    }
                }
                _mapper.Map(model, talk);

                if (await _campRepository.SaveChangesAsync())
                {
                    return _mapper.Map<TalkDto>(talk);
                }
                else
                {
                    return BadRequest("Failed to update");
                }
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Failure updating talk" + ex.Message + "\n" + ex.InnerException ?? "");
            }
        }

    }
}