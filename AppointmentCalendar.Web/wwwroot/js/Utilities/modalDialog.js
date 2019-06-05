$(function () {
    // all buttons with data-toggle equal to ajax-modal
    var placeholderElement = $('#modal-placeholder');
    $('button[data-toggle="ajax-modal"]').click(function (event) {
        var url = $(this).data('url');
        $.get(url).done(function (data) {
            placeholderElement.html(data);
            placeholderElement.find('.modal').modal({ backdrop: "static" });
        });
    });
    placeholderElement.on('click', '[data-save="modal"]', function (event) {
        event.preventDefault();

        var form = $(this).parents('.modal').find('form');
        var actionUrl = form.attr('action');
        var dataToSend = form.serialize();

        $.post(actionUrl, dataToSend).done(function (data) {
            var newBody = $('.modal-body', data);
            placeholderElement.find('.modal-body').replaceWith(newBody);

            // find IsValid input field and check it's value
            // if it's valid then hide modal window
            var isValid = newBody.find('[name="IsValid"]').val() === 'True';

            if (isValid) {
                placeholderElement.find('.modal').modal('hide');

                var delay = 0;
                var delayField = newBody.find('[name="Delay"]');
                if (delayField.length > 0) {
                    var delay = parseInt(delayField.val());
                    setTimeout(function () { location.reload(); }, delay);
                } else {
                    location.reload();
                }
            }
        });
    });
});