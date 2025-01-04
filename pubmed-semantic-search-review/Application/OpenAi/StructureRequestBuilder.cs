using PubMedSemanticSearchReview.Application.OpenAi.StructuredRequests;
using AbstractAnalysisProperties = PubMedSemanticSearchReview.Application.OpenAi.AbstractAnalysis.Properties;
using StructuredMessage = PubMedSemanticSearchReview.Application.OpenAi.Requests.Message;

namespace PubMedSemanticSearchReview.Application.OpenAi
{
    internal class StructureRequestBuilder
    {
        public static string BuildAbstractAnalysisPrompt(string promptTemplate, string articleTitle, string articleText)
        {
            var abstractText = $"{articleTitle}\r\n{articleText}";

            if (!promptTemplate.Contains("{{abstract}}"))
            {
                throw new NotImplementedException("Bad prompt template");
            }

            return promptTemplate.Replace("{{abstract}}", abstractText);
        }

        /// <summary>
        /// Constructs a formatted prompt string summarizing an article's title, abstract, relevance reason, and additional prompt text.
        /// </summary>
        /// <param name="postPromptText">The concluding text appended after the article details, intended to guide further processing or prompt responses.</param>
        /// <param name="articleTitle">The title of the article to be included in the prompt.</param>
        /// <param name="abstractSummary">A brief summary of the article's abstract, providing an overview of its main points or findings.</param>
        /// <param name="relevanceReason">An explanation of why the article is relevant, helping to clarify its importance or connection to a specific topic.</param>
        /// <returns>A single, formatted string containing the article title, abstract summary, relevance reason, and additional text.</returns>
        /// <example>
        /// string prompt = BuildArticleInclusionPrompt(
        ///     "Based on this information, determine the article's inclusion relevance.",
        ///     "A systematic review of human evidence for intergenerational effects of radiation.",
        ///     "This review examines studies on the impact of radiation exposure across generations.",
        ///     "Relevant for understanding long-term effects of environmental factors."
        /// );
        /// </example>
        public static string BuildArticleInclusionPrompt(string postPromptText, string articleTitle, string abstractSummary, string relevanceReason)
        {
            return $"Article Title: {articleTitle}\r\nAbstract Summary: {abstractSummary}\\r\\nRelevance Reason: {relevanceReason}\\r\\n{postPromptText}";
        }

        public static StructuredRequestDto<AbstractAnalysisProperties> GetAbstractAnalysisRequest(string model, string systemPrompt, string userPrompt,
            int maxTokens, double temperature)
        {
            return new()
            {
                Model = model,
                MaxTokens = maxTokens,
                Temperature = temperature,
                Messages = [
                new StructuredMessage{ Role = "system", Content = systemPrompt},
                new StructuredMessage{ Role = "user", Content = userPrompt}
                ],
                ResponseFormat = new ResponseFormat<AbstractAnalysisProperties>
                {
                    Type = "json_schema",
                    JsonSchema = new JsonSchema<AbstractAnalysisProperties>
                    {
                        Name = "abstract_response",
                        Strict = true,
                        Schema = new Schema<AbstractAnalysisProperties>
                        {
                            Type = "object",
                            Properties = new AbstractAnalysisProperties
                            {
                                AbstractSummary = new CustomType { Type = "string" },
                                EstimatedPercentRelevant = new CustomType { Type = "number" },
                                IsRelevant = new CustomType { Type = "boolean" },
                                RelevanceReason = new CustomType { Type = "string" }
                            },
                            AdditionalProperties = false,
                            SchemaRequired = [ "is_relevant",
                                                                "estimated_percent_relevant",
                                                                "abstract_summary",
                                                                "relevance_reason" ]
                        }
                    }
                }
            };
        }
    }
}
