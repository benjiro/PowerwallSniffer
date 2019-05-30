namespace PowerwallSniffer.Model
{
    using System.Text.Json.Serialization;

    public class PercentageModel
    {
        [JsonPropertyName("percentage")]
        public double Percentage { get; set; }
    }
}