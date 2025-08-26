using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using UserService.Application.Commands;
using UserService.Application.DTOs;
using UserService.Domain.Entities;
using UserService.Domain.Interfaces;

namespace UserService.Application.Handlers;

public class CreateUserHandler(
    IUserRepository userRepository,
    IMapper mapper,
    IValidator<CreateUserCommand> validator,
    IPasswordHasher<User> passwordHasher,
    ILogger<CreateUserHandler> logger)
    : IRequestHandler<CreateUserCommand, UserDto>
{
    public async Task<UserDto> Handle(CreateUserCommand req, CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating user with email: {Email}", req.Email);

        var validationResult = await validator.ValidateAsync(req, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var existingUser = await userRepository.GetByEmailAsync(req.Email);
        if (existingUser != null)
            throw new InvalidOperationException($"User with email {req.Email} already exists");

        var user = new User
        {
            FirstName = req.FirstName,
            LastName = req.LastName,
            Email = req.Email,
            PhoneNumber = req.PhoneNumber,
            IsActive = true
        };

        user.PasswordHash = passwordHasher.HashPassword(user, req.Password);

        await userRepository.CreateAsync(user);
        await userRepository.SaveChangesAsync();

        logger.LogInformation("User created successfully with ID: {UserId}", user.Id);

        return mapper.Map<UserDto>(user);
    }
}