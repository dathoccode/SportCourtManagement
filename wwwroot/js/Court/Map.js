document.addEventListener("DOMContentLoaded", () => {
    const slider = document.querySelector(".slider");
    if (slider) {
        let isDown = false;
        let startX;
        let scrollLeft;

        slider.addEventListener("mousedown", (e) => {
            isDown = true;
            slider.classList.add("cursor-grabbing");
            startX = e.pageX - slider.offsetLeft;
            scrollLeft = slider.scrollLeft;
        });
        slider.addEventListener("mouseleave", () => (isDown = false));
        slider.addEventListener("mouseup", () => (isDown = false));
        slider.addEventListener("mousemove", (e) => {
            if (!isDown) return;
            e.preventDefault();
            const x = e.pageX - slider.offsetLeft;
            const walk = (x - startX) * 2;
            slider.scrollLeft = scrollLeft - walk;
        });
    }

    // Cập nhật chiều cao navbar động
    const sidebar = document.getElementById('sidebar');
    const navbar = document.getElementById('navbar');
    const searchbox = document.getElementById('searchbox');
    const menuBtn = document.getElementById('menu-btn');

    menuBtn.addEventListener('click', () => {
        sidebar.classList.toggle('hidden');
    });

    function largeScreenSidebar() {
        const height = navbar.offsetHeight + 'px';
        sidebar.style.top = height;
        searchbox.style.background = 'white';
        menuBtn.addEventListener('click', changeSearchboxBg);
    }

    function changeSearchboxBg() {
        if (sidebar.classList.contains('hidden')) {
            searchbox.style.background = 'transparent';
        } else {
            searchbox.style.background = 'white';
        }
    }

    window.onresize = function () {
        if (window.innerWidth > 992) largeScreenSidebar();
        else smallScreenSidebar();
    }
    window.onload = window.onresize();
    function smallScreenSidebar() {
        sidebar.style.top = 'auto';
        menuBtn.removeEventListener('click', changeSearchboxBg);
        searchbox.style.background = 'transparent';
        console.log('small screen');
    }

    
});


