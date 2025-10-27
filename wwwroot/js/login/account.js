// Ẩn/hiện mật khẩu
document.querySelectorAll('.toggle-password').forEach(btn => {
    btn.addEventListener('click', () => {
        const input = btn.previousElementSibling;
        input.type = input.type === 'password' ? 'text' : 'password';
        btn.classList.toggle('active');
    });
});

// Validate form ForgotPassword
const forgotForm = document.querySelector('#forgot-form');
if (forgotForm) {
    forgotForm.addEventListener('submit', e => {
        const email = forgotForm.querySelector('input[type="email"]').value.trim();
        if (!email.includes('@')) {
            alert("Vui lòng nhập email hợp lệ.");
            e.preventDefault();
        }
    });
}

// Kiểm tra xác nhận mật khẩu (Register)
const registerForm = document.querySelector('#register-form');
if (registerForm) {
    registerForm.addEventListener('submit', e => {
        const pass = registerForm.querySelector('#Password').value;
        const confirm = registerForm.querySelector('#ConfirmPassword').value;
        if (pass !== confirm) {
            alert("Mật khẩu xác nhận không khớp.");
            e.preventDefault();
        }
    });
}
