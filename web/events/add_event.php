<?php
require_once '../connection.php'; // подключаем скрипт
 
$link = mysqli_connect($host, $user, $password, $database) 
    or die("Ошибка " . mysqli_error($link));
    
$name = htmlentities(mysqli_real_escape_string($link, $_POST['name']));
$description = htmlentities(mysqli_real_escape_string($link, $_POST['description']));
$key_word = htmlentities(mysqli_real_escape_string($link, $_POST['key_word']));
     
$query = "INSERT INTO `events`(`name`, `description`, `key_word`) VALUES ('$name', '$description', '$key_word')";
 
$result = mysqli_query($link, $query) or die("Error " . mysqli_error($link)); 
    if($result)
    {
        echo "done";
    }
 
mysqli_close($link);
?>