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
using System.ComponentModel;
using System.Linq;
using System.Text.Json;

public static partial class Compute
{
    internal class epdmapping_chatcompletion
    {
        public static string EPDMapping(Dictionary<string, string> question, Dictionary<string, string> epd_lib_dict, Dictionary<string, string> epdlibrary_examples, Dictionary<string, string> remaining_examples, Dictionary<string, string> df_dict)
        {
            OpenAIAdapter adapter = OpenAI_ChatAdapter();
            PromptExecutionConfig config = new PromptExecutionConfig 
            {
                Temperature = 0,
                OutputType = new JsonSchema
                {
                    Name = m_mappingResponseSchemaName,
                    Schema = m_mappingResponseJsonSchema
                }
            };

            string userPrompt = $" Select categories for the material described in the following dictionary: {question}." + 
                $"\nUse the Costplan entry, Headline 1, Headline 2, Headline 3, Headline 4, and Headline 5 to select one of the fixed categories under each of the following dictionary keys: resourceType, resourceSubType, resourceTypeL3, technicalSpec and EPD name." +
                $"\nThe valid category structure is provided: {epd_lib_dict}. Note that the JSON no longer contains the original label names, but its hierarchy follows:" +
                $"\rLevel 0 = resourceType" + 
                $"\rLevel 1 = resourceSubType" +
                $"\rLevel 2 = resourceTypeL3" +
                $"\rLevel 3 = technicalSpec" +
                $"\rLevel 4 = EPD name" +
                $"\rUse this category structure to identify the correct category at each depth." +
                $"\rAll categories will always have information and the valid category should always be mapped even if the category is null." +
                $"\rIt is explicitly valid and expected for the same string to appear at multiple hierarchy levels (e.g. resourceType=\"insulation\" AND resourceSubType=\"insulation\"). When the hierarchy defines the same value at Level N and Level N+1, you MUST output that identical string for the fields with the same value. Do NOT substitute a different value or remove one of the values simply because it would \"look\" like a duplicate — the hierarchy is the source of truth and all the values should be filled in." + 
                $"\rBe aware that the headlines are not always present, and may be empty." + 
                $"\rUse the following examples to support the selection: {epdlibrary_examples["examples"]}. Each example is an array following the schema order as stated here: {epdlibrary_examples["schema"]}." + 
                $"\rWhere valid categories can't be reasonably selected from the valid category structure, select resourceType, resourceSubType, resourceTypeL3, technicalSpec and EPD name using the following examples instead: {remaining_examples["examples"]}. Each example is an array following the schema order as stated here: {remaining_examples["schema"]}." + 
                $"\rBe aware that these examples will not always follow the valid category structure and if that is the case the category strucutre as provided in the example should be followed. All categories in the examples will always have information and the valid category should always be mapped even if the category is null or left empty.";
            
            string systemPrompt = $"Role: You are a UK‑based building‑engineering carbon‑calculation assistant. You are aware of the terms in the follwing dictionary: {df_dict}" + 
                $"\rPrimary Task: Given a single construction costplan entry and headlines, select the required categories and return JSON only — no explanations or extra text. Your task is NOT to classify or infer new labels. Your task is to SELECT an EXISTING VALID PATH from the provided category structure. Each of the following must be selected exactly from the JSON hierarchy provided:" + 
                $"\rresourceType must be selected from Level 0" +
                $"\rresourceSubType must be a child of the selected resourceType at Level 1" +
                $"\rresourceTypeL3 must be a child of the selected resourceSubType at Level 2" +
                $"\rtechnicalSpec must be a child of the selected resourceTypeL3 at Level 3 OR be an empty string if missing" +
                $"\rEPD name must be selected from Level 4 under the chosen technicalSpec" +
                $"\rYou are performing hierarchical path retrieval, not semantic classification." +
                $"\rIf no exact valid path exists in the provided hierarchy: select the closest EXISTING FULL PATH from the hierarchy. DO NOT create, modify, infer, or generalise any label" +
                //$"\rOutput Format: Return exactly one JSON object with the following keys, in this order, and no additional keys:\r\n    \"id\" — string\r\n    \"Costplan entry\" — string\r\n    \"Headline 1\" — string (different level descriptions of a building element, e.g. Substructure, Upper floors, Timber floor structure, etc.)\r\n    \"Headline 2\" — string (different level descriptions of a building element, e.g. Substructure, Upper floors, Timber floor structure, etc.)\r\n    \"Headline 3\" — string (different level descriptions of a building element, e.g. Substructure, Upper floors, Timber floor structure, etc.)\r\n    \"Headline 4\" — string (different level descriptions of a building element, e.g. Substructure, Upper floors, Timber floor structure, etc.)\r\n    \"Headline 5\" — string (different level descriptions of a building element, e.g. Substructure, Upper floors, Timber floor structure, etc.)\r\n    \"resourceType\" — string (e.g., metal, timber, concrete, masonry, insulation, membrane, plasticMembraneRoofing)\r\n    \"resourceSubType\" — string (e.g., structuralSteel, softwood, rebar, aluminiumExtrusions)\r\n    \"resourceTypeL3\" — string (e.g., structuralSteelPlateSheetCoilsDecking)\r\n    \"technicalSpec\" — string (e.g., structuralSteelPlateSheetCoilsDecking)\r\n    \"EPD name\" — string (will vary in detail level depending on the material, e.g. 'Single skin wall from bricks, including mortar, 102.5 mm, 1:5 cement:sand mix, uses CEM I cement, 194.11 kg/m2' and 'Ceramic tile',)\r\n    \r\n
                $"Strict Rules" +
                $"JSON only: Output must be a valid JSON object, not a string or code block. Do not include any extra spaces, indentation, or line breaks in the output. Do not wrap the JSON in quotes or markdown/code fences. Examples may be formatted for readability; output must be a single-line JSON object." +
                $"\rSchema completeness: Always select a category for all of the levels even if the value is the same for different levels." +
                $"\rNormalisation: Don't change the formatting of any of the outputs." +
                $"\rHierarchy Enforcement: All selected values must form a valid parent-child path in the provided category structure. A value at Level N is ONLY valid if it exists as a child of the selected Level N-1 value in the hierarchy." +
                $"\rClosed Set Constraint: All output values must be copied EXACTLY from the category structure. String similarity, paraphrasing, or semantic equivalents are NOT permitted. If the exact label is not present in the structure it MUST NOT be used.";

            //BH.Engine.Base.Compute.StartSuppressRecordingEvents(true, true); // TO SUPRESS WARNINGS AND ERRORS

            //Dictionary<string, string>  mappingResult = new Dictionary<string, string>();
            //Output<string, bool> response

            //try
            //{
            GenerateChatCompletion prompt = new GenerateChatCompletion { System = systemPrompt, User = new List<string> { userPrompt } };
            Output<string, bool> response = adapter.ExecuteAsync(prompt, config).GetAwaiter().GetResult();

            //mappingResult = JsonSerializer.Deserialize<Dictionary<string, string>>(response.Item1); - NO NEED TO DESIRIALISE FROM JSON AS THIS WILL BE MOVED TO THE PYTHON ENVIRONMENT
            //}
            //catch (Exception ex)
            //{
            //    Base.Compute.RecordWarning($"EPD mapping failed to parse the chat completion: {ex.Message}");
            //}

            return response.Item1;
        }

