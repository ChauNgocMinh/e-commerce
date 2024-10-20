﻿using AutoMapper;
using E_Commerce.Application.Response.MovieResponse;
using E_Commerce.Domain.Entities.Movies;
namespace E_Commerce.Application.AutoMapping.ToMovieMapping
{
    public class ToMovieMapping : Profile
    {
        public ToMovieMapping() 
        {
            CreateMap<Movie, MovieResponse>();
            CreateMap<MovieImage, MovieImageResponse>();
            CreateMap<MovieTag, MovieTagResponse>();
        }
    }
}
