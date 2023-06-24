
# GetAll
Used to obtain all the airroutes.
**URL** : `/api/AirRoute/GetAll`
**Method** : `GET`
**Auth required** : `Admin`

# GetSingle
Used to obtain a single airroute given the id without linked flights.
**URL** : `/api/AirRoute/GetSingle`
**Method** : `GET`
**Auth required** : `Admin`

# GetSingleWithFlights
Used to obtain a single airroute given the id with linked flights.
**URL** : `/api/AirRoute/GetSingleWithFlights`
**Method** : `GET`
**Auth required** : `Admin`

# Create
Used to create an airroute.
**URL** : `/api/AirRoute/Create`
**Method** : `POST`
**Auth required** : `Admin`
**Input data :**
```json
{
  "departureAirportId": [int],
  "arrivalAirportId": [int],
  "aircraftId": [int]
}
```

# Delete
Used to delete an active airroute given the id.
**URL** : `/api/AirRoute/Delete`
**Method** : `DELETE`
**Auth required** : `Admin`

# Restore
Used to restore an incative airroute given the id.
**URL** : `/api/AirRoute/Restore`
**Method** : `DELETE`
**Auth required** : `Admin`

# Update
Used to update an airroute.
**URL** : `/api/AirRoute/Update`
**Method** : `PUT`
**Auth required** : `Admin`
**Input data :**
```json
{
  "airRouteId": [int],
  "aircraftId": [int]
}
```