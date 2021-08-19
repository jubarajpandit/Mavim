using Mavim.Manager.Connect.Write.Database;
using Mavim.Manager.Connect.Write.EventSourcing.Interfaces;

namespace Mavim.Manager.Connect.Write.EventSourcing
{
    public class CommonEventSourcing : EventSourcingBase, ICommonEventSourcing
    {
        public CommonEventSourcing(ConnectDbContext dbContext) : base(dbContext)
        { }
    }
}
