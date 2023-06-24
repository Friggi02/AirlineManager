
# Login
Used to log into the website.
**URL** : `/api/Authenticate/Login`
**Method** : `POST`
**Auth required** : `None`
**Input data :**
```json
{
    "username": [string],
    "password": [string]
}
```

# Register
Uset to register a new user into the website.
**URL :** `/api/Authenticate/Register`
**Method** : `POST`
**Auth required** : `None`
**Input data :**
```json
{
  "username": [string],
  "email": [string],
  "password": [string]
}
```

# RegisterAdmin
Used to register a new admin into the website.
**URL** : `/api/Authenticate/RegisterAdmin`
**Method** : `POST`
**Auth required** : `Admin`
**Input data :**
```json
{
  "username": [string],
  "email": [string],
  "password": [string]
}
```

# GetAllUsers
Used to obtain all users.
**URL** : `/api/Authenticate/GetAllUsers`
**Method** : `Get`
**Auth required** : `Admin`

# GetSingle
Used to obtain a single user given the id without linked reservations.
**URL** : `/api/Authenticate/GetSingle`
**Method** : `GET`
**Auth required** : `Admin`

# Update
Used by admin to update a user Email.
**URL** : `/api/Authenticate/Update`
**Method** : `PUT`
**Auth required** : `Admin`
**Input data :**
```json
{
  "userId": [string],
  "email": [string]
}
```

# SelfUpdate
Used by user to update its onw Email.
**URL** : `/api/Authenticate/SelfUpdate`
**Method** : `PUT`
**Auth required** : `Admin`
**Input data :**
```json
{
  "email": [string]
}
```

# SelfDelete
Used by user to delete its onw account.
**URL** : `/api/Authenticate/SelfDelete`
**Method** : `DELETE`
**Auth required** : `User`

# Delete
Used by admin to delete a user account given the id.
**URL** : `/api/Authenticate/Delete`
**Method** : `DELETE`
**Auth required** : `Admin`

# Restore
Used to restore an incative User given the id.
**URL** : `/api/Authenticate/Restore`
**Method** : `DELETE`
**Auth required** : `Admin`

# Test
Used to test the connection without authorization
**URL** : `/api/Authenticate/Test`
**Method** : `GET`
**Auth required** : `None`