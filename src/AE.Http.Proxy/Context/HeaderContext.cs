namespace AE.Http.Proxy.Context
{
    using System.Collections.Generic;
    using System.Linq;

    public class HeaderContext : IHeaderContext
    {
        private readonly List<string> valueHistory;

        public HeaderContext(IHeaderContext header, HeaderOrigin origin)
            : this(header.Key, header.Value, origin)
        {
        }

        public HeaderContext(string key, string value, HeaderOrigin origin)
        {
            this.Key = key;
            this.Origin = origin;
            this.valueHistory = new List<string> { value };
        }

        public HeaderOrigin Origin { get; private set; }

        public string Key { get; private set; }

        public string Value
        {
            get { return this.valueHistory.Last(); }
        }

        public IEnumerable<string> ValueHistory
        {
            get { return this.valueHistory.ToList(); }
        }

        public bool IsDeleted { get; private set; }
        
        public bool IsModified { get; private set; }

        public void ChangeValue(string value)
        {
            this.valueHistory.Add(value);
            this.IsModified = true;
        }

        public void MarkAsDeleted()
        {
            this.IsDeleted = true;
            this.IsModified = true;
        }
    }
}