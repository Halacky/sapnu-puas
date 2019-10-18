<!DOCTYPE html>
<html lang="ru">
    <head>
        <title>Responsive Table</title>
        <meta name="author" content="Wasilich">
		<link rel="shortcut icon" href="favicon.ico" type="image/x-icon">
        <meta name="description" content="description">
        <meta name="keywords" content="keywords">
        <meta charset="utf-8">
        <link rel="stylesheet" href="../StyleForTable.css" />
    </head>
<body>
<?php
require_once '../connection.php'; // подключаем скрипт
 
$link = mysqli_connect($host, $user, $password, $database) 
    or die("Ошибка " . mysqli_error($link)); 
     
$query ="SELECT * FROM users";
 
$result = mysqli_query($link, $query) or die("Ошибка " . mysqli_error($link)); 
if($result)
{
    $rows = mysqli_num_rows($result); // количество полученных строк
     
    echo "<div class=\"block-90-offset-005\"> <table> <thead> <tr> <th>Id</th> <th>Фамилия</th> <th>Имя</th> <th>Отчество</th> <th>Дата рождения</th> <th>Номер телефона</th> <th>Учебная группа</th> <th>Электронная почта</th> <th>Id в ВКонтакте</th> </tr> </thead> <tbody>";
    for ($i = 0 ; $i < $rows ; ++$i)
    {
        $row = mysqli_fetch_row($result);
        echo "<tr>";
            for ($j = 0 ; $j < 9 ; ++$j) echo "<td>$row[$j]</td>";
        echo "</tr>";
    }
    echo "</tbody>
  </table>
  </div>";
     
    // очищаем результат
    mysqli_free_result($result);
}
 
mysqli_close($link);
?>    
</body>
</html>
