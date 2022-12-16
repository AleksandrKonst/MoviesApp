using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesApp.Data;
using MoviesApp.Models;
using MoviesApp.ViewModels.Actors;

namespace MoviesApp.Controllers;

[Route("api/actors")]
[ApiController]
public class ActorsApiController: ControllerBase
{
        private readonly MoviesContext _context;
        private readonly IMapper _mapper;
        
        public ActorsApiController(MoviesContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper; 
        }
    
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ActorViewModel>))]  
        [ProducesResponseType(404)]
        public ActionResult<IEnumerable<ActorViewModel>> GetMovies()
        {
            var actors = _mapper.Map<IEnumerable<Actor>, IEnumerable<ActorViewModel>>(_context.Actors.ToList());
            return Ok(actors);
        }
        
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(ActorViewModel))]  
        [ProducesResponseType(404)]
        public IActionResult GetById(int id)
        {
            var actors = _mapper.Map<ActorViewModel>(_context.Actors.FirstOrDefault(m => m.Id == id));
            if (actors == null) return NotFound();  
            return Ok(actors);
        }
        
        [HttpPost]
        public ActionResult<InputActorsModel> PostActor(InputActorsModel inputModel)
        {
            var actor = _context.Add(_mapper.Map<Actor>(inputModel)).Entity;
            _context.SaveChanges();

            return CreatedAtAction("GetById", new { id = actor.Id }, _mapper.Map<InputActorsModel>(inputModel));
        }
        
        [HttpPut("{id}")]
        public IActionResult UpdateMovie(int id, EditActorViewModel editModel)
        {
            try
            {
                var actor = _mapper.Map<Movie>(editModel);
                actor.Id = id;
                
                _context.Update(actor);
                _context.SaveChanges();
                
                return Ok(_mapper.Map<EditActorViewModel>(actor));
            }
            catch (DbUpdateException)
            {
                if (!ActorExists(id))
                {
                    return BadRequest();
                }
                else
                {
                    throw;
                }
            }
        }
        
        [HttpDelete("{id}")] 
        public ActionResult<DeleteActorViewModel> DeleteMovie(int id)
        {
            var actor = _context.Movies.Find(id);
            if (actor == null) return NotFound();  
            _context.Movies.Remove(actor);
            _context.SaveChanges();
            return Ok(_mapper.Map<DeleteActorViewModel>(actor));
        }

        private bool ActorExists(int id)
        {
            return _context.Movies.Any(e => e.Id == id);
        }
}