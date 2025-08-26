using MediatR;
using UserService.Application.DTOs;

namespace UserService.Application.Commands;

public record CreateUserCommand : 
    AbstractCreateUserDto, IRequest<UserDto>;
