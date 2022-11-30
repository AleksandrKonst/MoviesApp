using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MoviesApp.Data;
using MoviesApp.Models;
using MoviesApp.ViewModels.Actors;

namespace MoviesApp.Controllers;

public class ActorsController : Controller
{
    private readonly MoviesContext _context;
    private readonly ILogger<HomeController> _logger;
    private readonly IMapper _mapper;


    public ActorsController(MoviesContext context, ILogger<HomeController> logger, IMapper mapper)
    {
        _context = context;
        _logger = logger;
        _mapper = mapper;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var actors = _mapper.Map<IEnumerable<Actor>, IEnumerable<ActorViewModel>>(_context.Actors.ToList());
        return View(actors);
    }

    [HttpGet]
    public IActionResult Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var viewModel = _mapper.Map<ActorViewModel>(_context.Actors.FirstOrDefault(m => m.Id == id));

        if (viewModel == null)
        {
            return NotFound();
        }

        return View(viewModel);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create([Bind("Name,Surname,DateOfBirth")] InputActorsModel inputModel)
    {
        if (ModelState.IsValid)
        {
            _context.Add(_mapper.Map<Actor>(inputModel));
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
        return View(inputModel);
    }
        
    [HttpGet]
    public IActionResult Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var editModel = _mapper.Map<EditActorViewModel>(_context.Actors.FirstOrDefault(m => m.Id == id));

        if (editModel == null)
        {
            return NotFound();
        }
            
        return View(editModel);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, [Bind("Name,Surname,DateOfBirth")] EditActorViewModel editModel)
    {
        if (ModelState.IsValid)
        {
            try
            {
                var actor = _mapper.Map<Actor>(editModel);
                actor.Id = id;
                _context.Update(actor);
                _context.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (!MovieExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(editModel);
    }
        
    [HttpGet]
    public IActionResult Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var deleteModel = _mapper.Map<DeleteActorViewModel>(_context.Actors.FirstOrDefault(m => m.Id == id));

        if (deleteModel == null)
        {
            return NotFound();
        }

        return View(deleteModel);
    }
        
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        var actor = _context.Actors.Find(id);
        _context.Actors.Remove(actor);
        _context.SaveChanges();
        _logger.LogError($"Actor with id {actor.Id} has been deleted!");
        return RedirectToAction(nameof(Index));
    }

    private bool MovieExists(int id)
    {
        return _context.Movies.Any(e => e.Id == id);
    }
}