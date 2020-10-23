using System;
using System.Collections;
using System.Collections.Generic;

namespace Rainbow.Services.Registery
{
    public interface IServiceRegisteryBuilder
    {
        IList<IServiceRegisterySource> Sources { get; }

        IServiceApplication Application { get; }
        IServiceRegistery Build();
    }
}
