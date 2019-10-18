<?php
require_once '../connection.php'; // подключаем скрипт
 
$link = mysqli_connect($host, $user, $password, $database) 
    or die("Ошибка " . mysqli_error($link));
    
$id = $_GET['id'];
     
$query = "INSERT INTO `users` (`id`) VALUES ('$id')";
 
$result = mysqli_query($link, $query) or die("Error " . mysqli_error($link)); 
    if($result)
    {
        echo "done";
    }
 
mysqli_close($link);
?>