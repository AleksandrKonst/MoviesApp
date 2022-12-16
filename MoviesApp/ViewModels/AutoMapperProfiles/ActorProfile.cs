using MoviesApp.Models;
using AutoMapper;
using MoviesApp.Services.Dto;
using MoviesApp.ViewModels.Actors;

namespace MoviesApp.ViewModels.AutoMapperProfiles
{
    public class ActorProfile: Profile
    {
        public ActorProfile()
        {
            CreateMap<ActorDto, InputActorsModel>().ReverseMap();
            CreateMap<ActorDto, DeleteActorViewModel>();
            CreateMap<ActorDto, EditActorViewModel>().ReverseMap();
            CreateMap<ActorDto, ActorViewModel>();
        }
    }
}
