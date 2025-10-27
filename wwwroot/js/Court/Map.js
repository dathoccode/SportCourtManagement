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

    const menuBtn = document.getElementById('menu-btn');
    const fieldTab = document.getElementById('field-tab');

    if (menuBtn && fieldTab) {
        menuBtn.addEventListener('click', () => {
            fieldTab.classList.toggle('show');
        });
    }
});
