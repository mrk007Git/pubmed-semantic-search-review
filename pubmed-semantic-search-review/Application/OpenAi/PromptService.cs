namespace PubMedSemanticSearchReview.Application.OpenAi;

internal class PromptService
{
    private const string SystemPromptFilePath = "Data/SystemPrompt_AbstractAnalysis.txt";
    private const string UserPromptFilePath = "Data/UserPrompt_AbstractAnalysis.txt";

    public static PromptDto GetPromptForAbstractAnalysis(string articleTitle, string abstractText)
    {
        var systemPrompt = SystemPromptForAbstractAnalysis();
        var userPrompt = UserPromptForAbstractAnalysis(articleTitle, abstractText);
        return new PromptDto(systemPrompt, userPrompt);
    }

    private static string SystemPromptForAbstractAnalysis()
    {
        if (!File.Exists(SystemPromptFilePath))
        {
            throw new FileNotFoundException($"{SystemPromptFilePath} not found");
        }

        return File.ReadAllText(SystemPromptFilePath);
    }

    private static string UserPromptForAbstractAnalysis(string articleTitle, string abstractText)
    {
        if (!File.Exists(UserPromptFilePath))
        {
            throw new FileNotFoundException($"{UserPromptFilePath} not found");
        }

        var prompt = File.ReadAllText(UserPromptFilePath);

        if (!prompt.Contains("{{abstract}}"))
        {
            throw new InvalidOperationException($"{UserPromptFilePath} does not contain {{abstract}}");
        }

        return prompt.Replace("{{abstract}}", $"Article Title: {articleTitle}\r\n\r\nAbstract:\r\n{abstractText}");
    }
}
