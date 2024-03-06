using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System.Collections.Generic;
using System;
using System.Linq;
using WebApplication_CalendarSchedule.Data;
using WebApplication_CalendarSchedule.Models;
using Microsoft.Extensions.Configuration;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Data;

namespace WebApplication_CalendarSchedule.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class EventsController : ControllerBase
	{
		private readonly AppDbContext _context;
		private readonly IConfiguration _conf;

		//Получение данных бд
		public EventsController(AppDbContext context, IConfiguration configuration)
		{
			_context = context;
			_conf = configuration;
		}

		[HttpGet]
		public IActionResult GetEvents()
		{
			List<Event> events = new List<Event>();

			//Запрос к БД
			using (var connection = _context.GetConnection())
			{

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

		//Добавление ивента
		[HttpPost]
		public IActionResult AddEvent(Event evt)
		{
			using (var connection = _context.GetConnection())
			{
				using (var cmd = new NpgsqlCommand(@"CALL insert_event(:i_eventname, :i_eventdescription, :i_eventstart, :i_eventend, :i_eventtypename, :i_isfullday)", connection))
				{
					cmd.CommandType = CommandType.Text;

					cmd.Parameters.AddWithValue("i_eventname", evt.Title);
					cmd.Parameters.AddWithValue("i_eventdescription", evt.Description);
					cmd.Parameters.AddWithValue("i_eventstart", evt.Start);
					cmd.Parameters.AddWithValue("i_eventend", evt.End);
					cmd.Parameters.AddWithValue("i_eventtypename", evt.EventType.TypeName);
					cmd.Parameters.AddWithValue("i_isfullday", evt.IsFullDay);

					cmd.ExecuteNonQuery();
				}

			}

			return Ok();
		}

		//Обновление ивента при редактировании
		[HttpPut("{id}")]
		public IActionResult UpdateEvent(int id, Event evt)
		{
			using (var connection = _context.GetConnection())
			{
				using (var cmd = new NpgsqlCommand(@"CALL update_event(:u_eventid, :u_eventname, :u_eventdescription, :u_eventstart, :u_eventend, :u_eventtypename, :u_isfullday)", connection))
				{
					cmd.CommandType = CommandType.Text;

					cmd.Parameters.AddWithValue("u_eventid", id);
					cmd.Parameters.AddWithValue("u_eventname", evt.Title);
					cmd.Parameters.AddWithValue("u_eventdescription", evt.Description);
					cmd.Parameters.AddWithValue("u_eventstart", evt.Start);
					cmd.Parameters.AddWithValue("u_eventend", evt.End);
					cmd.Parameters.AddWithValue("u_eventtypename", evt.EventType.TypeName);
					cmd.Parameters.AddWithValue("u_isfullday", evt.IsFullDay);

					cmd.ExecuteNonQuery();
				}

			}

			return Ok();
		}

		//Удаление ивента
		[HttpDelete("{id:int}")]
		public IActionResult DeleteEvent(int id)
		{
			using (var connection = _context.GetConnection())
			{
				using (var cmd = new NpgsqlCommand(@"CALL delete_event (:d_eventid)", connection))
				{
					cmd.CommandType = CommandType.Text;

					cmd.Parameters.AddWithValue("d_eventid", id);

					cmd.ExecuteNonQuery();
				}
			}
			return Ok();
		}
	}
}
