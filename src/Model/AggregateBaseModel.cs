namespace PowerwallSniffer.Model
{
    using System;
    using System.Text.Json.Serialization;

    public class AggregateBaseModal : BaseModel
    {
        [JsonPropertyName("last_communication_time")]
        public DateTime LastCommunicationTime { get; set; }
        
        [JsonPropertyName("instant_power")]
        public double InstantPower { get; set; }
        
        [JsonPropertyName("Instant_reactive_Power")]
        public double InstantReactivePower { get; set; }
        
        [JsonPropertyName("instant_apparent_power")]
        public double InstantApparentPower { get; set; }
        
        [JsonPropertyName("frequency")]
        public double Frequency { get; set; }
        
        [JsonPropertyName("energy_exported")]
        public double EnergyExported { get; set; }
        
        [JsonPropertyName("energy_imported")]
        public double EnergyImported { get; set; }
        
        [JsonPropertyName("instant_average_voltage")]
        public double InstantAverageVoltage { get; set; }
        
        [JsonPropertyName("instant_total_current")]
        public double InstantTotalCurrent { get; set; }
        
        [JsonPropertyName("i_a_current")]
        public double IACurrent { get; set; }
        
        [JsonPropertyName("i_b_current")]
        public double IBCurrent { get; set; }

        [JsonPropertyName("i_c_current")]
        public double ICCurrent { get; set; }
        
        [JsonPropertyName("timeout")]
        public double Timeout { get; set; }
    }
}