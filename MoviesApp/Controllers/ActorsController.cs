using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MoviesApp.Data;
using MoviesApp.Models;
using MoviesApp.ViewModels.Actors;

namespace MoviesApp.Controllers;

public class Actors : Controller
{
    private readonly MoviesContext _context;
    private readonly ILogger<HomeController> _logger;


    public Actors(MoviesContext context, ILogger<HomeController> logger)
    {
        _context = context;
        _logger = logger;
    }
    
    [HttpGet]
    public IActionResult Index()
    {
        return View(_context.Actors.Select(a => new ActorViewModel
        {
            Id = a.Id,
            Name = a.Name,
            Surname = a.Surname,
            DateOfBirth = a.DateOfBirth    
        }).ToList());
    }
    
    [HttpGet]
    public IActionResult Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var viewModel = _context.Actors.Where(a => a.Id == id).Select(a => new ActorViewModel
        {
            Id = a.Id,
            Name = a.Name,
            Surname = a.Surname,
            DateOfBirth = a.DateOfBirth
        }).FirstOrDefault();

            
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
            _context.Add(new Actor
            {
                Name = inputModel.Name,
                Surname = inputModel.Surname,
                DateOfBirth = inputModel.DateOfBirth
            });
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

        var editModel = _context.Actors.Where(a => a.Id == id).Select(a => new EditActorViewModel
        {
            Name = a.Name,
            Surname = a.Surname,
            DateOfBirth = a.DateOfBirth
        }).FirstOrDefault();
            
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
                var actor = new Actor
                {
                    Id = id,
                    Name = editModel.Name,
                    Surname = editModel.Surname,
                    DateOfBirth = editModel.DateOfBirth
                };
                    
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

        var deleteModel = _context.Actors.Where(a => a.Id == id).Select(a => new DeleteActorViewModel
        {
            Name = a.Name,
            Surname = a.Surname,
            DateOfBirth = a.DateOfBirth
        }).FirstOrDefault();
            
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