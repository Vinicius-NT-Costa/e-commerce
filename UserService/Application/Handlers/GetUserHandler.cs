using AutoMapper;
using MediatR;
using UserService.Application.DTOs;
using UserService.Application.Queries;
using UserService.Domain.Interfaces;

namespace UserService.Application.Handlers;

public class GetUserHandler(
    IUserRepository userRepository,
    IMapper mapper,
    ILogger<GetUserHandler> logger
) : IRequestHandler<GetUserQuery, UserDto?>
{
    public async Task<UserDto?> Handle(GetUserQuery req, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting user with ID: {UserId}", req.Id);

        var user = await userRepository.GetByIdAsync(req.Id);

        if (user != null) return mapper.Map<UserDto>(user);

        logger.LogWarning("User not found with ID: {UserId}", req.Id);
        return null;
    }
}