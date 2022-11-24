using System;
using System.ComponentModel.DataAnnotations;

namespace MoviesApp.ViewModels.Actors;

public class InputActorsModel
{
    public string Name { get; set; }
        
    public string Surname { get; set; }

    [DataType(DataType.Date)]
    public DateTime DateOfBirth { get; set; }
}