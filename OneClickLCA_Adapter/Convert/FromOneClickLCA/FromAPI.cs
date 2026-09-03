using BH.Adapter.OneClickLCA.Objects;
using BH.oM.Adapters.OneClickLCA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BH.Adapter.OneClickLCA
{
    public static partial class Convert
    {
        /***************************************************/
        /*** CalculationResultsApi                      ****/
        /***************************************************/

        public static CalculationResult FromAPI(this CalculationResultRecord record)
        {
            return new CalculationResult()
            {
                DesignId = record.DesignId,
                ToolId = record.ToolId,
                CalculationRuleId = record.CalculationRuleId,
                ResultCategoryId = record.ResultCategoryId,
                Result = record.Result,
                CalculationResultDataSets = record.CalculationResultDataSets.Select(x => x.FromAPI()).ToList(),
            };
        }

        /***************************************************/

        public static CalculationResultDataSet FromAPI(this CalculationResultDataSetRecord record)
        {
            return new CalculationResultDataSet()
            {
                DataSetId = record.DataSetId,
                ResourceId = record.ResourceId,
                ResourceName = record.ResourceName,
                ProfileId = record.ProfileId,
                Result = record.Result,
                MainResourceId = record.MainResourceId,
                MainResourceName = record.MainResourceName,
                MainProfileId = record.MainProfileId,
            };
        }

        /***************************************************/

        public static CalculationTotalResult FromAPI(this CalculationTotalResultRecord record)
        {
            return new CalculationTotalResult()
            {
                CalculationRuleId = record.CalculationRuleId,
                Result = record.Result,
                ResultUnit = record.ResultUnit,
            };
        }

        /***************************************************/
        /**** ProjectsDataApi                           ****/
        /***************************************************/

        public static Project FromAPI(this ProjectRecord record)
        {
            return new Project()
            {
                ProjectId = record.ProjectId,
                Name = record.Name,
                ProjectType = record.ProjectType,
                AssetType = record.ProjectType,
                DateCreated = record.DateCreated,
                LastUpdated = record.LastUpdated,
                AvailableTools = record.AvailableTools,
                Designs = record.Designs.Select(x => x.FromAPI()).ToList(),
                Country = record.Country,
                ProjectCode = record.ProjectCode,
                CreatedBy = record.CreatedBy.FromAPI(),
                LastUpdatedBy = record.LastUpdatedBy.FromAPI(),
                PrimaryTool = record.PrimaryTool,
            };
        }

        /***************************************************/

        public static Design FromAPI(this DesignRecord record)
        {
            return new Design()
            {
                DesignId = record.DesignId,
                Name = record.Name,
                DesignType = record.DesignType,
                ChosenDesign = record.ChosenDesign,
                Baseline = record.Baseline,
                RibaStage = record.RibaStage,
                LastUpdated = record.LastUpdated,
                CreatedBy = record.CreatedBy.FromAPI(),
                LastUpdatedBy = record.LastUpdatedBy.FromAPI(),
            };
        }

        /***************************************************/

        public static BasicUserDetails FromAPI(this BasicUserDetailsRecord record)
        {
            return new BasicUserDetails()
            {
                UserId = record.UserId,
                Email = record.Email,
                Username = record.Name,
            };
        }

        /***************************************************/
    }
}
