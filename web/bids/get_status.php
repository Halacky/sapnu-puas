<?php
require_once '../connection.php'; // подключаем скрипт
 
$link = mysqli_connect($host, $user, $password, $database) 
    or die("Ошибка " . mysqli_error($link)); 
     
$id = $_GET['id'];

$query = "SELECT `bid_type`, `bid_status` FROM `bids` WHERE `id`='$id'";
 
$result = mysqli_query($link, $query) or die("Ошибка " . mysqli_error($link)); 
if($result)
{
    $rows = mysqli_num_rows($result); // количество полученных строк
    for ($j = 0 ; $j < $rows ; ++$j) {
        $row = mysqli_fetch_row($result);
            echo "$row[0]: ";
            if ($row[1]) {
                echo "готова!";
            } else {
                echo "не готова";
            }
            if ($j!=$rows-1) echo "~";
    }
    // очищаем результат
    mysqli_free_result($result);
}
 
mysqli_close($link);
?>