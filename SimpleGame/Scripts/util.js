$(function ()
{
    // Ссылка на автоматически-сгенерированный прокси хаба
    var game = $.connection.gameHub;

    // Функция, вызываемая при подключении нового пользователя
    game.client.onConnected = function (id, userName, allUsers)
    {
        //// установка в скрытых полях имени и id текущего пользователя
        $('#hdId').val(id);

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

    // Показывает польз-лю диалог приглашения в игру
    game.client.showInvitation = function (gameName, caller) {
        result = confirm("Хочешь сыграть в " + gameName + " с игроком " + caller.Name);
        game.server.invitationConfirm(result, caller);
    }

    // Открываем соединение
    $.connection.hub.start().done(function ()
    {
        $(document).ready(function () {
            var gName = $("#gamename").val();
            game.server.startNewGame(gName);

        })     
    });

});
//Добавление нового пользователя
function AddUser(id, name)
{
    var userId = $('#hdId').val();

    if (userId != id)
    {
        $("#player-list").append('<p id="' + id + '"><b>' + name + '   </b>' +
                                '<input type="button" value="Пригласить" onclick="InvPlayer(' + "'" + id + "'" + ')" /></p>');
    }
}

function InvPlayer(id) {
    var game = $.connection.gameHub;
    game.server.invite(id);
}