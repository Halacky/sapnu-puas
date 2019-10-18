<?php
require_once '../connection.php'; // подключаем скрипт
 
$link = mysqli_connect($host, $user, $password, $database) 
    or die("Ошибка " . mysqli_error($link)); 
     
$query ="SELECT * FROM `users` WHERE `id` = " . $_GET['id'];
 
$result = mysqli_query($link, $query) or die("Ошибка " . mysqli_error($link)); 
if($result)
{
    $rows = mysqli_num_rows($result); // количество полученных строк
        $row = mysqli_fetch_row($result);
            for ($j = 0 ; $j < 9 ; ++$j) {
                if ($row[$j]=="" || $row[$j]==null)
                echo "nul~";
                else
                echo "$row[$j]~";
            }
    // очищаем результат
    mysqli_free_result($result);
}
 
mysqli_close($link);
?>