        private static OpenAIAdapter OpenAI_ChatAdapter()
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
        private static ModelType m_AdapterModel = ModelType.GPT_6_Luna;

        private static string m_mappingResponseSchemaName = "semantic_identifier_matches";
        private static string m_mappingResponseJsonSchema = @"{
          ""type"": ""object"",
          ""properties"": {
            ""id"": { ""type"": ""string"" },
            ""Costplan entry"": { ""type"": ""string"" },
            ""Headline 1"": { ""type"": ""string"" },
            ""Headline 2"": { ""type"": ""string"" },
            ""Headline 3"": { ""type"": ""string"" },
            ""Headline 4"": { ""type"": ""string"" },
            ""Headline 5"": { ""type"": ""string"" },
            ""resourceType"": { ""type"": ""string"" },
            ""resourceSubType"": { ""type"": ""string"" },
            ""resourceTypeL3"": { ""type"": ""string"" },
            ""technicalSpec"": { ""type"": ""string"" },
            ""EPD name"": { ""type"": ""string"" }
          },
          ""required"": [
            ""id"",
            ""Costplan entry"",
            ""Headline 1"",
            ""Headline 2"",
            ""Headline 3"",
            ""Headline 4"",
            ""Headline 5"",
            ""resourceType"",
            ""resourceSubType"",
            ""resourceTypeL3"",
            ""technicalSpec"",
            ""EPD name""
          ],
          ""additionalProperties"": false
        }";
    }
}
