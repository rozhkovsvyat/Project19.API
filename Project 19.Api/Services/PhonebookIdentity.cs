using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

using Project_19.Models;

namespace Project_19.Services;

/// <summary>
/// Содержит свойства и методы инициализируемой идентификации Mongo
/// </summary>
public class PhonebookIdentity : MongoIdentity, 
	IInitializator<IEnumerable<CreateAccountWithRoleForm>>
{
	#region IInitializator

	/// <inheritdoc/>
	public void Initialize(IEnumerable<CreateAccountWithRoleForm> arg, ILogger? log = null)
	{
		log?.LogInformation($"{nameof(Initialize)} >>> Started.");

		Task.Run(() => AddAccounts(arg, log)).Wait();
		Task.Run(() => AddRoles(arg, log)).Wait();
		Task.Run(() => AssignRolesToAccounts(arg, log)).Wait();

		log?.LogInformation($"{nameof(Initialize)} >>> Finished.");
	}

	#endregion

	/// <inheritdoc/>
	public PhonebookIdentity(SignInManager<MongoAccount> signInManager, 
		UserManager<MongoAccount> userManager, RoleManager<MongoRole> roleManager, 
		IOptions<IdentityTokenOptions> tokenOptions) : base(signInManager, userManager, roleManager, tokenOptions) { }

	/// <summary>
	/// Добавляет коллекцию <see cref="Account"/> используя коллекцию <see cref="CreateAccountWithRoleForm"/>
	/// </summary>
	/// <param name="forms">Коллекция <see cref="CreateAccountWithRoleForm"/></param>
	/// <param name="log">Логгер</param>
	private async Task AddAccounts(IEnumerable<CreateAccountWithRoleForm> forms, ILogger? log = null)
	{
		foreach (var form in forms)
		{
			var accountForm = CreateAccountForm.ForLogin(form.Login)
				.ForEmail(form.Email).ForPassword(form.Password);
			try
			{
				await AddAsync(accountForm);
				log?.LogInformation($"{nameof(Initialize)} >>> " +
				                    $"Account \"{accountForm.Login}\" created.");
			}
			catch (InvalidOperationException e)
			{
				foreach (var error in e.Message.Deserialize())
					log?.LogWarning($"{nameof(Initialize)} >>> {error}.");
			}
		}
	}

	/// <summary>
	/// Добавляет коллекцию <see cref="Role"/> используя коллекцию <see cref="CreateAccountWithRoleForm"/>
	/// </summary>
	/// <param name="forms">Коллекция <see cref="CreateAccountWithRoleForm"/></param>
	/// <param name="log">Логгер</param>
	private async Task AddRoles(IEnumerable<CreateAccountWithRoleForm> forms, ILogger? log = null)
	{
		foreach (var form in forms)
		{
			if (string.IsNullOrEmpty(form.RoleName)) continue;
			var roleForm = CreateRoleForm.ForName(form.RoleName);

			try
			{
				await AddRoleAsync(roleForm);
				log?.LogInformation($"{nameof(Initialize)} >>> " +
				                    $"Role \"{roleForm.Name}\" created.");
			}
			catch (InvalidOperationException e)
			{
				foreach (var error in e.Message.Deserialize())
					log?.LogWarning($"{nameof(Initialize)} >>> {error}.");
			}
		}
	}

	/// <summary>
	/// Назначает коллекцию <see cref="Role"/> указанным <see cref="Account"/> используя коллекцию <see cref="CreateAccountWithRoleForm"/>
	/// </summary>
	/// <param name="forms">Коллекция <see cref="CreateAccountWithRoleForm"/></param>
	/// <param name="log">Логгер</param>
	private async Task AssignRolesToAccounts(IEnumerable<CreateAccountWithRoleForm> forms, ILogger? log = null)
	{
		foreach (var form in forms)
		{
			if (string.IsNullOrEmpty(form.RoleName)) continue;

			try
			{
				await AddToRoleAsync(form.Login, form.RoleName);
				log?.LogInformation($"{nameof(Initialize)} >>> " +
				                    $"Role \"{form.RoleName}\" assigned to \"{form.Login}\".");
			}
			catch (InvalidOperationException e)
			{
				foreach (var error in e.Message.Deserialize())
					log?.LogWarning($"{nameof(Initialize)} >>> {error}.");
			}
			catch (KeyNotFoundException)
			{
				log?.LogError($"{nameof(Initialize)} >>> Account not found.");
			}
		}
	}
}
