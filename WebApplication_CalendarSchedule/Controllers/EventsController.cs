using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System.Collections.Generic;
using System;
using System.Linq;
using WebApplication_CalendarSchedule.Data;
using WebApplication_CalendarSchedule.Models;

namespace WebApplication_CalendarSchedule.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class EventsController : ControllerBase
	{
		private readonly AppDbContext _context;

		public EventsController(AppDbContext context)
		{
			_context = context;
		}

		[HttpGet]
		public IActionResult GetEvents()
		{
			List<Event> events = new List<Event>();


			string connectionString = "Host=localhost;Port=5432;Database=calendarscheduledb;Username=postgres;Password=admin";

			//Запрос к БД
			using (var connection = new NpgsqlConnection(connectionString))
			{
				connection.Open();

				using (var command = new NpgsqlCommand("SELECT * FROM getevents()", connection))
				{
					using (var reader = command.ExecuteReader())
					{
						while (reader.Read())
						{

							var evt = new Event
							{
								Id = Convert.ToInt32(reader["eventid"]),
								Title = reader["eventname"].ToString(),
								Description = reader["eventdescription"].ToString(),
								Start = Convert.ToDateTime(reader["eventstart"]),
								End = Convert.ToDateTime(reader["eventend"]),
								IsFullDay = Convert.ToBoolean(reader["isfullday"]),

								EventType = new EventType
								{
									TypeName = reader["eventtypename"].ToString(),
									ThemeColor = reader["themecolor"].ToString()
								}
							};
							events.Add(evt);
						}
					}
				}
			}

			return Ok(events);
		}
	}
}
