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

});

function showAvailableGame(gm) {
    $("#rooms").append('<div class="well" id="' + gm.Id + '"><p>Игра: ' + gm.Name + '</p>' +
            '<input type="button" value="Присоединиться" onclick="JoinToGame(' + "'" + gm.Id + "'" + ')" /></div>');
}
