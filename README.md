# WebApplication_CalendarSchedule
**Нефункциональные требования:**
- Одностраничное веб-приложение. Технология ASP.NET Core.
- Хранение данных в СУБД PostgreSQL с использованием хранимых процедур для чтения и модификации данных.
- Пользовательский интерфейс. Технологии: HTML, CSS, JavaScript, Bootstrap, jQuery.

**Функциональные требования**
- Пользователь может просматривать календарь событий на месяц на одной странице.
- В виде всплывающих окон пользователь может добавлять, редактировать, просматривать и удалять события.
- События окрашены в цвет категории.

## Описание работы приложения:
1. Пользователь при открытии приложения видит главную страницу.
![image](https://github.com/UnforgettableStorm/WebApplication_CalendarSchedule/assets/78379528/2af10b0a-63f1-44d6-8320-c792935a3e4c)
2. Он может добавить событие, ввести название, описание, ввести дату и время начала и конца события, или выбрать будет ли оно идти весь день.
![image](https://github.com/UnforgettableStorm/WebApplication_CalendarSchedule/assets/78379528/e1ed9006-6570-4d92-8350-4114b5a73b02)
3. Пользователь может отредактировать, или удалить выбранное событие.
![image](https://github.com/UnforgettableStorm/WebApplication_CalendarSchedule/assets/78379528/9d7878ed-242a-4d9f-9bf8-1247f210cb74)
4. Результат:
![image](https://github.com/UnforgettableStorm/WebApplication_CalendarSchedule/assets/78379528/a8b09cb7-f90d-467b-8031-2d3f40e9dc2b)

### ER диаграмма базы данных (PostgreSQL)
![image](https://github.com/UnforgettableStorm/WebApplication_CalendarSchedule/assets/78379528/e108632b-f96e-463b-9a88-389d241fa9e3)

Для работы с базой данных были созданы 3 хранимые процедуры (insert, delete, update) и 1 функция для выборки данных.
![image](https://github.com/UnforgettableStorm/WebApplication_CalendarSchedule/assets/78379528/5ecab6a6-9472-485c-a3b0-9a3211550612)

Backup базы данных для сервера PostgreSQL16 прилагается.




   

