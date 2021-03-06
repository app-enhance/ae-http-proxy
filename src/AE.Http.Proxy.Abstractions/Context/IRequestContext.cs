﻿namespace AE.Http.Proxy.Abstractions.Context
{
    using System.Collections.Generic;

    public interface IRequestContext
    {
        RequestMethod Method { get; }

        string Path { get; }

        IEnumerable<IHeaderContext> Headers { get; }

        string Content { get; }

        string ContentType { get; }

        ContentDescriptor ContentDescriptor { get; }
    }
}