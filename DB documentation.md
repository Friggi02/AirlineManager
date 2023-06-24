## Aircraft
| Column | Data type | Key |
| ---------- | --------- | --------- |
| AircraftId | int | PK |
| Name | nvarchar(30) |

## Airport
| Column | Data type | Key |
| ---------- | --------- | --------- |
| AirportId | int | PK |
| Name | nvarchar(44) |
| IataCode | nvarchar(3) |
| City | nvarchar(23) |
| Country | nvarchar(14) |
| IsSchengen | bit |

## AirRoute
| Column | Data type | Key |
| ---------- | --------- | --------- |
| AirRouteId | int | PK |
| DepartureAirportId | int | FK |
| ArrivalAirportId | int | FK |
| AircraftId | int | FK |
| IsDeleted | bit |

## Flight
| Column | Data type | Key |
| ---------- | --------- | --------- |
| FlightId | int | PK |
| AirRouteId | int | FK |
| DepartureTime | datetime2(7) |
| ArrivalTime | datetime2(7) |
| IsDeleted | bit |

## Reservation
| Column | Data type | Key |
| ---------- | --------- | --------- |
| ReservationId | int | PK |
| FlightId | int | FK |
| IsDeleted | bit |
| UserId | nvarchar(450) | FK |

## ReservedSeat
| Column | Data type | Key |
| ---------- | --------- | --------- |
| ReservedSeatId | int | PK |
| SeatCode | nvarchar(3) |
| PassengerName | nvarchar(50) |
| PassengerSurname | nvarchar(50) |
| PassengerAge | tinyint |
| ReservationId | int | FK |
| IsDeleted | bit |

## Seat
| Column | Data type | Key |
| ---------- | --------- | --------- |
| SeatId | int | PK |
| AircraftId | int | FK |
| Code | nvarchar(3) |
| Row | tinyint |
| Column | nvarchar(1) |
| Category | tinyint |

## User
| Column | Data type | Key |
| ---------- | --------- | --------- |
| Id | nvarchar(450)  | PK |
| Discriminator | nvarchar(max)  |
| IsDeleted | bit |
| UserName | nvarchar(256) |
| NormalizedUserName | nvarchar(256) |
| Email | nvarchar(256) |
| NormalizedEmail | nvarchar(256) |
| EmailConfirmed | bit |
| PasswordHash | nvarchar(max) |
| SecurityStamp | nvarchar(max) |
| ConcurrencyStamp | nvarchar(max) |
| PhoneNumber | nvarchar(max) |
| PhoneNumberConfirmed | bit |
| TwoFactorEnabled | bit |
| LockoutEnd | datetimeoffset(7) |
| LockoutEnabled | bit |
| AccessFailedCount | int |