using LearnStore.Application.DTO;
using LearnStore.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Application.Mappers
{
    public class AuthorMapper:IMapper<Author, AuthorDto>
    {
        public Author ToDomain(AuthorDto authorDto)
        {
            ArgumentNullException.ThrowIfNull(authorDto, nameof(authorDto));
            return new Author
            {
                Id = authorDto.Id,
                FirstName = authorDto.FirstName,
                LastName = authorDto.LastName,
                MiddleName = authorDto.MiddleName,
                Info = authorDto.Info
            };
        }

        public object ToDomain(object dto)
        {
           return ToDomain((AuthorDto)dto);
        }

        public AuthorDto ToDto(Author author)
        {
            ArgumentNullException.ThrowIfNull(author, nameof(author));
            return new AuthorDto
            {
                Id = author.Id,
                FirstName = author.FirstName,
                LastName = author.LastName,
                MiddleName = author.MiddleName,
                Info = author.Info
            };
        }

        public object ToDto(object domain)
        {
            return ToDto((Author)domain);
        }

        
    }

}
