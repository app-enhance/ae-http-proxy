namespace AE.Http.Proxy.Abstractions.Context
{
    using System.Collections.Generic;

    public interface IHeaderContext
    {
        HeaderOrigin Origin { get; }

        string Key { get; }

        string Value { get; }

        IEnumerable<string> ValueHistory { get; }

        bool IsDeleted { get; }

        bool IsModified { get; }
    }
}