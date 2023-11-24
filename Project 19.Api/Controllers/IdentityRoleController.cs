using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

using Project_19.Models;

namespace Project_19.Controllers;

/// <summary>
/// Api-контроллер работы с коллекцией <see cref="Role"/>
/// </summary>
[ApiController, Route("[controller]")]
[Authorize(Roles = "admin")]
public class IdentityRoleController : ControllerBase
{
	/// <inheritdoc cref="IIdentity"/>
	private readonly IIdentity _identity;

	/// <summary>
	/// Конструктор
	/// </summary>
	/// <param name="identity">Объект, содержащий методы и свойства идентификации</param>
	public IdentityRoleController(IIdentity identity) => _identity = identity;

	/// <inheritdoc cref="IIdentity.GetRolesAsync()"/>
	[HttpGet]
    public async Task<IActionResult> Get() => Ok(await _identity.GetRolesAsync());

	/// <inheritdoc cref="IIdentity.GetRoleByIdAsync"/>
	[HttpGet("{id}")]
    public async Task<IActionResult> Get(string id) => Ok(await _identity.GetRoleByIdAsync(id));

	/// <summary>
	/// <inheritdoc cref="IIdentity.AddRoleAsync"/>
	/// </summary>
	/// <returns><see cref="HttpStatusCode.Conflict"/> и <see cref="HttpStatusCode.OK"/></returns>
	[HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateRoleForm role)
	{
		try { await _identity.AddRoleAsync(role); return Ok(); }

		catch (Exception) {return Conflict(); }
	}

	/// <summary>
	/// <inheritdoc cref="IIdentity.UpdateRoleAsync"/>
	/// </summary>
	/// <returns><see cref="HttpStatusCode.NotFound"/>, <see cref="HttpStatusCode.InternalServerError"/> и <see cref="HttpStatusCode.OK"/></returns>
	[HttpPut("{id}")]
    public async Task<IActionResult> Put(string id, [FromBody] Role role)
	{
		if (id != role.Id) return NotFound();

		try { await _identity.UpdateRoleAsync(role); return Ok(); }

		catch (KeyNotFoundException) { return NotFound(); }

		catch(Exception) { return StatusCode(500); }
	}

	/// <summary>
	/// <inheritdoc cref="IIdentity.RemoveRoleByIdAsync"/>
	/// </summary>
	/// <returns><see cref="HttpStatusCode.NotFound"/>, <see cref="HttpStatusCode.InternalServerError"/> и <see cref="HttpStatusCode.OK"/></returns>
	[HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
	{
		try { await _identity.RemoveRoleByIdAsync(id); return Ok(); }

		catch (KeyNotFoundException) { return NotFound(); }

		catch (Exception) { return StatusCode(500); }
	}

	#region Additional

	/// <inheritdoc cref="IIdentity.GetRoleByNameAsync"/>
	[HttpGet("Name/{name}")]
    public async Task<IActionResult> Name(string name) => Ok(await _identity.GetRoleByNameAsync(name));

	/// <summary>
	/// <inheritdoc cref="IIdentity.RemoveRoleByNameAsync"/>
	/// </summary>
	/// <returns><see cref="HttpStatusCode.NotFound"/>, <see cref="HttpStatusCode.InternalServerError"/> и <see cref="HttpStatusCode.OK"/></returns>
	[HttpDelete("Remove/{name}")]
    public async Task<IActionResult> Remove(string name)
	{
		try { await _identity.RemoveRoleByNameAsync(name); return Ok(); }

		catch (KeyNotFoundException) { return NotFound(); }

		catch (Exception) { return StatusCode(500); }
	}

	#endregion
}
