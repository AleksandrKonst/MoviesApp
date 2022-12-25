using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MoviesApp.Data;
using MoviesApp.Filters;
using MoviesApp.Models;
using MoviesApp.Services;
using MoviesApp.Services.Dto;
using MoviesApp.ViewModels.Actors;

namespace MoviesApp.Controllers;

public class ActorsController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IMapper _mapper;
    private readonly IActorService _service;

    public ActorsController(ILogger<HomeController> logger, IMapper mapper, IActorService service)
    {
        _logger = logger;
        _mapper = mapper;
        _service = service;
    }

    [HttpGet]
    [Authorize]
    public IActionResult Index()
    {
        var actors = _mapper.Map<IEnumerable<ActorDto>, IEnumerable<ActorViewModel>>(_service.GetAllActors());
        return View(actors);
    }

    [HttpGet]
    [Authorize]
    public IActionResult Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var viewModel = _mapper.Map<ActorViewModel>(_service.GetActor((int) id));

        if (viewModel == null)
        {
            return NotFound();
        }

        return View(viewModel);
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [ActorDate]
    [Authorize(Roles = "Admin")]
    public IActionResult Create([Bind("Name,Surname,DateOfBirth")] InputActorsModel inputModel)
    {
        if (ModelState.IsValid)
        {
            _service.AddActor(_mapper.Map<ActorDto>(inputModel));
            return RedirectToAction(nameof(Index));
        }
        return View(inputModel);
    }
        
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public IActionResult Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var editModel = _mapper.Map<EditActorViewModel>(_service.GetActor((int) id));

        if (editModel == null)
        {
            return NotFound();
        }
            
        return View(editModel);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    [ActorDate]
    [Authorize(Roles = "Admin")]
    public IActionResult Edit(int id, [Bind("Name,Surname,DateOfBirth")] EditActorViewModel editModel)
    {
        if (ModelState.IsValid)
        {
            var actor = _mapper.Map<ActorDto>(editModel);
            actor.Id = id;
                
            var result = _service.UpdateActor(actor);

            if (result == null)
            {
                return NotFound();
            }
                
            return RedirectToAction(nameof(Index));
        }
        return View(editModel);
    }
        
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public IActionResult Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var deleteModel = _mapper.Map<DeleteActorViewModel>(_service.GetActor((int) id));

        if (deleteModel == null)
        {
            return NotFound();
        }

        return View(deleteModel);
    }
        
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public IActionResult DeleteConfirmed(int id)
    {
        var actor = _service.DeleteActor(id);
        if (actor==null)
        {
            return NotFound();
        }
        _logger.LogError($"Actor with id {actor.Id} has been deleted!");
        return RedirectToAction(nameof(Index));
    }
}