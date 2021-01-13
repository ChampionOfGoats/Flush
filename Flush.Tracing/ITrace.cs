using System;

namespace Flush.Tracing
{
    public struct Trace
    {
        TraceEventKind Event { get; }
        DateTime Timestamp { get; }
        string RaisedBy { get; }
        ITraceEventArgs Arguments { get; }
    }
}