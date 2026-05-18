using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Application.Interfaces
{
    public interface IMapperRegistry
    {
        IMapper<TSource, TDestination> Get<TSource, TDestination>();
    }
}
