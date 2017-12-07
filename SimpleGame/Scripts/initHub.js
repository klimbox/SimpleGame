$(function () {
    // Ссылка на автоматически-сгенерированный прокси хаба
    var game = $.connection.gameHub;

    // Открываем соединение и добавляемся к списку польз-й хаба
    $.connection.hub.start().done(function () {
        $(document).ready(function () {
            var name = $("#username").val();
            if (name) {
                game.server.joinHub();
            }
        })
    });

    // Получаем список запущенныех игр с доступными слотами для игроков
    game.client.getAvailableGames = function (games) {
        for (var i = 0; i < games.length; i++) {
            showAvailableGame(games[i]);
        }
    }

    // Показывает польз-лю диалог приглашения в игру
    game.client.showInvitation = function (currGame) {
        result = confirm("Хочешь сыграть в '" + currGame.Name + "' с игроком " + currGame.GameOwnerName);
        game.server.invitationConfirm(result, currGame.Id, currGame.GameOwnerName);
    }

    //
    game.client.redirectToGame = function (){
        $(location).attr('href', 'http://localhost:54591/Home/Game')
    }
});

function showAvailableGame(gm) {
    $("#rooms").append('<div class="well" id="' + gm.Id + '"><p>Игра: ' + gm.Name + gm.Id + '</p>' +
        '<p>Инициатор: ' + gm.PlayerNameX + '</p>' +
            '<input type="button" value="Присоединиться" onclick="JoinToGame(' + "'" + gm.Id + "'" + ')" /></div>');
}

