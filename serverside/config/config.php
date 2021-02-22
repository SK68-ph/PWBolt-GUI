<?php
	// Database credentials
	require 'config/Cryptor.php';

	define('DB_SERVER', 'localhost');
	define('DB_USERNAME', 'root');
	define('DB_PASSWORD', '');
	define('DB_NAME', 'test');

	// Attempt to connect to MySQL database
	$link = new mysqli(DB_SERVER, DB_USERNAME, DB_PASSWORD, DB_NAME);
	$salt = 'changethissalt';
	if (!$link) {
		die("Error: Unable to connect " . $mysql_db->connect_error);
	}
?>