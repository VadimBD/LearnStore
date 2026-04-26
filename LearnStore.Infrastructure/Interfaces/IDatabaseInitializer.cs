using System;
using System.Collections.Generic;
using System.Text;

namespace LearnStore.Infrastructure.Interfaces
{
    public interface IDatabaseInitializer<TContext> where TContext : DbContext
    {
            void EnsureDatabaseAndUser();
    }
}
