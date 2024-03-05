using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Npgsql;
using WebApplication_CalendarSchedule.Models;

namespace WebApplication_CalendarSchedule.Data
{
	public class AppDbContext : DbContext
	{
		private readonly IConfiguration _conf;


		public AppDbContext(DbContextOptions<AppDbContext> options, IConfiguration configuration)
			: base(options)
		{
			_conf = configuration;
		}

		public DbSet<Event> Events { get; set; }

		public NpgsqlConnection GetConnection() 
		{
			var connectionString = _conf.GetConnectionString("DefaultConnection");
			var connection = new NpgsqlConnection(connectionString);
			connection.Open();
			return connection;
		}
	}
}

