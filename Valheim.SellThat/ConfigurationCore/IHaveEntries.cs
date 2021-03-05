using System.Collections.Generic;

namespace Valheim.SellThat.ConfigurationCore
{
    public interface IHaveEntries
    {
        Dictionary<string, IConfigurationEntry> Entries { get; set; }
    }
}
