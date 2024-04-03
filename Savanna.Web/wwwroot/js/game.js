$(function () {
    $('loadgameButton').on('click', function () {
        //Fetch data from the server with .get
        $.get('/Index/LoadGame', function (data) {
            //Insert returned HTML to modal body
            $('loadGameModal'.modal - body).html(data);
            $('#loadGameModal').modal('show');
        });
    });
});