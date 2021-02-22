<?php
    session_start();
    if( !(isset($_SESSION['guid']) &&  isset($_SESSION['pwbolt'])))
    die("Status:ERROR,empty params Failed");
    
    // Init Database connection
    require_once 'config/config.php';
    
    //Initiate Vars
    $guid = $_SESSION['guid'];
    $loggedin = $_SESSION['loggedin'];
    $pwbolt = $_SESSION['pwbolt'].$salt;
    $userTable = "tbl_userVault".$guid;

    // Select all of the rows in the user's table.
    if ($stmt = $link->prepare("SELECT * from {$userTable}")) {
        if ($stmt->execute()) {
          $stmt->store_result();
          if ($stmt->num_rows >= 1) {
            $out_id    = NULL;
            $out_website = NULL;
            $out_username = NULL;
            $out_password = NULL;
            if (!$stmt->bind_result($out_id, $out_website,$out_username, $out_password)) {
              echo "Status:ERROR, Binding output parameters failed: (" . $stmt->errno . ") " . $stmt->error;
            }
              $Cryptor = new \Vendor\Library\Cryptor($pwbolt);
              $list = array();
              while ($stmt->fetch()) {  // itterate and decrypt password from the database and then output as plain text.
                $decrypted = $Cryptor->decrypt($out_password);
                $list[] = array('id' => $out_id, 'website' => $out_website, 'username' => $out_username, 'password' => $decrypted);
              }
              echo json_encode($list);
            }else{
              echo "Status:WARNING, Empty Data";
            }
          }else
              echo "Status:ERROR, Failed Executing Query";
        }else
          echo "Status:ERROR, Account Not Found";
    $stmt->close();
    mysqli_close($link);
?>
