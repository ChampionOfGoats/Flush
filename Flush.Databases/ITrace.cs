using System;
using Flush.Core;
using Flush.Core.Contracts;

namespace Flush.Databases
{
    public interface ITrace
    {
        int TraceId { get; }
        TraceEventKind Event { get; }
        DateTime Timestamp { get; }
        int ApplicationUserId { get; }
        ITraceEventArgs Details { get; }
    }
}