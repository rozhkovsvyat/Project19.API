using Project_19.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services
	.AddPhonebook(builder.Configuration) //.AddTestPhonebook()
	.AddPhonebookRandomInitialization() //.AddSimpleInitialization();
	.AddPhonebookIdentity(builder.Configuration)
	.AddPhonebookIdentityBagInitialization(builder.Configuration);
	//.AddPhonebookIdentitySimpleInitialization()

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{		
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();	
app.MapHealthChecks("/healthz");

app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope()
	.InitializePhonebook(app.Logger).InitializeIdentity(app.Logger);

app.Run();
