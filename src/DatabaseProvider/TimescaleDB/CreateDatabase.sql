CREATE SCHEMA IF NOT EXISTS home_data;

create table if not exists home_data.solar
(
    last_communication_time timestamptz,
    instant_power real,
    instant_reactive_power real,
    instant_apparent_power real,
    frequency real,
    energy_exported real,
    energy_imported real,
    instant_average_voltage real,
    instant_total_current real,
    i_a_current real,
    i_b_current real,
    i_c_current real,
    timeout int
);

create table if not exists home_data.site
(
    last_communication_time timestamptz,
    instant_power real,
    instant_reactive_power real,
    instant_apparent_power real,
    frequency real,
    energy_exported real,
    energy_imported real,
    instant_average_voltage real,
    instant_total_current real,
    i_a_current real,
    i_b_current real,
    i_c_current real,
    timeout int
);

create table if not exists home_data.load
(
    last_communication_time timestamptz,
    instant_power real,
    instant_reactive_power real,
    instant_apparent_power real,
    frequency real,
    energy_exported real,
    energy_imported real,
    instant_average_voltage real,
    instant_total_current real,
    i_a_current real,
    i_b_current real,
    i_c_current real,
    timeout int
);

create table if not exists home_data.battery
(
    last_communication_time timestamptz,
    instant_power real,
    instant_reactive_power real,
    instant_apparent_power real,
    frequency real,
    energy_exported real,
    energy_imported real,
    instant_average_voltage real,
    instant_total_current real,
    i_a_current real,
    i_b_current real,
    i_c_current real,
    timeout int,
    batt_percentage real
);

SELECT create_hypertable('home_data.site', 'last_communication_time', if_not_exists => TRUE);
SELECT create_hypertable('home_data.load', 'last_communication_time', if_not_exists => TRUE);
SELECT create_hypertable('home_data.battery', 'last_communication_time', if_not_exists => TRUE);
SELECT create_hypertable('home_data.solar', 'last_communication_time', if_not_exists => TRUE);

