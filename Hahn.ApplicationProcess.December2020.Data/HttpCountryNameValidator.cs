using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Hahn.ApplicationProcess.December2020.Domain;

namespace Hahn.ApplicationProcess.December2020.Data
{
    public sealed class HttpCountryNameValidator : ICountryNameValidator
    {
        public async Task<bool> CheckIfCountryNameIsValidAsync(string countryName, CancellationToken cancellationToken = default)
        {
            using var httpClient = new HttpClient();
            var encodedCountryName = Uri.EscapeDataString(countryName);
            var url = $"https://restcountries.eu/rest/v2/name/{encodedCountryName}?fullText=true";
            var response = await httpClient.GetAsync(url, cancellationToken);
            return response.StatusCode == HttpStatusCode.OK;
        }
    }
}