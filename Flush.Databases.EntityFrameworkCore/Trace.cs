using System;
using Flush.Core;
using Flush.Core.Contracts;

namespace Flush.Databases.EntityFrameworkCore
{
    public class Trace : ITrace
    {
        public int TraceId { get; set; }
        public TraceEventKind Event { get; set; }
        public DateTime Timestamp { get; set; }
        public int ApplicationUserId { get; set; }
        public ITraceEventArgs Details { get; set; }

        public int SessionId { get; set; }
        public Session Session { get; set; }
    }
}
