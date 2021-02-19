namespace Hahn.ApplicationProcess.December2020.Web.ErrorResponses
{
    public abstract class NotFoundResponse : BaseErrorResponse
    {
        public int Status { get; } = 404;
    }
}