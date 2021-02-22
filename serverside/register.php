<?php
    //Various validation for inputs
    $data = json_decode(file_get_contents('php://input'), true);
    //check if both username and pwbolt exist
    if( !(isset($data['username']) &&  isset($data['pwbolt']))) 
     die("Status:ERROR, Params Not Found");

    // check if both username and password are not empty and higher the minimum lenght
    if ( (empty($data["username"]) || empty($data["pwbolt"]) || strlen($data["username"]) < 5 || strlen($data["pwbolt"]) < 5 )) 
    die("Status:ERROR, username and password too short or invalid params.");
    
    // check if username's characters are valid
    if (preg_match("/[^A-Za-z0-9]/", $data["username"]))
    die("Status:ERROR, Invalid Characters");
    
    // Init Database connection
    require_once 'config/config.php';

    //Initiate Vars
    $username = trim($data["username"]);
    $pwbolt = trim($data["pwbolt"]);

    // see php documentation about password_verify and password_hash for more info
    $hashedPw = password_hash($pwbolt, PASSWORD_BCRYPT);
    if(password_verify($pwbolt, $hashedPw)) {
      $random = random_int(1,2147483047); //Generate a random number

      // it will be then used for both uniqid in user's table and the user's own private table as a name
      if ($stmt = $link->prepare("INSERT INTO tbl_users() VALUES(?,?,?)")) {
        $stmt->bind_param('sss', $random,$username,$hashedPw);

          if ($stmt->execute()) {
            echo "Status:OK , Register Done";
            $userTable = "tbl_userVault".$random;
            $sql2 = "CREATE TABLE IF NOT EXISTS {$userTable}(id int PRIMARY KEY auto_increment,website varchar(100) NOT NULL,username varchar(100) NOT NULL,password varchar(600) NOT NULL)";
            
            if ($stmt2 = $link->prepare($sql2)) {
              $stmt2->execute();
              $stmt2->close();
            }
          }else {
            echo "Status:ERROR, Username already taken";
          }
          $stmt->close();
        }
      }
    mysqli_close($link);
    //#https://elucidative-designa.000webhostapp.com/PWBOLT_register.php
?>
