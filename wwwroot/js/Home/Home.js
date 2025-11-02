$(document).ready(function () {

    // Chi tiết sân bóng trong modal
    $('.container').on('click', '.show-court-details', function () {
        var courtId = $(this).data('court-id');
        var modal = $('#courtInfoModal');
        var modalBody = modal.find('.modal-body');
        var modalTitle = modal.find('.modal-title');
        var modalBookButton = modal.find('#modalBookButton');

        modalBody.html('<p class="text-center">Đang tải thông tin...</p>');
        modalTitle.text('Đang tải...'); 
        modalBookButton.attr('href', '#'); 

        modal.modal('show');

        $.ajax({
            url: '/Home/GetCourtDetailsPartial', 
            type: 'GET', 
            data: { courtId: courtId }, 
            success: function (response) {
                modalBody.html(response);

                var courtNameInPartial = $(response).find('h6').first().text() || "Thông tin sân";
                modalTitle.text(courtNameInPartial);

                var bookingUrl = '/Booking/Create?courtId=' + courtId; 
                modalBookButton.attr('href', bookingUrl);

            },
            error: function (xhr, status, error) {
                modalBody.html('<p class="text-danger text-center">Lỗi khi tải thông tin sân.</p>');
                console.error("AJAX Error:", status, error);
                modalTitle.text('Lỗi');
            }
        });
    });

    // Chức năng tim kiếm sân bóng
    $('#searchInput').on('keyup', function () {
        var searchText = $(this).val().toLowerCase();

        $('#court-card-container .row.g-3 > div').each(function () {
            var currentCard = $(this);

            var courtName = currentCard.find('.card-title').text().toLowerCase();

            if (courtName.includes(searchText)) {
                currentCard.show();
            } else {
                currentCard.hide();
            }
        });
    });

    // Nút xoá nội dung tìm kiếm
    $('.search-bar').on('click', '.clear-search-btn', function () {
        var searchInput = $(this).siblings('.form-control');
        searchInput.val('');
        searchInput.focus();
        searchInput.trigger('keyup');
    });

    //Icon tìm kiếm    
    $('.search-bar').on('click', '.search-icon-btn', function () {
        var searchInput = $(this).siblings('.form-control');
        searchInput.focus();
    });

    // Nút yêu thích
    $('.container').on('click', '.fav-btn', function (e) {
        e.preventDefault();
        var $button = $(this);
        $button.toggleClass('favorited');
        let courtName = $button.closest('.card').find('h6').first().text();
        if ($button.hasClass('favorited')) {
            $button.find('.fas.fa-heart').show();
            $button.find('.far.fa-heart').hide();
            alert('Đã thêm "' + courtName + '" vào danh sách yêu thích!');
        } else {
            $button.find('.fas.fa-heart').hide();
            $button.find('.far.fa-heart').show();
            alert('Đã xóa "' + courtName + '" khỏi danh sách yêu thích.');
        }
    });

    // Nút dẫn đường
    $('.container').on('click', '.dir-btn', function (e) {
        e.preventDefault();
        let courtName = $(this).closest('.card').find('h6').first().text();
        alert('Bắt đầu dẫn đường đến "' + courtName + '"...');
    });

});