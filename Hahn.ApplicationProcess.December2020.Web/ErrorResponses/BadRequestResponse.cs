using System.Collections.Generic;

namespace Hahn.ApplicationProcess.December2020.Web.ErrorResponses
{
    public abstract class BadRequestResponse : BaseErrorResponse
    {
        public int Status { get; } = 400;

        public Dictionary<string, string[]> Errors { get; } = new();
    }
}