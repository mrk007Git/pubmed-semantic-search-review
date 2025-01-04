using Microsoft.Extensions.Options;

namespace PubMedSemanticSearchReview.Infrastructure.Configuration;

internal class PubMedConfigValidation : IValidateOptions<PubMedConfig>
{
    public ValidateOptionsResult Validate(string? name, PubMedConfig options)
    {
        ArgumentNullException.ThrowIfNull(options);

        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(options.BaseAddress))
        {
            errors.Add($"{nameof(options.BaseAddress)} is required.");
        }

        if (string.IsNullOrWhiteSpace(options.ApiKey))
        {
            errors.Add($"{nameof(options.ApiKey)} is required.");
        }

        if (options.RateLimitPerSecond < 1)
        {
            errors.Add($"{nameof(options.RateLimitPerSecond)} must be greater than 0.");
        }

        if (options.Endpoints == null)
        {
            errors.Add($"{nameof(options.Endpoints)} is required.");
        }
        else
        {
            if (string.IsNullOrWhiteSpace(options.Endpoints.Fetch))
            {
                errors.Add($"{nameof(options.Endpoints.Fetch)} is required.");
            }

            if (string.IsNullOrWhiteSpace(options.Endpoints.Summary))
            {
                errors.Add($"{nameof(options.Endpoints.Summary)} is required.");
            }

            if (string.IsNullOrWhiteSpace(options.Endpoints.Search))
            {
                errors.Add($"{nameof(options.Endpoints.Search)} is required.");
            }
        }

        if (errors.Count != 0)
        {
            return ValidateOptionsResult.Fail(errors);
        }

        return ValidateOptionsResult.Success;
    }
}
