using System;

namespace Flush.Databases
{
    public interface ISession
    {
        int SessionId { get; }
        DateTime StartDateTime { get; }
        DateTime EndDateTime { get; }
    }
}