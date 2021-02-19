namespace Hahn.ApplicationProcess.December2020.Web.ErrorResponses
{
    public abstract class BaseErrorResponse
    {
        public string Type { get; } = string.Empty;

        public string Title { get; } = string.Empty;

        public string TraceId { get; } = string.Empty;
    }
}