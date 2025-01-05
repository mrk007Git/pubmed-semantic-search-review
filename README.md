# PubMed Semantic Search Review

**PubMed Semantic Search Review** is a tool designed to enhance systematic literature reviews by leveraging semantic search capabilities through OpenAI's GPT models. This project is focused on improving the relevance and comprehensiveness of PubMed article searches, with initial support for OpenAI's GPT-based models and PubMed's eUtils API.

## Features

- Retrieves PubMed articles using specific search terms and filters.
- Analyzes articles for relevance using OpenAI's GPT models.
- Outputs results in a structured CSV format for easy review.
- Supports bulk processing with a rate limit to prevent API throttling.
- Logs system operations for better traceability and debugging.

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

1. **OpenAI Costs**: 
   - This project incurs costs based on token usage.
   - Current pricing:
     - **Prompt Tokens**: $2.50 per million tokens.
     - **Completion Tokens**: $10.00 per million tokens.
   - Monitor your API usage and budget carefully.

2. **Rate Limiting**:
   - The PubMed API limits requests to 1 per second by default.
   - The tool implements a delay to adhere to this limit.

3. **Logging**:
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
