using System;

namespace Flush.Database
{
    public interface ISession
    {
        DateTime StartDateTime { get; }
        DateTime? EndDateTime { get; }
        int Phase { get; }
    }
}
