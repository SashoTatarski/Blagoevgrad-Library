$('#add-author').click(function () {
    $('#create-author-modal').modal('show');
});

$('#create-author').click(function () {
    const authorNameValue = $('#new-author-name').val();

    $.ajax({
        type: 'POST',
        url: '/bookmanagement/addauthorrest',
        data: { authorName: authorNameValue },
        success: function (response) {
            $('#create-author-modal').modal('hide');

            const presentOption = Array.from($('#authors-selectlist')[0].children).find(o => o.value === response.id);

            if (presentOption) {
                $(presentOption).attr('selected', 'selected');
            } else {
                const option = $(`<option value="${response.id}">${response.name}</option>`);
                $('#authors-selectlist').append(option);
                option.attr('selected', 'selected');
            }
        }
    });
});



$('#add-publisher').click(function () {

    $('#create-publisher-modal').modal('show');
});

$('#create-publisher').click(function () {
    const authorPublisherValue = $('#new-publisher-name').val();

    $.ajax({
        type: 'POST',
        url: '/bookmanagement/addpublisherrest',
        data: { publisherName: authorPublisherValue },
        success: function (response) {
            $('#create-publisher-modal').modal('hide');

            const presentOption = Array.from($('#publisher-selectlist')[0].children).find(o => o.value === response.id);

            if (presentOption) {
                $(presentOption).attr('selected', 'selected');
            } else {
                const option = $(`<option value="${response.id}">${response.name}</option>`);
                $('#publisher-selectlist').append(option);
                option.attr('selected', 'selected');
            }
        }
    });
});

$('#add-genre').click(function () {
    $('#create-genre-modal').modal('show');
});

$('#create-genre').click(function () {
    const genreValue = $('#new-genre-name').val();

    $.ajax({
        type: 'POST',
        url: '/bookmanagement/addgenrerest',
        data: { genreName: genreValue },
        success: function (response) {
            $('#create-genre-modal').modal('hide');

            const presentOption = Array.from($('#genre-selectlist')[0].children).find(o => o.value === response.id.toString());

            console.log($('#genre-selectlist')[0].children);
            console.log(presentOption);

            if (presentOption) {
                $(presentOption).attr('selected', 'selected');
            } else {
                const option = $(`<option value="${response.id}">${response.name}</option>`);
                $('#genre-selectlist').append(option);
                option.attr('selected', 'selected');
            }
        }
    });
});