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

    // Sidebar toggle and responsive behavior
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

    //Leaflet map
    var map = L.map('map').setView([51.505, -0.09], 13);
    var locateBtn = document.getElementById('locate-btn');


    L.tileLayer('https://tile.openstreetmap.org/{z}/{x}/{y}.png', {
        maxZoom: 19,
        attribution: '&copy; <a href="http://www.openstreetmap.org/copyright">OpenStreetMap</a>'
    }).addTo(map);

    // Khi tìm thấy vị trí
    map.on('locationfound', function (e) {
        var radius = e.accuracy / 2;

        L.marker(e.latlng).addTo(map)
            .bindPopup("Bạn đang ở đây.").openPopup();

        L.circle(e.latlng, radius).addTo(map);
    });

    // Khi không thể lấy vị trí
    map.on('locationerror', function (e) {
        alert("Không thể xác định vị trí: " + e.message);
    });

    locateBtn.addEventListener('click', function () {
        map.locate({ setView: true, maxZoom: 16 });
        console.log("click button triggered");
        L.marker(e.latlng).addTo(map)
            .bindPopup("Bạn đang ở vị trí này").openPopup();
    });
});

