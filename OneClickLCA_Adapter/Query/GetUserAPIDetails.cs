using BH.Adapter.OneClickLCA.Objects;
using BH.Engine.Base;
using BH.oM.Adapter;
using BH.oM.Adapters.SQL;
using BH.oM.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BH.Adapter.OneClickLCA
{
    public static partial class Query
    {
        //TODO: return both clientID and clientSecret(API key)
        public static UserAPIDetails GetUserAPIDetails(this IBHoMAdapter adapter, string userId = null, bool defaultUseEnvironmentName = true)
        {
            if (adapter == null)
            {
                BH.Engine.Base.Compute.RecordError("The user authentication adapter was null when requesting the users API key. Please provide a valid adapter when creating the OneClickLCAAdapter.");
                return null;
            }

            if (string.IsNullOrEmpty(userId) && !defaultUseEnvironmentName)
            {
                BH.Engine.Base.Compute.RecordError("The given user ID/name was null or empty, which is invalid for getting the API key. Please provide a valid user ID.");
                return null;
            }

            //TODO: sanitise the user ID!!!
            TableRequest request = new TableRequest()
            {
                Table = "dbo.GetAPIKeyByUser",
                Filter = $"User = '{userId ?? System.Environment.UserName}'"
            };

            List<object> pulled = adapter.Pull(request).ToList();

            CustomObject first = pulled.OfType<CustomObject>().FirstOrDefault();

            if (first == null)
            {
                BH.Engine.Base.Compute.RecordError($"The user {userId} is not authorised to pull from the One Click LCA API.");
                return null;
            }

            //assume that the resulting dictionary is correct if there were no errors and the return was not empty.
            return new UserAPIDetails()
            {
                UserId = userId,
                ClientId = (string)first.CustomData["ClientId"],
                ClientSecret = (string)first.CustomData["ClientSecret"]
            };
        }
    }
}
