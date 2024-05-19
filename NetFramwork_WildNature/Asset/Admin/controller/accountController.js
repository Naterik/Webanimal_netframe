$(document).on('click', '.btn-active', function (e) {
    e.preventDefault();
    var btn = $(this);
    var id = btn.data('id');

    $.ajax({
        url: "/Admin/Account/ChangeStatus",
        type: "POST",
        data: { id: id },
        dataType: "json",
        success: function (response) {
            if (response.success) {
                // Update button state
                if (response.state) {
                    btn.removeClass('btn-danger').addClass('btn-success').text('Kích hoạt');
                } else {
                    btn.removeClass('btn-success').addClass('btn-danger').text('Khóa');
                }

                // Update table row
                var row = $('tr[data-id="' + response.id + '"]');
                row.find('.account-state').text(response.state ? 'Kích hoạt' : 'Khóa');
                row.find('.account-locked').text(response.isLocked ? 'Có' : 'Không');
            }
        }
    });
});