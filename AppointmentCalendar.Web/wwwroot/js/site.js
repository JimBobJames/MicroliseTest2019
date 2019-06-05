$(function() {
    $(".event").on('click',
        function () {
            var button = $(this).parent().children('.js-eventEdit').first();
            button.click();
        });

    $("#DeleteAppointment").on('click',
        function() {
            alert('Delete Clicked');
        });
});
