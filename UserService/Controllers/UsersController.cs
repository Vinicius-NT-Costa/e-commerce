using Common.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserService.Application.Commands;
using UserService.Application.DTOs;
using UserService.Application.Queries;

namespace UserService.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
public class UsersController(
    ILogger<UsersController> logger, IMediator mediator
) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType<ApiResponse<UserDto>>(201)]
    [ProducesResponseType<ApiResponse<object>>(400)]
    public async Task<ActionResult<ApiResponse<UserDto>>> CreateUser([FromBody] CreateUserCommand command)
    {
        try
        {
            var user = await mediator.Send(command);
            var response = ApiResponse<UserDto>.Success(user, "User created successfully");

            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, response);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating user");
            var errorResponse = ApiResponse<object>.Error("Failed to create user", ex.Message);
            return BadRequest(errorResponse);
        }
    }

    //[Authorize]
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<UserDto>), 200)]
    [ProducesResponseType(typeof(ApiResponse<object>), 404)]
    public async Task<ActionResult<ApiResponse<UserDto>>> GetUser(Guid id)
    {
        try
        {
            var query = new GetUserQuery { Id = id };
            var user = await mediator.Send(query);

            if (user == null)
            {
                var notFoundResponse = ApiResponse<object>.Error("User not found");
                return NotFound(notFoundResponse);
            }

            var response = ApiResponse<UserDto>.Success(user);
            return Ok(response);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting user with ID: {UserId}", id);
            var errorResponse = ApiResponse<object>.Error("Failed to get user", ex.Message);
            return BadRequest(errorResponse);
        }
    }
}