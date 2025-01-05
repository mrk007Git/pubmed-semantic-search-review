[![.NET](https://github.com/mrk007Git/pubmed-semantic-search-review/actions/workflows/dotnet.yml/badge.svg)](https://github.com/mrk007Git/pubmed-semantic-search-review/actions/workflows/dotnet.yml)

# PubMed Semantic Search Review

**PubMed Semantic Search Review** is a tool designed to systematically query the PubMed API using single search terms and optional date ranges. By design, it only accepts single search terms for focused retrieval. Boolean operations (`AND`, `OR`) can be applied during the analysis phase, enabling more complex evaluations of relevance after data retrieval.

The tool processes each article's title and abstract text by combining them into a prompt for OpenAI's GPT models, generating structured outputs defined by the following schema:

```csharp
public class StructuredResponseDto : StructuredResponseBaseDto
{
    [JsonPropertyName("is_relevant")]
    public bool IsRelevant { get; set; }
    [JsonPropertyName("estimated_percent_relevant")]
    public decimal? EstimatedPercentRelevant { get; set; }
    [JsonPropertyName("abstract_summary")]
    public string? AbstractSummary { get; set; }
    [JsonPropertyName("relevance_reason")]
    public string? RelevanceReason { get; set; }
}
```

---

## Features

- Queries PubMed using single search terms and optional date ranges.
- Processes article titles and abstracts with OpenAI GPT models for structured relevance analysis.
- Outputs results in a structured CSV format for easy review.
- Supports bulk processing with rate limits to prevent API throttling.
- Provides detailed logs for better traceability and debugging.

---

## Setup Instructions

### Prerequisites

1. **.NET 8**: Ensure you have the .NET SDK installed on your machine.
   - Download from [Microsoft's official site](https://dotnet.microsoft.com/download).

2. **API Keys**: 
   - **PubMed API Key**: Obtain a key by registering for NCBI APIs at [NCBI Developers](https://www.ncbi.nlm.nih.gov/account/).
   - **OpenAI API Key**: Obtain a key by signing up at [OpenAI's platform](https://platform.openai.com/).

---

### Configuration

1. **Clone the Repository**
   ```bash
   git clone https://github.com/your-repo/pubmed-semantic-search-review.git
   cd pubmed-semantic-search-review
   ```

2. **Add API Keys to `appsettings.json`**
   - Navigate to `appsettings.json` in the root directory and update the following placeholders:
     ```json
     "PubMedConfig": {
       "ApiKey": "your-pubmed-api-key"
     },
     "OpenAiConfig": {
       "ApiKey": "your-openai-api-key"
     }
     ```

3. **Set Up User Secrets (Optional)**
   - For added security, store API keys in `secrets.json`:
     ```bash
     dotnet user-secrets init
     dotnet user-secrets set "PubMedConfig:ApiKey" "your-pubmed-api-key"
     dotnet user-secrets set "OpenAiConfig:ApiKey" "your-openai-api-key"
     ```

4. **Install Dependencies**
   ```bash
   dotnet restore
   ```

5. **Run the Application**
   ```bash
   dotnet run
   ```

---

### Key Considerations

1. **Temperature Setting**:
   - The `Temperature` parameter in the `OpenAiConfig` section directly influences the creativity or randomness of the model's output. It is a critical tuning parameter to balance between deterministic and exploratory responses.

2. **System and User Prompts**:
   - The **system prompt** (`SystemPrompt_AbstractAnalysis.txt`) sets the tone and context for the model's behavior (e.g., "You are a helpful and professional medical researcher").
   - The **user prompt** (`UserPrompt_AbstractAnalysis.txt`) incorporates the article's title and abstract, alongside your specific research question and hypothesis, to guide the model's structured analysis. Text in this file must contain a placeholder for the abstract which is currently _{{abstract}}_.

3. **OpenAI Costs**:
   - This project incurs costs based on token usage.
   - Current pricing (January 1st, 2025):
     - **Prompt Tokens**: $2.50 per million tokens.
     - **Completion Tokens**: $10.00 per million tokens.
   - Monitor your API usage and budget carefully.

4. **Rate Limiting**:
   - The PubMed API limits requests to 1 per second by default.
   - The tool implements a delay to adhere to this limit.

5. **Logging**:
   - Logs are saved to the `logs` directory, segmented by hour.
   - Review logs for insights or troubleshooting.

---

## Future Plans

- Integration with free Large Language Models (LLMs) to reduce costs.
- Enhanced analysis and report generation capabilities.
- UI-based configuration for non-technical users.

---

## License

This project is licensed under the [MIT License](LICENSE).

---

## Support

For issues or feature requests, please open a ticket in the [GitHub Issues](https://github.com/your-repo/pubmed-semantic-search-review/issues) section.

