<script>
    document.addEventListener('DOMContentLoaded', function() {
            // 1. Tìm input
            const avatarInput = document.getElementById('AvatarFileInput');
    // 2. Tìm ảnh để thay
    const avatarImage = document.getElementById('ProfileAvatarImage');

    // 3. Lắng nghe sự kiện "change" (khi chọn file xong)
    avatarInput.addEventListener('change', function() {
                if (avatarInput.files && avatarInput.files[0]) {
                    // 4. Tạo URL tạm thời
                    const newImageUrl = URL.createObjectURL(avatarInput.files[0]);
    // 5. Gán URL đó cho ảnh
    avatarImage.src = newImageUrl;
                }
            });
        });
</script>