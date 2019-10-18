<!DOCTYPE html>
<html lang="ru">
    <head>
        <title>Bids</title>
        <meta name="author" content="Wasilich">
		<link rel="shortcut icon" href="favicon.ico" type="image/x-icon">
        <meta name="description" content="description">
        <meta name="keywords" content="keywords">
        <meta charset="utf-8">
        <link rel="stylesheet" href="../StyleForTable.css" />
    </head>
<body>
    <form action="upd_status.php" method="POST">
<?php
require_once '../connection.php'; // подключаем скрипт
 
$link = mysqli_connect($host, $user, $password, $database) 
    or die("Ошибка " . mysqli_error($link)); 
     
$query ="SELECT * FROM bids";
 
$result = mysqli_query($link, $query) or die("Ошибка " . mysqli_error($link)); 
if($result)
{
    $rows = mysqli_num_rows($result); // количество полученных строк
     
    echo "<div class=\"block-90-offset-005\"> <form action= \"update_status.php\" method= \"POST\"> <table> <thead> <tr> <th>Id</th> <th>Фамилия</th> <th>Имя</th> <th>Отчество</th> <th>Дата рождения</th> <th>Академическая группа</th> <th>Тип заявки</th> <th>Статус заявки</th> </tr> </thead> <tbody>";
    for ($i = 0 ; $i < $rows ; ++$i)
    {
        $row = mysqli_fetch_row($result);
        echo "<tr>";
            for ($j = 0 ; $j < 7 ; ++$j) echo "<td>$row[$j]</td>";
            if ($row[7]) echo "<td><input type=\"checkbox\" checked></td>";
            else echo "<td><input type=\"checkbox\"></td>";
        echo "</tr>";
        
    }
    echo "</tbody></table></div>";
     
    // очищаем результат
    mysqli_free_result($result);
}

mysqli_close($link);
?>
<input type="submit" value="Сохранить">
</form>
</body>
</html>