using BH.oM.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace BH.Adapter.OneClickLCA.Objects
{
    public class UserAPIDetails: IObject
    {
        public virtual string UserId { get; set; }

        public virtual string ClientId { get; set; }

        public virtual string ClientSecret { get; set; }
    }
}
