namespace AE.Http.Proxy.Abstractions.Context
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public abstract class MessageContext
    {
        private readonly Dictionary<string, HeaderContext> headers;

        private readonly List<byte[]> rawContentHistory;

        protected MessageContext(IDictionary<string, string> headers, byte[] rawContent)
        {
            if (headers == null)
            {
                throw new ArgumentNullException("headers");
            }

            this.headers = headers.ToDictionary(x => x.Key, x => new HeaderContext(x.Key, x.Value, this.InitialHeadersOrigin));
            this.rawContentHistory = new List<byte[]> { rawContent ?? new byte[0] };
        }

        public IEnumerable<IHeaderContext> Headers
        {
            get { return this.headers.Select(x => x.Value).ToList(); }
        }

        public string Content
        {
            get
            {
                var rawContent = this.rawContentHistory.Last();
                return Encoding.UTF8.GetString(rawContent, 0, rawContent.Length);
            }
        }

        public byte[] RawContent
        {
            get { return this.rawContentHistory.Last(); }
        }

        public long ContentLength
        {
            get { return this.RawContent.Length; }
        }

        public IEnumerable<byte[]> RawContentHistory
        {
            get { return this.rawContentHistory.ToList(); }
        }

        public ContentDescriptor ContentDescriptor
        {
            get { return this.RetrieveContentDescriptor(); }
        }

        public string ContentType
        {
            get { return this.GetContentType(); }
        }

        protected abstract HeaderOrigin InitialHeadersOrigin { get; }

        public void SetContent(string content, string contentType = null)
        {
            var rawBytes = Encoding.UTF8.GetBytes(content);
            this.SetContent(rawBytes, string.IsNullOrWhiteSpace(contentType) ? this.ContentType : contentType);
        }

        public void SetContent(byte[] content, string contentType)
        {
            if (string.IsNullOrWhiteSpace(contentType))
            {
                throw new ArgumentNullException("contentType");
            }

            this.SetHeader("Content-Type", contentType);
            this.SetHeader("Content-Length", content.Length.ToString());
            this.rawContentHistory.Add(content);
        }

        public void SetHeader(string key, string value)
        {
            var header = this.FindHeader(key);
            if (header != null)
            {
                header.ChangeValue(value);
            }
            else
            {
                this.headers.Add(key, new HeaderContext(key, value, HeaderOrigin.New));
            }
        }

        public void DeleteHeader(string key)
        {
            var header = this.FindHeader(key);
            if (header == null)
            {
                throw new KeyNotFoundException(string.Format("There is no header with key: {0}", key));
            }

            header.MarkAsDeleted();
        }

        private HeaderContext FindHeader(string key)
        {
            HeaderContext header;
            return this.headers.TryGetValue(key, out header) ? header : null;
        }

        private string GetContentType()
        {
            var contentTypeHeader = this.FindHeader("Content-Type");
            return contentTypeHeader == null ? null : contentTypeHeader.Value;
        }

        private ContentDescriptor RetrieveContentDescriptor()
        {
            var contentType = this.ContentType;
            if (contentType == null)
            {
                return ContentDescriptor.Unkown;
            }

            if (contentType.EndsWith("css") || contentType.EndsWith("javascript"))
            {
                return ContentDescriptor.Resource;
            }

            if (contentType.StartsWith("text"))
            {
                return ContentDescriptor.PlainText;
            }

            if (contentType.StartsWith("image"))
            {
                return ContentDescriptor.Image;
            }

            return ContentDescriptor.Other;
        }
    }
}