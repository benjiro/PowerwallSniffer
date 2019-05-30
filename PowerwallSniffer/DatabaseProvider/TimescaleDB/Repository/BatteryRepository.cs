namespace PowerwallSniffer.DatabaseProvider.TimescaleDB.Repository
{
    using Npgsql;
    using NpgsqlTypes;
    using Model;

    public class BatteryRepository : IRepository<BatteryModel>
    {
        private readonly string _connectionString;

        public BatteryRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        private NpgsqlConnection Connection => new NpgsqlConnection(_connectionString);

        public void Insert(BatteryModel item)
        {
            using (var dbConnection = Connection)
            {
                dbConnection.Open();

                using (var command = new NpgsqlCommand(@"
                            INSERT INTO home_data.battery
                                (
                                    last_communication_time,
                                    instant_power,
                                    instant_reactive_power,
                                    instant_apparent_power,
                                    frequency,
                                    energy_exported,
                                    energy_imported,
                                    instant_average_voltage,
                                    instant_total_current,
                                    i_a_current,
                                    i_b_current,
                                    i_c_current,
                                    timeout,
                                    batt_percentage
                                )
                             
                            VALUES 
                                (
                                    @last_communication_time,
                                    @instant_power,
                                    @instant_reactive_power,
                                    @instant_apparent_power,
                                    @frequency,
                                    @energy_exported,
                                    @energy_imported,
                                    @instant_average_voltage,
                                    @instant_total_current,
                                    @i_a_current,
                                    @i_b_current,
                                    @i_c_current,
                                    @timeout,
                                    @batt_percentage
                                )", dbConnection))
                {
                    command.Parameters.AddWithValue("last_communication_time", NpgsqlDbType.TimestampTz,
                        item.LastCommunicationTime);
                    command.Parameters.AddWithValue("instant_power", NpgsqlDbType.Real, item.InstantPower);
                    command.Parameters.AddWithValue("instant_reactive_power", NpgsqlDbType.Real,
                        item.InstantReactivePower);
                    command.Parameters.AddWithValue("instant_apparent_power", NpgsqlDbType.Real,
                        item.InstantApparentPower);
                    command.Parameters.AddWithValue("frequency", NpgsqlDbType.Real, item.Frequency);
                    command.Parameters.AddWithValue("energy_exported", NpgsqlDbType.Real, item.EnergyExported);
                    command.Parameters.AddWithValue("energy_imported", NpgsqlDbType.Real, item.EnergyImported);
                    command.Parameters.AddWithValue("instant_average_voltage", NpgsqlDbType.Real,
                        item.InstantAverageVoltage);
                    command.Parameters.AddWithValue("instant_total_current", NpgsqlDbType.Real,
                        item.InstantTotalCurrent);
                    command.Parameters.AddWithValue("i_a_current", NpgsqlDbType.Real, item.IACurrent);
                    command.Parameters.AddWithValue("i_b_current", NpgsqlDbType.Real, item.IBCurrent);
                    command.Parameters.AddWithValue("i_c_current", NpgsqlDbType.Real, item.ICCurrent);
                    command.Parameters.AddWithValue("timeout", NpgsqlDbType.Integer, item.Timeout);
                    command.Parameters.AddWithValue("batt_percentage", NpgsqlDbType.Real, item.Percentage);

                    command.ExecuteNonQuery();
                }

                dbConnection.Close();
            }
        }
    }
}