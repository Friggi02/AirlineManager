
# GetAll
Used to obtain all the flights.
**URL** : `/api/Flight/GetAll`
**Method** : `GET`
**Auth required** : `User`

# GetSingle
Used to obtain a single flight given the id without linked reservations.
**URL** : `/api/Flight/GetSingle`
**Method** : `GET`
**Auth required** : `User`

# SearchingFlights
Used to obtain a list of flights that have a specified arrival and departure airport and that depart in a specified period.
**URL** : `/api/Flight/SearchingFlights`
**Method** : `POST`
**Auth required** : `User`
**Input data :**
```json
{
  "departureAirportId": [string],
  "arrivalAirportId": [string],
  "startPeriod": [DateTime],
  "endPeriod": [DateTime]
}
```
or 
```json
{
  "departureAirportId": [string],
  "arrivalAirportId": [string],
  "startPeriod": [DateTime]
}
```
It will automatically set "endPeriod" one week later.

# Create
Used to create a flight.
**URL** : `/api/Flight/Create`
**Method** : `POST`
**Auth required** : `Admin`
**Input data :**
```json
{
  "airRouteId": [int],
  "departureTime": [DateTime],
  "arrivalTime": [DateTime],
}
```

# Delete
Used to delete an active flight given the id.
**URL** : `/api/Flight/Delete`
**Method** : `DELETE`
**Auth required** : `Admin`

# Update
Used to update a flight given the id.
**URL** : `/api/Flight/Update`
**Method** : `PUT`
**Auth required** : `Admin`
**Input data :**
```json
{
  "flightId": [int],
  "airRouteId": [int],
  "departureTime": [DateTime],
  "arrivalTime": [DateTime],
}
```

# GetFlightsByRoute
Used to get all the flight on a specific route given its id.
**URL** : `/api/Flight/GetFlightsByRoute`
**Method** : `GET`
**Auth required** : `User`

# Duplicate
Used to copy all the flight of a specific day on an etire period.
**URL** : `/api/Flight/# Duplicate`
**Method** : `POST`
**Auth required** : `Admin`
**Input data :**
```json
{
  "day": [DateTime],
  "startPeriod": [DateTime],
  "endPeriod": [DateTime]
}
```