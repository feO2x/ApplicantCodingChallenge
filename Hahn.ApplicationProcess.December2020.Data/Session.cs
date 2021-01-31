using System.Threading.Tasks;

namespace Hahn.ApplicationProcess.December2020.Data
{
    public abstract class Session : ReadOnlySession, ISession
    {
        protected Session(DatabaseContext context) : base(context) { }

        public Task SaveChangesAsync() => Context.SaveChangesAsync();
    }
}