using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

using Project_19.Models;

namespace Project_19.Controllers;

/// <summary>
/// Api-контроллер работы с коллекцией <see cref="Contact"/>
/// </summary>
[ApiController, Route("[controller]")]
[Authorize]
public class ContactsController : ControllerBase
{
	/// <inheritdoc cref="IContacts"/>
	private readonly IContacts _contacts;

	/// <summary>
	/// Конструктор
	/// </summary>
	/// <param name="contacts">Объект, содержащий коллекцию элементов типа <see cref="Contact"/></param>
	public ContactsController(IContacts contacts) => _contacts = contacts;

	#region [AllowAnonymous]

	/// <inheritdoc cref="IContacts.GetAsync()"/>
	[HttpGet]
	[AllowAnonymous]
	public async Task<IActionResult> Get() => Ok(await _contacts.GetAsync());

	/// <summary>
	/// Отвечает на проверку работоспособности
	/// </summary>
	/// <returns><see cref="HttpStatusCode.OK"/></returns>
	[HttpGet("Ping")]
	[AllowAnonymous]
	public IActionResult Ping() => Ok();

	#endregion

	#region [Authorize]

	/// <inheritdoc cref="IContacts.GetAsync(int)"/>
	[HttpGet("{id}")]
	public async Task<IActionResult> Get(int id) => Ok(await _contacts.GetAsync(id));

	/// <inheritdoc cref="IContacts.AddAsync"/>
	[HttpPost]
	public async Task<IActionResult> Post([FromBody] Contact contact)
	{
		await _contacts.AddAsync(contact);
		return Ok();
	}

	#endregion

	#region [Authorize(Roles = "admin")]

	/// <summary>
	/// <inheritdoc cref="IContacts.UpdateAsync"/>
	/// </summary>
	/// <returns><see cref="HttpStatusCode.NotFound"/>, <see cref="HttpStatusCode.InternalServerError"/> и <see cref="HttpStatusCode.OK"/></returns>
	[HttpPut("{id}")]
	[Authorize(Roles = "admin")]
	public async Task<IActionResult> Put(int id, [FromBody] Contact contact)
	{
		if (id != contact.Id) return NotFound();

		try { await _contacts.UpdateAsync(contact); return Ok(); }

		catch (KeyNotFoundException) { return NotFound(); }

		catch (Exception) { return StatusCode(500); }
	}

	/// <summary>
	/// <inheritdoc cref="IContacts.RemoveAsync"/>
	/// </summary>
	/// <returns><see cref="HttpStatusCode.NotFound"/> и <see cref="HttpStatusCode.OK"/></returns>
	[HttpDelete("{id}")]
	[Authorize(Roles = "admin")]
	public async Task<IActionResult> Delete(int id)
	{
		try { await _contacts.RemoveAsync(id); return Ok(); }

		catch (Exception) { return NotFound(); }
	}

	#endregion
}
