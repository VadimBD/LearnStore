using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Application.Mappers
{
    public class MapperRegistry (IServiceProvider ServiceProvider): IMapperRegistry
    {
        public IMapper<TSource, TDestination> Get<TSource, TDestination>() 
        {
            return ServiceProvider.GetRequiredService<IMapper<TSource, TDestination>>();
        }
    }
}
