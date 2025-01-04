using Microsoft.Extensions.Options;

namespace PubMedSemanticSearchReview.Infrastructure.Configuration;

internal class OpenAiConfigValidation : IValidateOptions<OpenAiConfig>
{
    public ValidateOptionsResult Validate(string? name, OpenAiConfig options)
    {
        ArgumentNullException.ThrowIfNull(options);

        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(options.BaseAddress))
        {
            errors.Add($"{nameof(options.BaseAddress)} is required.");
        }

        if (string.IsNullOrWhiteSpace(options.Model))
        {
            errors.Add($"{nameof(options.Model)} is required.");
        }

        if (string.IsNullOrWhiteSpace(options.ApiKey))
        {
            errors.Add($"{nameof(options.ApiKey)} is required.");
        }

        if (string.IsNullOrWhiteSpace(options.SystemRole))
        {
            errors.Add($"{nameof(options.SystemRole)} is required.");
        }

        if (options.Temperature < 0 || options.Temperature > 11)
        {
            errors.Add($"{nameof(options.Temperature)} must be between 0 and 1.");
        }

        if (options.MaxTokens <= 0)
        {
            errors.Add($"{nameof(options.MaxTokens)} must be greater than 0.");
        }

        if (options.Endpoints == null)
        {
            errors.Add($"{nameof(options.Endpoints)} is required.");
        }
        else
        {
            if (string.IsNullOrWhiteSpace(options.Endpoints.ChatCompletions))
            {
                errors.Add($"{nameof(options.Endpoints.ChatCompletions)} is required.");
            }
        }

        if (errors.Count != 0)
        {
            return ValidateOptionsResult.Fail(errors);
        }

        return ValidateOptionsResult.Success;
    }
}
