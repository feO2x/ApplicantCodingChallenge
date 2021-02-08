using System.Threading;
using System.Threading.Tasks;

namespace Hahn.ApplicationProcess.December2020.Domain
{
    public interface ICountryNameValidator
    {
        Task<bool> CheckIfCountryNameIsValidAsync(string countryName, CancellationToken cancellationToken);
    }
}