$(document).ready(function () {
    $("#Title, #Message").keyup(function () {
        var payloadObject = {
            title: $("#Title").val(),
            message: $("#Message").val()
        };

        $("#Payload").val(JSON.stringify(payloadObject));
    });
});