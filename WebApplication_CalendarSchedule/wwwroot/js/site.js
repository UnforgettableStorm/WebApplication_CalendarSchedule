var currentEventId;
var createEventModal = new bootstrap.Modal(document.getElementById('createEventModal'));
var editDeleteEventModal = new bootstrap.Modal(document.getElementById('editDeleteEventModal'));

document.addEventListener('DOMContentLoaded', function () {

	var calendarEl = document.getElementById('calendar');
	var calendar = new FullCalendar.Calendar(calendarEl, {
		initialView: 'dayGridMonth',
		eventClick: function (info) {
			openEventModal(info);
		}
	});


	//загрузка ивентов в календарь
	function loadEvents() {
		fetch('/api/events')
			.then(response => response.json())
			.then(data => {

				calendar.getEvents().forEach(event => event.remove());

				data.forEach(event => {
					calendar.addEvent({
						id: event.id,
						title: event.title,
						description: event.description,
						start: event.start,
						end: event.end,
						typeName: event.eventType.typeName,
						backgroundColor: event.eventType.themeColor,
						allDay: event.isFullDay
					});
				});
			})
			.catch(error => console.error('Ошибка загрузки данных:', error));

	}

	//Вызываем модальное окно для создания ивента
	document.getElementById('createEventModalBtn').addEventListener('click', function () {
		createEventModal.show();
	});

	//обработка нажатия кнопки сохранить, созжание ивента
	document.getElementById('saveCreateEventBtn').addEventListener('click', function () {
		var startDateTime = document.getElementById('startDate').value + 'T' + document.getElementById('startTime').value;
		var endDateTime = document.getElementById('endDate').value + 'T' + document.getElementById('endTime').value;
		
		var eventData = {
			Title: document.getElementById('eventTitle').value,
			Description: document.getElementById('eventDescription').value,
			Start: startDateTime,
			End: endDateTime,
			EventType: {
				TypeName: document.getElementById('eventTypeName').value
			},
			IsFullDay: document.getElementById('fullDaySwitch').checked
		};

		fetch('/api/Events', {
			method: 'POST',
			headers: {
				'Content-Type': 'application/json'
			},
			body: JSON.stringify(eventData)
		})
			.then(response => {
				if (response.ok) {
					loadEvents(); 
					createEventModal.hide();
				}
			})
			.catch(error => console.error('Create ERROR', error));
	});

	//функция для открытия 
	function openEventModal(info) {
		editDeleteEventModal.show();


		// получаем значения даты
		var startDate = new Date(info.event.start);
		var endDate = new Date(info.event.end);

		// форматирование даты и времени под формочку
		var startDateFormatted = formatDate(startDate);
		var startTimeFormatted = formatTime(startDate);
		var endDateFormatted = formatDate(endDate);
		var endTimeFormatted = formatTime(endDate);

		//заполняем формочку данными из info
		document.getElementById('eventTitleEdDel').value = info.event.title;
		document.getElementById('eventDescriptionEdDel').value = info.event.extendedProps.description;
		document.getElementById('startDateEdDel').value = startDateFormatted;
		document.getElementById('startTimeEdDel').value = startTimeFormatted;
		document.getElementById('endDateEdDel').value = endDateFormatted;
		document.getElementById('endTimeEdDel').value = endTimeFormatted;
		document.getElementById('eventTypeEdDel').value = info.event.extendedProps.typeName;
		document.getElementById('fullDaySwitchEdDel').checked = info.event.allday;
		currentEventId = info.event.id; //здесь запоминаем текущий айди
	}
	//функции для форматирования------
	function formatDate(date) {
		var year = date.getFullYear();
		var month = (date.getMonth() + 1).toString().padStart(2, '0');
		var day = date.getDate().toString().padStart(2, '0');
		return `${year}-${month}-${day}`;
	}

	function formatTime(date) {
		var hours = date.getHours().toString().padStart(2, '0');
		var minutes = date.getMinutes().toString().padStart(2, '0');
		return `${hours}:${minutes}`;
	}
	//----------

	document.getElementById('saveEditEventBtn').addEventListener('click', function () {
		var startDateTime = document.getElementById('startDateEdDel').value + 'T' + document.getElementById('startTimeEdDel').value; //преобразуем datetime в формат, принимаемый БД
		var endDateTime = document.getElementById('endDateEdDel').value + 'T' + document.getElementById('endTimeEdDel').value;

		var eventId = currentEventId; //тут айди присваиваем ранее запомненному для корректного запроса PUT/DELETE
		if (!eventId) {
			console.error('Event ID is not defined');
			return;
		}

		var eventData = {
			Title: document.getElementById('eventTitleEdDel').value,
			Description: document.getElementById('eventDescriptionEdDel').value,
			Start: startDateTime,
			End: endDateTime,
			EventType: {
				TypeName: document.getElementById('eventTypeEdDel').value
			},
			IsFullDay: document.getElementById('fullDaySwitchEdDel').checked
		};

		fetch('/api/Events/' + eventId, { // Добавление id в URL запроса
			method: 'PUT',
			headers: {
				'Content-Type': 'application/json'
			},
			body: JSON.stringify(eventData)
		})
			.then(response => {
				if (response.ok) {
					loadEvents(); 
					editDeleteEventModal.hide(); 
				}
			})
			.catch(error => console.error('Update ERROR:', error));
	});

	// Удаление события
	document.getElementById('deleteEventBtn').addEventListener('click', function () {
		var eventId = currentEventId; 
		if (!eventId) {
			console.error('Event ID is not defined');
			return;
		}
		fetch('/api/Events/' + eventId, {
			method: 'DELETE'
		})
			.then(response => {
				if (response.ok) {
					loadEvents();
					editDeleteEventModal.hide(); 
				}
			})
			.catch(error => console.error('Delete error:', error));
	});

	//Очистка окон
	$('#createEventModal').on('hidden.bs.modal', function () {
	
		document.getElementById('eventTitle').value = '';
		document.getElementById('eventDescription').value = '';
		document.getElementById('startDate').value = '';
		document.getElementById('startTime').value = '';
		document.getElementById('endDate').value = '';
		document.getElementById('endTime').value = '';
		document.getElementById('eventTypeName').value = '';
		document.getElementById('fullDaySwitch').checked = false;
	});

	
	$('#editDeleteEventModal').on('hidden.bs.modal', function () {
		
		document.getElementById('eventTitleEdDel').value = '';
		document.getElementById('eventDescriptionEdDel').value = '';
		document.getElementById('startDateEdDel').value = '';
		document.getElementById('startTimeEdDel').value = '';
		document.getElementById('endDateEdDel').value = '';
		document.getElementById('endTimeEdDel').value = '';
		document.getElementById('eventTypeEdDel').value = '';
		document.getElementById('fullDaySwitchEdDel').checked = false;
	});

	/*//Простенькая обработка свича fullday
	document.getElementById('fullDaySwitch').addEventListener('change', function () {
		var endTimeInput = document.getElementById('endTime');
		var endDateInput = document.getElementById('endDate');
		if (this.checked) {
			endTimeInput.disabled = true;
			endDateInput.disabled = true;
			endTimeInput.value = '00:00';
			endDateInput.value = '1970-01-01'
			
		} else {
			endTimeInput.disabled = false;
			endDateInput.disabled = false;
			
		}
	});
	*/
	loadEvents();
	calendar.render();
});