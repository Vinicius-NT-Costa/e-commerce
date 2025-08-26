using MediatR;
using UserService.Application.DTOs;

namespace UserService.Application.Queries;

public record GetUserQuery : IRequest<UserDto?>
{
    public Guid Id { get; set; }
}