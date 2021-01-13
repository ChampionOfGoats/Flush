using System;

namespace Flush.Database
{
    public interface ISession
    {
        int SessionId { get; }
        DateTime StartDateTime { get; }
        DateTime? EndDateTime { get; }
    }
}