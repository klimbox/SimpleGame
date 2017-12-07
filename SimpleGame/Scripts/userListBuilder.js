$(function () {
    // Ссылка на автоматически-сгенерированный прокси хаба
    var game = $.connection.gameHub;

    // После запуска игры, получаем список доступных пользователей 
    game.client.showUsers = function (users) {
        $("div#player-list").empty();
        $("#player-list").append('<h3>Игроки онлайн:</h3>');
        for (var i = 0; i < users.length; i++) {
            AddUserToPage(users[i].Name);
        }
    }
});

function AddUserToPage(usrName) {
    $("#player-list").append('<div class="well" id="' + usrName + '"><p>Игрок: ' + usrName + '</p>' +
        '<input type="button" value="Пригласить" onclick="InvitePlayer(' + "'" + usrName + "'" + ')" /></div>');
}