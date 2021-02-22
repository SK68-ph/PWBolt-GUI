<?php
    session_start();
    $data = json_decode(file_get_contents('php://input'), true);

    //check if both id and guid exist
    if( !(isset($data['id']) && isset($_SESSION['guid'])))
    die("Status:ERROR,empty params Failed");

    // check if both id and guid are not empty
    if ((empty($data['id']) || empty($_SESSION['guid'])))
    die("Status:ERROR, Params Invalid");

    // Init Database connection
    require_once 'config/config.php';

    // Init vars
    $id = trim($data["id"]);
    $guid = $_SESSION['guid'];
    $userTable = "tbl_userVault".$guid;

    // Delete record in user's table using the id from POST data
    $sql = "DELETE FROM {$userTable} WHERE id=?";
    if ($stmt = $link->prepare($sql)) {
      $stmt->bind_param('s', $id);

      if ($stmt->execute()) 
        echo 'Status:OK, Account Deleted';
      else
        echo 'Status:ERROR, Account Deletetion Failed';
      
      $stmt->close();
    }else 
      echo "Status:ERROR, Failed intitiating query";

    mysqli_close($link);
?>
