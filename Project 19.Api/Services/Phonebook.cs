using Microsoft.EntityFrameworkCore;

using Project_19.Models;

namespace Project_19.Services;

/// <summary>
/// Сервис инициализируемой базы данных элементов типа <see cref="Contact"/>
/// </summary>
public class Phonebook : DbContacts, 
	IInitializator<IEnumerable<Contact>>
{
	#region IInitializator

	/// <inheritdoc/>
	/// <param name="contacts">Коллекция элементов типа <see cref="Contact"/></param>
	/// <param name="log"><inheritdoc/></param>
	public void Initialize(IEnumerable<Contact> contacts, ILogger? log = null)
	{
		string msg;

		if (!Context.Database.CanConnect())
		{
			msg = $"\"{nameof(Phonebook)}\" connection failed.";
			log?.LogError($"{nameof(Initialize)} >>> {msg}");
			return;
		}

		if (Context.Contacts.Any())
		{
			msg = $"{nameof(Context.Contacts)} in \"{nameof(Phonebook)}\" exist.";
			log?.LogInformation($"{nameof(Initialize)} >>> {msg}");
			return;
		}

		using var trans = Context.Database.BeginTransaction();
		try
		{
			Context.Contacts.AddRange(contacts);
			//_context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Contacts ON;");
			Context.SaveChanges();
			//_context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Contacts OFF;");
			Context.Database.ExecuteSqlRaw($"ALTER TABLE \"{nameof(Context.Contacts)}\" " +
			                                $"ALTER COLUMN \"{nameof(Contact.Id)}\" " +
			                                $"RESTART SET START {Context.Contacts.Count() + 1}");

			trans.Commit();

			msg = $"{nameof(Context.Contacts)} in \"{nameof(Phonebook)}\" initialized.";
			log?.LogInformation($"{nameof(Initialize)} >>> {msg}");
		}
		catch (Exception e)
		{
			trans.Rollback();

			msg = $"{nameof(Context.Contacts)} in \"{nameof(Phonebook)}\" not initialized. {e.Message}";
			log?.LogError($"{nameof(Initialize)} >>> {msg}");
		}
	}

	#endregion

	/// <inheritdoc cref="DbContacts(ContactsContext)"/>
	/// <param name="context"></param>
	/// <param name="log">Логгер</param>
	public Phonebook(ContactsContext context, ILogger? log = null) : base(context)
	{
		try { Context.Database.Migrate(); }
		catch (Exception e) { log?.LogError($"{nameof(Phonebook)} >>> {e.Message.ToLower()}"); }
	}
}
