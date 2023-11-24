using Project_19.Models;

namespace Project_19.Services;

/// <summary>
/// Тестовый инициализируемый сервис элементов типа <see cref="Contact"/>
/// </summary>
public class TestPhonebook : TestContacts, 
	IInitializator<IEnumerable<Contact>>
{
	#region IInitializator

	/// <inheritdoc/>
	/// <param name="contacts">Коллекция элементов типа <see cref="Contact"/></param>
	/// <param name="log"><inheritdoc/></param>
	public void Initialize(IEnumerable<Contact> contacts, ILogger? log = null)
	{
		string msg;

		if (Contacts.Count > 0)
		{
			msg = $"Contacts in \"{nameof(TestContacts)}\" exist.";
			log?.LogInformation($"{nameof(Initialize)} >>> {msg}");
			return;
		}

		Contacts.AddRange(contacts);
		NextId += Contacts.Count;

		msg = $"Contacts in \"{nameof(TestContacts)}\" initialized.";
		log?.LogInformation($"{nameof(Initialize)} >>> {msg}");
	}

	#endregion

	/// <inheritdoc/>
	public TestPhonebook(int delayMaxMs) : base(delayMaxMs) { }
}
