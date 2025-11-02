// Tạo timeline header
const header = document.getElementById("timelineHeader");

const headerName = document.createElement("div");
headerName.textContent = ""; // Ô trống cho cột tên sân
header.appendChild(headerName);

for (let i = 0; i < timeSlots.length; i++) {
    const hourCell = document.createElement("div");
    hourCell.textContent = timeSlots[i];
    hourCell.style.gridColumn = "span 1"; 
    header.appendChild(hourCell);
}

//synchronize header and body timeline when scroll
const headerWrapper = document.getElementById("timelineHeaderWrapper");
console.log("header: " + headerWrapper)
const bodyWrapper = document.getElementById("timelineBodyWrapper");

// Khi scroll body → scroll header theo
bodyWrapper.addEventListener("scroll", () => {
    headerWrapper.scrollLeft = bodyWrapper.scrollLeft;

});