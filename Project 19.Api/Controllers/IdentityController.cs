using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

using Project_19.Models;

namespace Project_19.Controllers;

/// <summary>
/// Api-контроллер работы с коллекцией <see cref="Account"/>
/// </summary>
[ApiController, Route("[controller]")]
[Authorize]
public class IdentityController : ControllerBase
{
	/// <inheritdoc cref="IIdentity"/>
	private readonly IIdentity _identity;
		
	/// <summary>
	/// Конструктор
	/// </summary>
	/// <param name="identity">Объект, содержащий методы и свойства идентификации</param>
	public IdentityController(IIdentity identity) => _identity = identity;

	#region [AllowAnonymous]

	/// <summary>
	/// <inheritdoc cref="IIdentity.SignInAsync"/>
	/// </summary>
	/// <param name="login">Логин аккаунта</param>
	/// <param name="form">Форма аккаунта</param>
	/// <returns><see cref="HttpStatusCode.NotFound"/> и <see cref="HttpStatusCode.OK"/></returns>
	[HttpPost("{login}")]
	[AllowAnonymous]
	public async Task<IActionResult> Post(string login, [FromBody] SignInForm form)
	{
		if (login != form.Login) return NotFound();

		try { return Ok(await _identity.SignInAsync(form)); }

		catch (Exception) { return NotFound(); }
	}

	/// <summary>
	/// <inheritdoc cref="IIdentity.AddAsync"/>
	/// </summary>
	/// <param name="form">Форма аккаунта</param>
	/// <returns><see cref="HttpStatusCode.Conflict"/>, <see cref="HttpStatusCode.InternalServerError"/> и  и <see cref="HttpStatusCode.OK"/></returns>
	[HttpPost]
	[AllowAnonymous]
	public async Task<IActionResult> Post([FromBody] CreateAccountForm form)
	{
		if (await _identity.GetByLoginAsync(form.Login) != null) return Conflict();

		try { await _identity.AddAsync(form); return Ok(); }

		catch (Exception) { return StatusCode(500); }
	}

	/// <summary>
	/// <inheritdoc cref="IIdentity.SignOutAsync"/>
	/// </summary>
	/// <returns><see cref="HttpStatusCode.OK"/></returns>
	[HttpPost("LogOut")]
	[AllowAnonymous]
	public async Task<IActionResult> LogOut()
	{
		await _identity.SignOutAsync();
		return Ok();
	}

	/// <summary>
	/// Отвечает на проверку работоспособности
	/// </summary>
	/// <returns><see cref="HttpStatusCode.OK"/></returns>
	[HttpGet("Ping")]
	[AllowAnonymous]
	public IActionResult Ping() => Ok();

	#endregion

	#region [Authorize]

	/// <summary>
	/// <inheritdoc cref="IIdentity.UpdatePasswordAsync"/>
	/// </summary>
	/// <param name="login">Логин</param>
	/// <param name="form">Форма аккаунта</param>
	/// <returns><see cref="HttpStatusCode.NotFound"/>, <see cref="HttpStatusCode.InternalServerError"/> и <see cref="HttpStatusCode.OK"/>/></returns>
	[HttpPut("Password/{login}")]
	public async Task<IActionResult> Password(string login, [FromBody] ChangePasswordForm form)
	{
		if (login != form.Login) return NotFound();

		try { await _identity.UpdatePasswordAsync(form); return Ok(); }

		catch (KeyNotFoundException) { return NotFound(); }

		catch (Exception) { return StatusCode(500); }
	}

	#endregion

	#region [Authorize(Roles = "admin")]

	/// <inheritdoc cref="IIdentity.GetAsync()"/>
	[HttpGet]
	[Authorize(Roles = "admin")]
	public async Task<IActionResult> Get() => Ok(await _identity.GetAsync());

	/// <inheritdoc cref="IIdentity.GetByIdAsync"/>
	[HttpGet("{id}")]
	[Authorize(Roles = "admin")]
	public async Task<IActionResult> Get(string id) => Ok(await _identity.GetByIdAsync(id));

	/// <summary>
	/// <inheritdoc cref="IIdentity.UpdateAsync"/>
	/// </summary>
	/// <returns><see cref="HttpStatusCode.NotFound"/>, <see cref="HttpStatusCode.InternalServerError"/> и <see cref="HttpStatusCode.OK"/></returns>
	[HttpPut("{id}")]
	[Authorize(Roles = "admin")]
	public async Task<IActionResult> Put(string id, [FromBody] Account account)
	{
		if (id != account.Id) return NotFound();

		try { await _identity.UpdateAsync(account); return Ok(); }

		catch (KeyNotFoundException) { return NotFound(); }

		catch (Exception) { return StatusCode(500); }
	}

	/// <summary>
	/// <inheritdoc cref="IIdentity.RemoveByIdAsync"/>
	/// </summary>
	/// <returns><see cref="HttpStatusCode.NotFound"/>, <see cref="HttpStatusCode.InternalServerError"/> и <see cref="HttpStatusCode.OK"/></returns>
	[HttpDelete("{id}")]
	[Authorize(Roles = "admin")]
	public async Task<IActionResult> Delete(string id)
	{
		try { await _identity.RemoveByIdAsync(id); return Ok(); }

		catch (KeyNotFoundException) { return NotFound(); }

		catch (Exception) { return StatusCode(500); }
	}

