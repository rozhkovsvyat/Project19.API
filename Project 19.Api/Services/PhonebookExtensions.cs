using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Reflection;
using System.Text.Json;
using System.Text;

using Project_19.Models;

namespace Project_19.Services;

/// <summary>
/// Содержит методы расширения <see cref="Phonebook"/>
/// </summary>
public static class PhonebookExtensions
{
	/// <summary>
	/// Регистрирует <see cref="Phonebook"/> в <see cref="IServiceCollection"/>
	/// </summary>
	/// <param name="collection"></param>
	/// <param name="config"></param>
	/// <exception cref="InvalidOperationException"></exception>
	public static IServiceCollection AddPhonebook(this IServiceCollection collection, IConfiguration config) 
		=> collection.AddNpgsql(config).AddScoped<IContacts, Phonebook>();

	/// <summary>
	/// Регистрирует <see cref="TestPhonebook"/> в <see cref="IServiceCollection"/>
	/// </summary>
	public static IServiceCollection AddTestPhonebook(this IServiceCollection collection)
		=> collection.AddSingleton<IContacts, TestPhonebook>();

	/// <summary>
	/// Регистрирует <see cref="RandomContactsFactory"/> в <see cref="IServiceCollection"/>
	/// </summary>
	public static IServiceCollection AddPhonebookRandomInitialization(this IServiceCollection collection) 
		=> collection.AddTransient<IContactsFactory, RandomContactsFactory>();

	/// <summary>
	/// Регистрирует <see cref="SimpleContactsFactory"/> в <see cref="IServiceCollection"/>
	/// </summary>
	public static IServiceCollection AddPhonebookSimpleInitialization(this IServiceCollection collection)
		=> collection.AddTransient<IContactsFactory, SimpleContactsFactory>();

	/// <summary>
	/// Инициализирует <see cref="Phonebook"/> в <see cref="IServiceScope"/>
	/// используя <see cref="IContactsFactory"/>
	/// </summary>
	/// <exception cref="ArgumentNullException"></exception>
	/// <exception cref="InvalidOperationException"></exception>
	public static IServiceScope InitializePhonebook(this IServiceScope scope, ILogger? log = null)
		=> scope.Initialize<IContacts, IEnumerable<Contact>>(scp =>
			scp.ServiceProvider.GetRequiredService<IContactsFactory>().Get(), log);

	/// <summary>
	/// Регистрирует <see cref="PhonebookIdentity"/> в <see cref="IServiceCollection"/>
	/// </summary>
	/// <param name="collection"></param>
	/// <param name="config"></param>
	/// <exception cref="InvalidOperationException"></exception>
	public static IServiceCollection AddPhonebookIdentity(this IServiceCollection collection, IConfiguration config)
		=> collection.AddMongoIdentity(config).AddJwtAuthentication(config).AddScoped<IIdentity, PhonebookIdentity>();

	/// <summary>
	/// Регистрирует <see cref="BagIdentityFactory"/> в <see cref="IServiceCollection"/>
	/// </summary>
	/// <param name="collection"></param>
	/// <param name="config"></param>
	public static IServiceCollection AddPhonebookIdentityBagInitialization(this IServiceCollection collection, IConfiguration config)
		=> collection.Configure<BagIdentityFactoryOptions>(config.GetSection(nameof(BagIdentityFactory)) ?? 
			throw new InvalidOperationException($"Section {nameof(BagIdentityFactory)} not found."))
				.AddTransient<IIdentityFactory, BagIdentityFactory>();

	/// <summary>
	/// Регистрирует <see cref="SimpleIdentityFactory"/> в <see cref="IServiceCollection"/>
	/// </summary>
	public static IServiceCollection AddPhonebookIdentitySimpleInitialization(this IServiceCollection collection)
		=> collection.AddTransient<IIdentityFactory, SimpleIdentityFactory>();

	/// <summary>
	/// Инициализирует <see cref="PhonebookIdentity"/> в <see cref="IServiceScope"/>
	/// используя <see cref="IIdentityFactory"/>
	/// </summary>
	/// <exception cref="ArgumentNullException"></exception>
	/// <exception cref="InvalidOperationException"></exception>
	public static IServiceScope InitializeIdentity(this IServiceScope scope, ILogger? log = null)
		=> scope.Initialize<IIdentity, IEnumerable<CreateAccountWithRoleForm>>(scp =>
			scp.ServiceProvider.GetRequiredService<IIdentityFactory>().Get(), log);

	/// <summary>
	/// Регистрирует <see cref="DbContext"/> (Npgsql) в <see cref="IServiceCollection"/>
	/// </summary>
	/// <param name="collection"></param>
	/// <param name="config"></param>
	/// <exception cref="InvalidOperationException"></exception>
	private static IServiceCollection AddNpgsql(this IServiceCollection collection, IConfiguration config)
	{
		var conStr = Environment.GetEnvironmentVariable("DB_CONSTR");
		if (string.IsNullOrEmpty(conStr)) conStr = config.GetConnectionString(nameof(ContactsContext));

		collection.AddDbContext<ContactsContext>(options =>
			options.UseNpgsql(conStr ?? throw new InvalidOperationException
				($"Connection string {nameof(ContactsContext)} not found."), 
				o => o.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name)));

