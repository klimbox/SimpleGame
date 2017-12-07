$(function ()
{
    var game = $.connection.gameHub;

    $("#00").click(function () {
        game.server.doAction($('#username').val(), "0,0");
    });
    $("#01").click(function () {
        game.server.doAction($('#username').val(), "0,1");
    });
    $("#02").click(function () {
        game.server.doAction($('#username').val(), "0,2");
    });
    $("#10").click(function () {
        game.server.doAction($('#username').val(), "1,0");
    });
    $("#11").click(function () {
        game.server.doAction($('#username').val(), "1,1");
    });
    $("#12").click(function () {
        game.server.doAction($('#username').val(), "1,2");
    });
    $("#20").click(function () {
        game.server.doAction($('#username').val(), "2,0");
    });
    $("#21").click(function () {
        game.server.doAction($('#username').val(), "2,1");
    });
    $("#22").click(function () {
        game.server.doAction($('#username').val(), "2,2");
    });

    game.client.sendField = function (field)
    {
        var obj = JSON.parse(field);
        // изменение поля
        for (var i = 0; i < obj.length; i++) {
            for (var j = 0; j < obj[i].length; j++) {
                var x = "-";
                if (obj[i][j] == 1) {
                    x = "X"
                }
                if (obj[i][j] == 2) {
                    x = "O";
                }
                $("#" + i + j).html(x);
            }
        }
    };

    game.client.showMessageAndRedirect = function (message)
    {
        alert(message);
        $(location).attr('href', 'http://localhost:54591');
    };
});