using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using AutoMapper;
using MoviesApp.Data;
using MoviesApp.Models;
using MoviesApp.ViewModels.Movies;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace MoviesApp.Controllers
{
    public class MoviesController : Controller
    {
        private readonly MoviesContext _context;
        private readonly ILogger<HomeController> _logger;
        private readonly IMapper _mapper;

        public MoviesController(MoviesContext context, ILogger<HomeController> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize]
        public IActionResult Index()
        {
            var movies = _mapper.Map<IEnumerable<Movie>, IEnumerable<MovieViewModel>>(_context.Movies.ToList());
            return View(movies);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var viewModel = _mapper.Map<MovieViewModel>(_context.Movies.FirstOrDefault(m => m.Id == id));

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
        [Authorize(Roles = "Admin")]
        public IActionResult Create([Bind("Title,ReleaseDate,Genre,Price")] InputMovieViewModel inputModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(_mapper.Map<Movie>(inputModel));
                _context.SaveChanges();

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

            var editModel = _mapper.Map<EditMovieViewModel>(_context.Movies.FirstOrDefault(m => m.Id == id));

            if (editModel == null)
            {
                return NotFound();
            }

            return View(editModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id, [Bind("Title,ReleaseDate,Genre,Price")] EditMovieViewModel editModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var movie = _mapper.Map<Movie>(editModel);
                    movie.Id = id;
                    _context.Update(movie);
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
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var deleteModel = _mapper.Map<DeleteMovieViewModel>(_context.Movies.FirstOrDefault(m => m.Id == id));

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
            var movie = _context.Movies.Find(id);
            _context.Movies.Remove(movie);
            _context.SaveChanges();
            _logger.LogError($"Movie with id {movie.Id} has been deleted!");
            return RedirectToAction(nameof(Index));
        }

        private bool MovieExists(int id)
        {
            return _context.Movies.Any(e => e.Id == id);
        }
    }
}