		collection.AddHealthChecks().AddDbContextCheck<ContactsContext>
			(nameof(ContactsContext), HealthStatus.Unhealthy, new[] 
				{ nameof(ContactsContext).ToLower(), "db", "pg", "postgre", "postgresql" });

		return collection;
	}

	/// <summary>
	/// Регистрирует и конфигурирует <see cref="IdentityUser"/> (Mongo) в <see cref="IServiceCollection"/>
	/// </summary>
	/// <param name="collection"></param>
	/// <param name="config"></param>
	/// <exception cref="InvalidOperationException"></exception>
	private static IServiceCollection AddMongoIdentity(this IServiceCollection collection, IConfiguration config)
	{
		var conStr = Environment.GetEnvironmentVariable("IDENTITY_CONSTR");

		if (string.IsNullOrEmpty(conStr))
			conStr = config.GetConnectionString(nameof(IdentityUser)) ??
			         throw new InvalidOperationException($"Connection string " +
			                                             $"{nameof(IdentityUser)} not found.");

		collection.AddIdentity<MongoAccount, MongoRole>().AddMongoDbStores
			<MongoAccount, MongoRole, Guid>(conStr, nameof(IdentityUser))
			.AddDefaultTokenProviders();

		collection.AddHealthChecks()
			.AddTypeActivatedCheck<MongoDbHealthCheck>
				(nameof(IdentityUser), HealthStatus.Unhealthy, new[] 
					{ nameof(IdentityUser).ToLower(), "db", "mongo", "mongodb" }, conStr);

		return collection.Configure<IdentityOptions>(options =>
		{
			options.Password.RequiredLength = 6;
			options.Password.RequireDigit = true;
			options.Password.RequireLowercase = true;
			options.Password.RequireNonAlphanumeric = true;
			options.Password.RequireUppercase = true;
			options.Password.RequiredUniqueChars = 1;
			options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
			options.Lockout.MaxFailedAccessAttempts = 10;
			options.Lockout.AllowedForNewUsers = true;
		});
	}

	/// <summary>
	/// Регистрирует и конфигурирует JWT аутентификацию в <see cref="IServiceCollection"/>
	/// </summary>
	/// <param name="collection"></param>
	/// <param name="config"></param>
	/// <exception cref="InvalidOperationException"></exception>
	private static IServiceCollection AddJwtAuthentication(this IServiceCollection collection, IConfiguration config)
	{
		var jwtConfig = config.GetSection(nameof(TokenValidationParameters));

		var issuer = Environment.GetEnvironmentVariable("JWT_ISSUER") is string i && !string.IsNullOrEmpty(i) ? i
			: jwtConfig["Issuer"] is string _i && !string.IsNullOrEmpty(_i) ? _i : throw new InvalidOperationException($"JWT-issuer not found.");

		var audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE") is string a && !string.IsNullOrEmpty(a) ? a
			: jwtConfig["Audience"] is string _a && !string.IsNullOrEmpty(_a) ? _a : throw new InvalidOperationException($"JWT-audience not found.");

		var key = Environment.GetEnvironmentVariable("JWT_KEY") is string k && !string.IsNullOrEmpty(k) ? k
			: jwtConfig["Key"] is string _k && !string.IsNullOrEmpty(_k) ? _k : throw new InvalidOperationException($"JWT-key not found.");

		collection.AddAuthentication(x =>
		{
			x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
			x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
		})
		.AddJwtBearer(x =>
		{
			x.SaveToken = true;
			x.RequireHttpsMetadata = false;
			x.TokenValidationParameters = new TokenValidationParameters
			{
				ValidateIssuerSigningKey = true,
				ValidateIssuer = true,
				ValidateAudience = true,
				ValidateLifetime = true,
				ValidIssuer = issuer,
				ValidAudience = audience,
				IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
				ClockSkew = TimeSpan.Zero
			};
		});

		var json = JsonSerializer.Serialize(new IdentityTokenOptions { Issuer = issuer, Audience = audience, Key = key  });
		config = new ConfigurationBuilder().AddJsonStream(new MemoryStream(Encoding.ASCII.GetBytes(json))).Build();

		return collection.Configure<IdentityTokenOptions>(config);
	}

	/// <summary>
	/// Регистрирует и конфигурирует Cookie-аутентификацию в <see cref="IServiceCollection"/>
	/// </summary>
	/// <param name="collection"></param>
	/// <exception cref="InvalidOperationException"></exception>
	private static IServiceCollection AddCookieAuthentication(this IServiceCollection collection) 
		=> collection.ConfigureApplicationCookie(options => 
		{
			options.Cookie.MaxAge = TimeSpan.FromMinutes(30);
			options.Events.OnRedirectToLogin = ctx => 
			{ 
				ctx.HttpContext.Response.StatusCode = 401; 
				return Task.CompletedTask; 
			};
			options.SlidingExpiration = true;
			options.Cookie.HttpOnly = true;
		});
}
