$(function ()
{
    $('#gameBody').hide();
    $('#loginBlock').show();
    // Ссылка на автоматически-сгенерированный прокси хаба
    var game = $.connection.gameHub;
    // Объявление функции, которая хаб вызывает при получении сообщений
    game.client.sendField = function (field)
    {
        var obj = JSON.parse(field);
        // Добавление сообщений на веб-страницу
        for (var i = 0; i < obj.length; i++)
        {
            for (var j = 0; j < obj[i].length; j++)
            {
                var x = "-";
                if (obj[i][j] == 1)
                {
                    x = "X"
                }
                if (obj[i][j] == 2)
                {
                    x = "O";
                }
                $("#" + i + j).html(x);
            }
        }        
    };

    game.client.showMessage = function (message)
    {
        alert(message);
    };

    // Функция, вызываемая при подключении нового пользователя
    game.client.onConnected = function (id, userName, allUsers)
    {

        $('#loginBlock').hide();
        $('#gameBody').show();
        // установка в скрытых полях имени и id текущего пользователя
        $('#hdId').val(id);
        $('#username').val(userName);
        $('#header').html('<h3>Добро пожаловать, ' + userName + '</h3>');

        // Добавление всех пользователей
        for (i = 0; i < allUsers.length; i++)
        {

            AddUser(allUsers[i].ConnectionId, allUsers[i].Name);
        }
    }

    // Добавляем нового пользователя
    game.client.onNewUserConnected = function (id, name)
    {

        AddUser(id, name);
    }

    // Удаляем пользователя
    game.client.onUserDisconnected = function (id, userName)
    {

        $('#' + id).remove();
    }

    // Открываем соединение
    $.connection.hub.start().done(function ()
    {
        // обработка логина
        $("#btnLogin").click(function ()
        {

            var name = $("#txtUserName").val();
            if (name.length > 0)
            {
                game.server.connect(name);
            }
            else
            {
                alert("Введите имя");
            }
        });

        $("#00").click(function ()
        {
            game.server.send($('#username').val(), "0,0");
        });
        $("#01").click(function ()
        {
            game.server.send($('#username').val(), "0,1");
        });
        $("#02").click(function ()
        {
            game.server.send($('#username').val(), "0,2");
        });
        $("#10").click(function ()
        {
            game.server.send($('#username').val(), "1,0");
        });
        $("#11").click(function ()
        {
            game.server.send($('#username').val(), "1,1");
        });
        $("#12").click(function ()
        {
            game.server.send($('#username').val(), "1,2");
        });
        $("#20").click(function ()
        {
            game.server.send($('#username').val(), "2,0");
        });
        $("#21").click(function ()
        {
            game.server.send($('#username').val(), "2,1");
        });
        $("#22").click(function ()
        {
            game.server.send($('#username').val(), "2,2");
        });
    });
});
// Кодирование тегов
function htmlEncode(value)
{
    var encodedValue = $('<div />').text(value).html();
    return encodedValue;
}
//Добавление нового пользователя
function AddUser(id, name)
{

    var userId = $('#hdId').val();

    if (userId != id)
    {

        $("#chatusers").append('<p id="' + id + '"><b>' + name + '</b></p>');
    }
}