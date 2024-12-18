﻿using AutoMapper;
using Library.Application.DTOs;
using Library.Domain.Entities;
using Microsoft.AspNetCore.Http;


namespace Library.Application.Utilities
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<Author, AuthorDTO>().ReverseMap();
                config.CreateMap<Book, BookDTO>().ReverseMap();
            });
            return mappingConfig;
        }

    }
}
