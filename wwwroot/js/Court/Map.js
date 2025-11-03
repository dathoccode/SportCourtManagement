document.addEventListener("DOMContentLoaded", () => {
    //top navbar slider
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

    // Responsive sidebar behavior
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
            .bindPopup("Bạn đang ở đây: " + e.latlng).openPopup();
        L.circle(e.latlng, radius).addTo(map);
    });


    // Khi không thể lấy vị trí
    map.on('locationerror', function (e) {
        alert("Không thể xác định vị trí: " + e.message);
    });

    locateBtn.addEventListener('click', function () {
        map.locate({ setView: true, maxZoom: 16 });
    });
    
    // Add layergroup to manage markers
    var markerGroup ={};

    //get sport list
    fetch('Court/GetSportTypes')
    .then(res => res.json())
    .then(data  => {
        data.forEach(type => {
            markerGroup[type.SportId] = L.layerGroup();
        })
    });

    // Add a marker at a every court's location
    fetch('/Court/GetCourts')
    .then(res => res.json())
    .then(data => {
        data.forEach(court => {
            const marker = L.marker([court.latitude, court.longitude])
                .addTo(map)
                .bindPopup(`<b>${court.courtName}</b><br>${court.courtAddress}`);

            marker.on('dbclick', function (e) {
                window.location.href = `/Court/Booking?courtID=${court.courtId}`
            })

            if(markerGroup[court.Sportd])
            {
                markerGroup[court.Sportd].addLayer(marker);
            }
        });
    }).catch(err => console.error('Lỗi khi lấy dữ liệu:', err));

    function toggleMarkers(type){
        if (map.hasLayer(markerGroup[type])) map.removeLayer(markerGroup[type])
        else map.addLayer(markerGroup[type])
    }



    $("#searchInput").on("keyup", function () {
        var keyword = $(this).val().toLowerCase();

        $.ajax({
            url: '/Court/FilterByKeyword',
            type: 'GET',
            data: { keyword: keyword },
            success: function (data) {
                $("#courtList").html(data); // Cập nhật danh sách
            },
            error: function (xhr, status, error) {
                console.error("Lỗi AJAX:", error);
            }
        });
    });

});

