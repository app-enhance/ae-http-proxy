namespace AE.Http.Proxy.Abstractions.Context
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public abstract class MessageContext
    {
        private readonly Dictionary<string, HeaderContext> _headers;

        private readonly List<byte[]> _rawContentHistory;

        protected MessageContext(IDictionary<string, string> headers, byte[] rawContent)
        {
            if (headers == null)
            {
                throw new ArgumentNullException("headers");
            }

            _headers = headers.ToDictionary(x => x.Key, x => new HeaderContext(x.Key, x.Value, InitialHeadersOrigin));
            _rawContentHistory = new List<byte[]> { rawContent ?? new byte[0] };
        }

        public IEnumerable<IHeaderContext> Headers
        {
            get { return _headers.Select(x => x.Value).ToList(); }
        }

        public string Content
        {
            get
            {
                var rawContent = _rawContentHistory.Last();
                return Encoding.UTF8.GetString(rawContent, 0, rawContent.Length);
            }
        }

        public byte[] RawContent
        {
            get { return _rawContentHistory.Last(); }
        }

        public long ContentLength
        {
            get { return RawContent.Length; }
        }

        public IEnumerable<byte[]> RawContentHistory
        {
            get { return _rawContentHistory.ToList(); }
        }

        public ContentDescriptor ContentDescriptor
        {
            get { return RetrieveContentDescriptor(); }
        }

        public string ContentType
        {
            get { return GetContentType(); }
        }

        protected abstract HeaderOrigin InitialHeadersOrigin { get; }

        public void SetContent(string content, string contentType = null)
        {
            var rawBytes = Encoding.UTF8.GetBytes(content);
            SetContent(rawBytes, string.IsNullOrWhiteSpace(contentType) ? ContentType : contentType);
        }

        public void SetContent(byte[] content, string contentType)
        {
            if (string.IsNullOrWhiteSpace(contentType))
            {
                throw new ArgumentNullException("contentType");
            }

            SetHeader("Content-Type", contentType);
            SetHeader("Content-Length", content.Length.ToString());
            _rawContentHistory.Add(content);
        }

        public void SetHeader(string key, string value)
        {
            var header = FindHeader(key);
            if (header != null)
            {
                header.ChangeValue(value);
            }
            else
            {
                _headers.Add(key, new HeaderContext(key, value, HeaderOrigin.New));
            }
        }

        public void DeleteHeader(string key)
        {
            var header = FindHeader(key);
            if (header == null)
            {
                throw new KeyNotFoundException(string.Format("There is no header with key: {0}", key));
            }

            header.MarkAsDeleted();
        }

        private HeaderContext FindHeader(string key)
        {
            HeaderContext header;
            return _headers.TryGetValue(key, out header) ? header : null;
        }

        private string GetContentType()
        {
            var contentTypeHeader = FindHeader("Content-Type");
            return contentTypeHeader == null ? null : contentTypeHeader.Value;
        }

        private ContentDescriptor RetrieveContentDescriptor()
        {
            var contentType = ContentType;
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