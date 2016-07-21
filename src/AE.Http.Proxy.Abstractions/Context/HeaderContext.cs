namespace AE.Http.Proxy.Abstractions.Context
{
    using System.Collections.Generic;
    using System.Linq;

    public class HeaderContext : IHeaderContext
    {
        private readonly List<string> _valueHistory;

        public HeaderContext(IHeaderContext header, HeaderOrigin origin)
            : this(header.Key, header.Value, origin)
        {
        }

        public HeaderContext(string key, string value, HeaderOrigin origin)
        {
            Key = key;
            Origin = origin;
            _valueHistory = new List<string> { value };
        }

        public HeaderOrigin Origin { get; private set; }

        public string Key { get; private set; }

        public string Value
        {
            get { return _valueHistory.Last(); }
        }

        public IEnumerable<string> ValueHistory
        {
            get { return _valueHistory.ToList(); }
        }

        public bool IsDeleted { get; private set; }

        public bool IsModified { get; private set; }

        public void ChangeValue(string value)
        {
            _valueHistory.Add(value);
            IsModified = true;
        }

        public void MarkAsDeleted()
        {
            IsDeleted = true;
            IsModified = true;
        }
    }
}