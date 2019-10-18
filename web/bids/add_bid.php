<?php
require_once '../connection.php'; // подключаем скрипт
 
$link = mysqli_connect($host, $user, $password, $database) 
    or die("Ошибка " . mysqli_error($link));
// Обрабатываем гет запрос
$id = $_GET['id'];
$bid_type = htmlentities(mysqli_real_escape_string($link, $_GET['bid_type']));

// Получаем инфу о пользователе
$query ="SELECT * FROM `users` WHERE `id`='$id'";
 
$result = mysqli_query($link, $query) or die("Ошибка " . mysqli_error($link)); 
if($result)
{
    $row = mysqli_fetch_row($result);
    $id = $row[0];
    $last_name = $row[1];
    $first_name = $row[2];
    $father_name = $row[3];
    $birthdate = $row[4];
    $ac_group = $row[6];
    mysqli_free_result($result);
}

// Добавляем заявку
$query = "INSERT INTO `bids`(`id`, `last_name`, `first_name`, `father_name`, `birthdate`, `ac_group`, `bid_type`) VALUES ('$id', '$last_name', '$first_name', '$father_name', '$birthdate', '$ac_group', '$bid_type')";
 
$result = mysqli_query($link, $query) or die("Error " . mysqli_error($link)); 
    if($result)
    {
        echo "done";
    }

mysqli_close($link);
?>