using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Application.Interfaces
{
    public interface IMapper
    {
        object ToDto(object domain);
        object ToDomain(object dto);
    }
    public interface IMapper<TDomain, TDto> : IMapper
    {
        TDto ToDto(TDomain domain);
        TDomain ToDomain(TDto dto);
    }
}
