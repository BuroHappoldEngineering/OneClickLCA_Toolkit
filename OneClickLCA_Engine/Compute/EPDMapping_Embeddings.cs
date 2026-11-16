using BH.Adapter.OpenAI;
using BH.Engine.Adapters.OpenAI;
using BH.oM.Adapters.OpenAI;
using BH.oM.Adapters.OpenAI.Authorization;
using BH.oM.Adapters.OpenAI.Commands;
using BH.oM.Adapters.OpenAI.Enums;
using BH.oM.Adapters.OpenAI.Output;
using BH.oM.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace BH.Engine.Adapters.OneClickLCA.Compute
{
    internal class EPDMapping_Embeddings
    {
        public static Output<List<List<double>>, bool> Embeddings(List<string> embedding_texts)
        {
            OpenAIAdapter adapter = OpenAI_EmbeddingAdapter();

            GenerateEmbeddings command = new GenerateEmbeddings{Input = embedding_texts};
            EmbeddingExecutionConfig config = new EmbeddingExecutionConfig{};

            Output<List<List<double>>, bool> embeddings = adapter.ExecuteAsync(command, config).GetAwaiter().GetResult();
            return embeddings;
        }

        private static OpenAIAdapter OpenAI_EmbeddingAdapter()
        {
            if (m_Adapter == null)
                m_Adapter = new OpenAIAdapter(m_AdapterUrl, m_AdapterModel.DeploymentName(), new MicrosoftCredentials { AppRegistrationId = m_AppRegistrationId, TenantId = m_TenantId, RedirectUri = m_RedirectUri });

            return m_Adapter;
        }

        private static OpenAIAdapter m_Adapter = null;
        private static string m_AppRegistrationId = "0c05379c-63b0-4695-a4fd-00630f4e595e";
        private static string m_TenantId = "50ee6418-869e-48f5-a982-3607fcee1e1d";
        private static string m_RedirectUri = "http://localhost";
        private static string m_AdapterUrl = "https://bhom-openai-automation.openai.azure.com/openai/v1/";
        private static EmbeddingModelType m_AdapterModel = EmbeddingModelType.TextEmbedding3Small;
    }
}
