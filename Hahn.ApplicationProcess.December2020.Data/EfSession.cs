using System.Threading.Tasks;

namespace Hahn.ApplicationProcess.December2020.Data
{
    public abstract class EfSession : EfReadOnlySession, ISession
    {
        protected EfSession(DatabaseContext context) : base(context) { }

        public Task SaveChangesAsync() => Context.SaveChangesAsync();
    }
}