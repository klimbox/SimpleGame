$(function ()
{
    // Ссылка на автоматически-сгенерированный прокси хаба
    var game = $.connection.gameHub;

    // Открываем соединение и стартуем новую игру
    $.connection.hub.start().done(function () {
        $(document).ready(function () {
            var gName = $("#gamename").val();
            game.server.startNewGame(gName);

        })
    });

    // После запуска игры, получаем список доступных пользователей 
    game.client.showUsers = function (users) {
        $("div#player-list").empty();
        $("#player-list").append('<h3>Игроки онлайн:</h3>');
        for (var i = 0; i < users.length; i++) {
            AddUserToPage(users[i].Name);
        }
    }

    //
    //game.client.showMessage = function (msg) {
    //    alert(msg);
    //}

    //// Показывает польз-лю диалог приглашения в игру
    //game.client.showInvitation = function (gameName, caller) {
    //    result = confirm("Хочешь сыграть в '" + gameName + "' с игроком " + caller.Name);
    //    game.server.invitationConfirm(result, caller);
    //}

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

});

//Добавление нового пользователя
function AddUserToPage(usrName) {
    $("#player-list").append('<div class="well" id="' + usrName + '"><p>Игрок: ' + usrName + '</p>' +
        '<input type="button" value="Пригласить" onclick="InvitePlayer(' + "'" + usrName + "'" + ')" /></div>');
}


function InvitePlayer(name) {
    var game = $.connection.gameHub;
    game.server.invite(name);
}