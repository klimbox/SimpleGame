$(function ()
{
    // $('#gameBody').hide();
    $('#loginBlock').show();
    // Ссылка на автоматически-сгенерированный прокси хаба
    var game = $.connection.gameHub;

    // Функция, вызываемая при подключении нового пользователя
    game.client.onConnected = function (id, userName, allUsers)
    {

        $('#loginBlock').hide();
        $('#gameBody').show();
        $('#Users').show();
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
    });
});
//Добавление нового пользователя
function AddUser(id, name)
{

    var userId = $('#hdId').val();

    if (userId != id)
    {

        $("#chatusers").append('<p id="' + id + '"><b>' + name + '</b></p>');
    }
}