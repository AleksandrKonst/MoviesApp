using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using MoviesApp.Services;
using MoviesApp.Services.Dto;

namespace MoviesApp.Controllers;

[Route("api/actors")]
[ApiController]
public class ActorsApiController: ControllerBase
{
    private readonly IActorService _service;
        
    public ActorsApiController(IActorService service)
    {
        _service = service;
    }
    
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<ActorDto>))]  
    [ProducesResponseType(404)]
    public ActionResult<IEnumerable<ActorDto>> GetMovies()
    {
        return Ok(_service.GetAllActors());
    }
        
    [HttpGet("{id}")]
    [ProducesResponseType(200, Type = typeof(ActorDto))]  
    [ProducesResponseType(404)]
    public IActionResult GetById(int id)
    {
        var actors = _service.GetActor(id);
        if (actors == null) return NotFound();  
        return Ok(actors);
    }
        
    [HttpPost]
    public ActionResult<ActorDto> PostActor(ActorDto inputDto)
    {
        var actor = _service.AddActor(inputDto);
        return CreatedAtAction("GetById", new { id = actor.Id }, actor);
    }
        
    [HttpPut("{id}")]
    public IActionResult UpdateMovie(int id, ActorDto editDto)
    {
        var actor = _service.UpdateActor(editDto);

        if (actor==null)
        {
            return BadRequest();
        }
                
        return Ok(actor);
    }
        
    [HttpDelete("{id}")] 
    public ActionResult<ActorDto> DeleteMovie(int id)
    {
        var actor = _service.DeleteActor(id);
        if (actor == null) return NotFound();
        return Ok(actor);
    }
}