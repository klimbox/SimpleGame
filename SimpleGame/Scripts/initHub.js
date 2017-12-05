$(function () {
    // Ссылка на автоматически-сгенерированный прокси хаба
    var game = $.connection.gameHub;

    // Открываем соединение
    $.connection.hub.start().done(function () {
        $(document).ready(function () {
            var name = $("#username").val();
            if (name) {
                game.server.connect(name);

                alert('Connected!');
            }
        })
    });

});