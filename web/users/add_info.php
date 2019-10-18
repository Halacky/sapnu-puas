<?php
require_once '../connection.php'; // подключаем скрипт
// подключаемся к серверу
$link = mysqli_connect($host, $user, $password, $database) 
        or die("Ошибка " . mysqli_error($link));

    $id = htmlentities(mysqli_real_escape_string($link, $_GET['id']));
    $type = htmlentities(mysqli_real_escape_string($link, $_GET['type']));
    $val = htmlentities(mysqli_real_escape_string($link, $_GET['value']));
    $query ="UPDATE `users` SET `$type`='$val' WHERE `id`='$id'";
    $result = mysqli_query($link, $query) or die("Ошибка " . mysqli_error($link)); 
 
    if($result)
        echo "done";
// закрываем подключение
mysqli_close($link);
?>