
const timeSlots = [];
var selectedSlots = [];

for (let h = 6; h < 24; h++) {
	timeSlots.push(`${h.toString().padStart(2, "0")}:00`);
	timeSlots.push(`${h.toString().padStart(2, "0")}:30`);
}
timeSlots.push("24:00");

window.addEventListener('DOMContentLoaded', () => {
	const schedule = document.getElementById('schedule');
	schedule.value = new Date().toISOString().split('T')[0];
	loadSlots(schedule.value);
});

schedule.addEventListener("change", () => {
	selectedSlots = [];
	loadSlots(schedule.value);
})

function loadSlots(date) {
	fetch(`/Court/GetSlotsDetails/${modelCourtID}?date=${date}`)
		.then(res => {
			if (!res.ok) throw new Error("Failed to load slot data");
			else {
				console.log("data fetched successfully with courtId:" + modelCourtID + ", Date: " + date);
				return res.json();
			}

		})
		.then(data => renderTimeline(data))
		.catch(err => console.error(err));
}

function renderTimeline(data) {
	document.getElementById('courtName').textContent = data.courtName;

	timelineBody.innerHTML = "";


	data.slots.forEach(slot => {
		const row = document.createElement("div");
		row.className = "timeline-row";

		// slot name column
		const nameCell = document.createElement("div");
		nameCell.textContent = slot.slotName;
		nameCell.className = "court-name";
		row.appendChild(nameCell);

		// Split time line into grid
		for (let i = 0; i < timeSlots.length - 1; i++) {
			const timePack = document.createElement("div");
			const start = timeSlots[i];
			const end = timeSlots[i + 1];

			timePack.dataset.CourtID = modelCourtID;
			timePack.dataset.SlotId = slot.slotId;
			timePack.dataset.Start = start;
			timePack.dataset.End = end;

			// Check slot status with booking schedules
			const booked = slot.bookings.find(b =>
				b.startTime <= start && b.endTime > start);
			var tempStartTime = new Date(`${schedule.value}T${start}:00`)
			if (booked) {
				timePack.className = "slot busy";
			}
			else if (tempStartTime <= new Date()) { // not handle locking slots out of court's open time
				timePack.className = "slot locked";
			}
			else {
				timePack.className = "slot available-slot";
			}

			row.appendChild(timePack);
		}
		timelineBody.appendChild(row);
	});


	document.querySelectorAll('.available-slot').forEach(item => {
		item.addEventListener('click', () => {
			item.classList.toggle('selected');

			const slotData = {
				slotId: item.dataset.SlotId,
				courtId: item.dataset.CourtID,
				startTime: item.dataset.Start,
				endTime: item.dataset.End
			};

			if (item.classList.contains('selected')) {
				selectedSlots.push(slotData);
				console.log("selected an other slot");
			} else {
				const idx = selectedSlots.findIndex(s => s.slotId === slotData.slotId);
				if (idx !== -1) selectedSlots.splice(idx, 1);
			}
		});
	});
}

document.getElementById("paymentConfirm").addEventListener("submit", function (event ) {
	if (selectedSlots.length != 0) document.getElementById("slotsInput").value = JSON.stringify(selectedSlots);
	else {
		alert("Vui lòng chọn sân trước khi tiếp tục!!");
		event.preventDefault();
		return;
	}
});


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
const bodyWrapper = document.getElementById("timelineBodyWrapper");

// Khi scroll body → scroll header theo
bodyWrapper.addEventListener("scroll", () => {
    headerWrapper.scrollLeft = bodyWrapper.scrollLeft;
});