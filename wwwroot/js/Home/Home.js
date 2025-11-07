$(document).ready(function () {
    // Load icon yêu thích cho thẻ sân đã chọn yêu thích
    $.ajax({
        type: 'GET',
        url: '/Home/GetFavoriteCourtIds',
        success: function (favoriteCourtIds) {
            if (favoriteCourtIds && favoriteCourtIds.length > 0) {
                favoriteCourtIds.forEach(function (courtId) {
                    var $button = $('[data-court-id="' + courtId + '"] .fav-btn');

                    $button.addClass('favorited');
                    $button.find('.fas.fa-heart').show();
                    $button.find('.far.fa-heart').hide();
                });
            }
        },
        error: function () {
            console.error("Không thể tải danh sách yêu thích.");
        }
    });

    // Chi tiết sân bóng trong modal
    $('.container').on('click', '.show-court-details', function (e) {
        // Nếu click vào nút hoặc link bên trong thì bỏ qua
        if ($(e.target).closest('button, a').length) return;

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

                var bookingUrl = '/Court/Booking?courtId=' + courtId; 
                modalBookButton.attr('href', bookingUrl);

                modalBookButton.removeClass('disabled');
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
        $('#searchInput').trigger('focus');
        searchInput.trigger('keyup');
    });

    //Icon tìm kiếm    
    $('.search-bar').on('click', '.search-icon-btn', function () {
        var searchInput = $(this).siblings('.form-control');
        $('#searchInput').trigger('focus');
    });

    // Nút yêu thích
    $('#court-card-container').on('click', '.fav-btn', function (e) {
        e.preventDefault();

        var $button = $(this);
        var courtId = $button.closest('.show-court-details').data('court-id');

        $button.prop('disabled', true);

        $.ajax({
            type: 'POST',
            url: '/Home/ToggleFavorite',
            data: { courtId: courtId },
            success: function (response) {
                if (response.requiresLogin) {
                    window.location.href = response.loginUrl;
                    return;
                }
                if (response.success) {
                    if (response.isFavorited) {
                        $button.addClass('favorited');
                        $button.find('.fas.fa-heart').show();
                        $button.find('.far.fa-heart').hide();
                    } else {
                        $button.removeClass('favorited');
                        $button.find('.fas.fa-heart').hide();
                        $button.find('.far.fa-heart').show();
                    }
                } else {
                    alert('Lỗi: ' + response.error);
                }
                $button.prop('disabled', false); // Kích hoạt lại nút
            },
            error: function () {
                alert('Không thể kết nối đến máy chủ.');
                $button.prop('disabled', false); 
            }
        });
    });

    // Filter Yêu thích
    $('#filter-favorites-btn').on('click', function (e) {
        e.preventDefault();

        var $this = $(this);
        var allCards = $('#court-card-container .row.g-3 > div[class*="col-"]');

        // Xóa text trong thanh tìm kiếm để tránh xung đột filter
        $('#searchInput').val('');

        if ($this.hasClass('active')) {
            $this.removeClass('active');
            allCards.show();
        } else {
            $this.addClass('active');
            allCards.hide();

            $.ajax({
                type: 'GET',
                url: '/Home/GetFavoriteCourtIds',
                success: function (response) {
                    if (response.requiresLogin) {
                        window.location.href = response.loginUrl;
                        return;
                    }
                    var favoriteIds = response;
                    if (favoriteIds && favoriteIds.length > 0) {
                        allCards.each(function () {
                            var currentCard = $(this);
                            var currentCardId = currentCard.data('court-id').toString();

                            if (favoriteIds.includes(currentCardId)) {
                                currentCard.show(); 
                            }
                        });
                    } else {
                        console.log("Bạn chưa yêu thích sân nào.");
                    }
                },
                error: function (response) {
                    alert('Lỗi khi tải danh sách yêu thích. Vui lòng đăng nhập và thử lại.');
                    console.error(response);
                    $this.removeClass('active');
                    allCards.show();
                }
            });
        }
    });

    // Nút dẫn đường
    $('.container').on('click', '.dir-btn', function (e) {
        e.preventDefault();
        let courtName = $(this).closest('.card').find('h6').first().text();
        alert('Bắt đầu dẫn đường đến "' + courtName + '"...');
    });

});