<?php
    session_start();
    //Various validation for inputs
    $data = json_decode(file_get_contents('php://input'), true);
    //check if both username and pwbolt exist
    if( !(isset($data['username']) &&  isset($data['pwbolt']))) 
    {
     die("Status:ERROR, Params Not Found");
    }
    // check if both username and password are not empty and higher the minimum lenght
    if ( (empty($data["username"]) || empty($data["pwbolt"]))) {
    die("Status:ERROR, Params Invalid");
    }
    // check if username's characters are valid
    if (preg_match("/[^A-Za-z0-9\@]/", $data["username"]))
    {
    die("Status:ERROR, Invalid Characters");
    }
    
    // Init Database connection
    require_once 'config/config.php';
    
    //Initiate Vars
    $username = trim($data["username"]);
    $pwbolt = trim($data["pwbolt"]);
    
    // Prepare Checking of username in database
    if ($stmt = $link->prepare("SELECT guid,pwbolt from tbl_users WHERE username=?")) {
        $stmt->bind_param('s', $username);

        if ($stmt->execute()) {
          $stmt->store_result();

          // Check if username exists. Verify user exists then verify
          if ($stmt->num_rows == 1) {
              $guid = '';
              $verifyPass = '';
              $stmt->bind_result($guid,$verifyPass);
              $stmt->fetch();
              // see php documentation about password_verify and password_hash for more info
              if(password_verify($pwbolt, $verifyPass)) { 
                echo "Status:OK, Login Success.";
                  $_SESSION['loggedin'] = true;
                  $_SESSION['guid'] = $guid;
                  $_SESSION['pwbolt'] = $pwbolt;
              }else
              echo "Status:ERROR, Incorrect Password";
          }else
              echo "Status:ERROR, Account Not Found";
        }else
          echo "Status:ERROR, Failed executing query";

        $stmt->close();

    }else
      echo "Status:ERROR, Failed intitiating query";

    mysqli_close($link);
?>