	#region Additional

	/// <inheritdoc cref="IIdentity.GetByLoginAsync"/>
	[HttpGet("Login/{login}")]
	[Authorize(Roles = "admin")]
	public async Task<IActionResult> Login(string login) => Ok(await _identity.GetByLoginAsync(login));

	/// <summary>
	/// <inheritdoc cref="IIdentity.RemoveByLoginAsync"/>
	/// </summary>
	/// <returns><see cref="HttpStatusCode.NotFound"/>, <see cref="HttpStatusCode.InternalServerError"/> и <see cref="HttpStatusCode.OK"/></returns>
	[HttpDelete("Remove/{login}")]
	[Authorize(Roles = "admin")]
	public async Task<IActionResult> Remove(string login)
	{
		try { await _identity.RemoveByLoginAsync(login); return Ok(); }

		catch (KeyNotFoundException) { return NotFound(); }

		catch (Exception) { return StatusCode(500); }
	}

	/// <summary>
	/// <inheritdoc cref="IIdentity.GetAvailableRolesAsync(string)"/>
	/// </summary>
	/// <param name="login">Логин</param>
	/// <returns>Коллекция элементов типа <see cref="Role"/> (может быть пустой)</returns>
	[HttpGet("Exclude/{login}")]
	[Authorize(Roles = "admin")]
	public async Task<IActionResult> Exclude(string login)
	{
		try { return Ok(await _identity.GetAvailableRolesAsync(login));  }

		catch (Exception) { return Ok(Enumerable.Empty<Role>()); }
	}

	/// <summary>
	/// <inheritdoc cref="IIdentity.GetRolesAsync(string)"/>
	/// </summary>
	/// <param name="login">Логин</param>
	/// <returns>Коллекция элементов типа <see cref="Role"/> (может быть пустой)</returns>
	[HttpGet("Include/{login}")]
	[Authorize(Roles = "admin")]
	public async Task<IActionResult> Include(string login)
	{
		try { return Ok(await _identity.GetRolesAsync(login)); }

		catch (Exception) { return Ok(Enumerable.Empty<Role>()); }
	}

	/// <summary>
	/// <inheritdoc cref="IIdentity.AddToRoleAsync"/>
	/// </summary>
	/// <param name="login">Логин</param>
	/// <param name="roleName">Название роли</param>
	/// <returns><see cref="HttpStatusCode.NotFound"/>, <see cref="HttpStatusCode.InternalServerError"/> и <see cref="HttpStatusCode.OK"/></returns>
	[HttpPut("Include/{login}")]
	[Authorize(Roles = "admin")]
	public async Task<IActionResult> Include(string login, [FromBody] string roleName)
	{
		try { await _identity.AddToRoleAsync(login, roleName); return Ok(); }

		catch (KeyNotFoundException) { return NotFound(); }

		catch (Exception) { return StatusCode(500); }
	}

	/// <summary>
	/// <inheritdoc cref="IIdentity.AddToRolesAsync"/>
	/// </summary>
	/// <param name="login">Логин</param>
	/// <param name="roleNames">Названия ролей</param>
	/// <returns><see cref="HttpStatusCode.NotFound"/>, <see cref="HttpStatusCode.InternalServerError"/> и <see cref="HttpStatusCode.OK"/></returns>
	[HttpPut("IncludeAll/{login}")]
	[Authorize(Roles = "admin")]
	public async Task<IActionResult> IncludeAll(string login, [FromBody] IEnumerable<string> roleNames)
	{
		try { await _identity.AddToRolesAsync(login, roleNames); return Ok(); }

		catch (KeyNotFoundException) { return NotFound(); }

		catch (Exception) { return StatusCode(500); }
	}

	/// <summary>
	/// <inheritdoc cref="IIdentity.RemoveFromRoleAsync"/>
	/// </summary>
	/// <param name="login">Логин</param>
	/// <param name="roleName">Название роли</param>
	/// <returns><see cref="HttpStatusCode.NotFound"/>, <see cref="HttpStatusCode.InternalServerError"/> и <see cref="HttpStatusCode.OK"/></returns>
	[HttpPut("Exclude/{login}")]
	[Authorize(Roles = "admin")]
	public async Task<IActionResult> Exclude(string login, [FromBody] string roleName)
	{
		try { await _identity.RemoveFromRoleAsync(login, roleName); return Ok(); }

		catch (KeyNotFoundException) { return NotFound(); }

		catch (Exception) { return StatusCode(500); }
	}

	/// <summary>
	/// <inheritdoc cref="IIdentity.RemoveFromRolesAsync"/>
	/// </summary>
	/// <param name="login">Логин</param>
	/// <param name="roleNames">Названия ролей</param>
	/// <returns><see cref="HttpStatusCode.NotFound"/>, <see cref="HttpStatusCode.InternalServerError"/> и <see cref="HttpStatusCode.OK"/></returns>
	[HttpPut("ExcludeAll/{login}")]
	[Authorize(Roles = "admin")]
	public async Task<IActionResult> ExcludeAll(string login, [FromBody] IEnumerable<string> roleNames)
	{
		try { await _identity.RemoveFromRolesAsync(login, roleNames); return Ok(); }

		catch (KeyNotFoundException) { return NotFound(); }

		catch (Exception) { return StatusCode(500); }
	}

	#endregion

	#endregion
}
