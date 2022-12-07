using System;
using System.ComponentModel.DataAnnotations;
using MoviesApp.Filters;

namespace MoviesApp.ViewModels.Actors;

public class InputActorsModel

{
    [Required]
    [ActorLenght(4)]
    public string Name { get; set; }
    
    [Required]
    [ActorLenght(4)]
    public string Surname { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime DateOfBirth { get; set; }
}