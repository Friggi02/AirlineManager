
# GetAll
Used to obtain all the reservations without linked ReservedSeats.
**URL** : `/api/Reservation/GetAll`
**Method** : `GET`
**Auth required** : `Admin`

# GetSingle
Used to obtain a single reservation given the id without linked ReservedSeats without linked ReservedSeats.
**URL** : `/api/Reservation/GetSingle`
**Method** : `GET`
**Auth required** : `Admin`

# GetSingleWithReservedSeats
Used to obtain a single reservation given the id without linked ReservedSeats with linked ReservedSeats.
**URL** : `/api/Reservation/GetSingleWithReservedSeats`
**Method** : `GET`
**Auth required** : `Admin`

# Create
Used to create a reservation.
**URL** : `/api/Reservation/Create`
**Method** : `POST`
**Auth required** : `User`
**Input data :**
```json
{
  "flightId": [int],
  "reservedSeats": [
    {
      "seatCode": [string],
      "passengerName": [string],
      "passengerSurname": [string],
      "passengerAge": [int],
    }
  ]
}
```

# Delete
Used to delete an active reservation given the id.
**URL** : `/api/Reservation/Delete`
**Method** : `DELETE`
**Auth required** : `Admin`

# Restore
Used to restore an incative reservation given the id.
**URL** : `/api/Reservation/Restore`
**Method** : `DELETE`
**Auth required** : `Admin`

# GetByUser

Used to obtain the user and all the reservations without linked ReservedSeats.
**URL** : `/api/Reservation/GetByUser`
**Method** : `GET`
**Auth required** : `User`