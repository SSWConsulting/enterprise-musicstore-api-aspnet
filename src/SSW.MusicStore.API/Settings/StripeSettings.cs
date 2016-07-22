using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSW.MusicStore.API.Settings
{
    public class StripeSettings
    {
        public string SecretKey { get; set; }
        public string PublishableKey { get; set; }
    }
}
