IF NOT EXISTS (SELECT * FROM AirRoutes)
BEGIN
BULK INSERT AirlineDb.dbo.Airports
FROM 'C:\all_docs\Airline Manager\db\Airports.csv'
WITH
(
    FIELDTERMINATOR = ',',
    ROWTERMINATOR = '0x0A',
    FIRSTROW = 2 -- Skip the header row if present
);
end

IF NOT EXISTS (SELECT * FROM AirRoutes)
BEGIN
BULK INSERT AirlineDb.dbo.AirRoutes
FROM 'C:\all_docs\Airline Manager\db\AirRoutes.csv'
WITH
(
    FIELDTERMINATOR = ',',
    ROWTERMINATOR = '0x0A',
    FIRSTROW = 2 -- Skip the header row if present
);
end

IF NOT EXISTS (SELECT * FROM Flights)
BEGIN
BULK INSERT AirlineDb.dbo.Flights
FROM 'C:\all_docs\Airline Manager\db\Flights.csv'
WITH
(
    FIELDTERMINATOR = ',',
    ROWTERMINATOR = '0x0A',
    FIRSTROW = 2 -- Skip the header row if present
);
end