namespace PowerwallSniffer.Model
{
    using System.Text.Json.Serialization;

    public class AggregateModel
    {
        [JsonPropertyName("site")]
        public SiteModel Site { get; set; }
        
        [JsonPropertyName("battery")]
        public BatteryModel Battery { get; set; }
        
        [JsonPropertyName("load")]
        public LoadModel Load { get; set; }
        
        [JsonPropertyName("solar")]
        public SolarModel Solar { get; set; }
        
        // Below structures aren't use, these aren't being set
        [JsonPropertyName("busway")]
        public AggregateBaseModal Busway { get; set; }
        
        [JsonPropertyName("frequency")]
        public AggregateBaseModal Frequency { get; set; }
        
        [JsonPropertyName("generator")]
        public AggregateBaseModal Generator { get; set; }
    }
}