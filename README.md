# PWBolt-GUI

A simple Password Manager for saving online accounts. 

![loginform](https://github.com/skrixx68/PWBolt-GUI/blob/main/loginform.PNG)
![mainform](https://github.com/skrixx68/PWBolt-GUI/blob/main/mainform.PNG)


# Features
- Bunifu.UI as Winforms framework.
- REST API as network(php as an API).
- Fetches and Diplay data Asynchronus.

# Libs used.

- https://github.com/Fody/Costura
 -https://github.com/JamesNK/Newtonsoft.Json

# Setup 

- Database 
Create new table 

```sql
CREATE TABLE `tbl_users` (
  `guid` int NOT NULL,
  `username` varchar(50) COLLATE utf8_unicode_ci NOT NULL,
  `pwbolt` varchar(600) COLLATE utf8_unicode_ci NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

```

- Serverside

 Update config/config.php(line 5-8) and enter your database credentials
Example
```php
	define('DB_SERVER', 'localhost');
	define('DB_USERNAME', 'MyUsername');
	define('DB_PASSWORD', 'MyPass');
	define('DB_NAME', 'MyDB_Name');
```

- ClientSide

 Update webserver.cs(line 27) with your webserver

```c#
    private static string webserver = @"http://localhost";
```