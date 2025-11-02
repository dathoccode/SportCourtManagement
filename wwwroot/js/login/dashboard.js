document.addEventListener('DOMContentLoaded', function () {

    // --- 0. HÀM HỖ TRỢ (Thêm số 0 vào trước ngày/tháng) ---
    function pad(num) {
        return num < 10 ? '0' + num : num;
    }

    // --- 1. TÌM TẤT CẢ CÁC NÚT VÀ INPUT ---
    const calendarAnchor = document.getElementById('date-filter-anchor'); // Nút "Xem tất cả"
    const btnChonNgay = document.getElementById('btn-chon-ngay');
    const btnChonThang = document.getElementById('btn-chon-thang');
    const btnChonNam = document.getElementById('btn-chon-nam');
    const btnXemTatCa = document.getElementById('btn-xem-tat-ca'); // Nút reset

    const datePickerInput = document.getElementById('date-range-picker');
    const monthPickerInput = document.getElementById('month-picker');
    const yearPickerInput = document.getElementById('year-picker');

    // --- 2. CẤU HÌNH CÁC LỊCH ---

    // LỊCH CHỌN KHOẢNG NGÀY
    const datePicker = flatpickr(datePickerInput, {
        mode: "range",
        dateFormat: "d-m-Y",
        appendTo: calendarAnchor.parentNode,
        onClose: function (selectedDates) {
            // Khi người dùng bấm "OK" và đã CHỌN ĐỦ 2 ngày
            if (selectedDates.length === 2) {
                const start = selectedDates[0];
                const end = selectedDates[1];

                // Format: "08/11" (giống ảnh của bạn)
                const startDateStr = pad(start.getDate()) + '/' + pad(start.getMonth() + 1);
                const endDateStr = pad(end.getDate()) + '/' + pad(end.getMonth() + 1);

                // Gán vào nút
                calendarAnchor.innerHTML = startDateStr + ' - ' + endDateStr + ' <i class="fas fa-calendar-alt ms-1"></i>';
            }
        }
    });

    // LỊCH CHỌN THÁNG
    const monthPicker = flatpickr(monthPickerInput, {
        plugins: [new monthSelectPlugin({ shorthand: true, dateFormat: "m-Y", altFormat: "F Y" })],
        appendTo: calendarAnchor.parentNode,
        onClose: function (selectedDates, dateStr) {
            // dateStr sẽ là "11-2025"
            if (dateStr) {
                calendarAnchor.innerHTML = 'Tháng ' + dateStr.replace('-', '/') + ' <i class="fas fa-calendar-alt ms-1"></i>';
            }
        }
    });

    // LỊCH CHỌN NĂM
    const yearPicker = flatpickr(yearPickerInput, {
        plugins: [new monthSelectPlugin({ dateFormat: "Y" })],
        appendTo: calendarAnchor.parentNode,

        // onReady chỉ chạy 1 lần, chúng ta dùng onOpen chạy MỖI LẦN
        onOpen: function (selectedDates, dateStr, instance) {
            // 1. Thêm class CSS (như cũ)
            instance.calendarContainer.classList.add('year-picker-container');

            // 2. Tìm ĐÚNG vùng chứa các năm
            const yearsContainer = instance.calendarContainer.querySelector('.flatpickr-monthSelect-years');

            if (yearsContainer) {
                // 3. Gán sự kiện click vào VÙNG CHỨA NĂM
                // Dùng .onclick để đảm bảo nó luôn được gán mới
                yearsContainer.onclick = function (e) {

                    // 4. Kiểm tra xem có bấm vào NÚT NĂM không
                    if (e.target.classList.contains('flatpickr-monthSelect-year')) {

                        // 5. Lấy text của năm
                        const selectedYear = e.target.textContent;

                        // 6. Cập nhật nút "Xem tất cả"
                        if (selectedYear) {
                            calendarAnchor.innerHTML = 'Năm ' + selectedYear + ' <i class="fas fa-calendar-alt ms-1"></i>';
                        }

                        // 7. Đóng lịch
                        instance.close();
                    }
                };
            } else {
                console.error("LỖI: Không tìm thấy '.flatpickr-monthSelect-years'");
            }
        },

        // Chúng ta vẫn cần onClose để xử lý trường hợp
        // người dùng không bấm gì mà lịch tự đóng
        onClose: function (selectedDates, dateStr) {
            // Chỉ cập nhật nếu dateStr có giá trị và nút chưa được cập nhật
            if (dateStr && !calendarAnchor.innerHTML.includes('Năm')) {
                calendarAnchor.innerHTML = 'Năm ' + dateStr + ' <i class="fas fa-calendar-alt ms-1"></i>';
            }
        }
    });

    // --- 3. GÁN SỰ KIỆN CLICK CHO CÁC NÚT ---

    // Hàm helper để lấy và ẩn dropdown
    function getDropdown() {
        return bootstrap.Dropdown.getInstance(calendarAnchor);
    }

    // Bấm "Chọn khoảng ngày"
    btnChonNgay.addEventListener('click', function (e) {
        e.preventDefault();
        getDropdown()?.hide(); // Ẩn dropdown (nếu có)
        datePicker.open();
    });

    // Bấm "Chọn tháng"
    btnChonThang.addEventListener('click', function (e) {
        e.preventDefault();
        getDropdown()?.hide();
        monthPicker.open();
    });

    // Bấm "Chọn năm"
    btnChonNam.addEventListener('click', function (e) {
        e.preventDefault();
        getDropdown()?.hide();
        yearPicker.open();
    });

    // Bấm "Xem tất cả"
    btnXemTatCa.addEventListener('click', function (e) {
        e.preventDefault();
        getDropdown()?.hide();
        // Reset lại chữ trên nút
        calendarAnchor.innerHTML = 'Xem tất cả <i class="fas fa-calendar-alt ms-1"></i>';

        // (Sau này, bạn sẽ thêm logic tải lại toàn bộ lịch ở đây)
    });

});