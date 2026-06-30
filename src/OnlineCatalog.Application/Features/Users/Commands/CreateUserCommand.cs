using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using FluentValidation;
using MediatR;
using OnlineCatalog.Application.Common;
using OnlineCatalog.Application.DTOs;
using OnlineCatalog.Domain.Entities;
using OnlineCatalog.Domain.Exceptions;
using OnlineCatalog.Domain.Interfaces.Repositories;

namespace OnlineCatalog.Application.Features.Users.Commands;

public record CreateUserCommand(string Name, string Email, string Password) : IRequest<CreateUserResponse>;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(255);
        RuleFor(x => x.Password).NotEmpty().MinimumLength(8);
    }
}

public class CreateUserCommandHandler(
    IUserRepository userRepository,
    IApiKeyRepository apiKeyRepository,
    IPasswordHasher passwordHasher) : IRequestHandler<CreateUserCommand, CreateUserResponse>
{
    public async Task<CreateUserResponse> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        if (await userRepository.ExistsByEmailAsync(request.Email, cancellationToken))
            throw new ConflictException($"Email '{request.Email}' is already in use.");

        var passwordHash = passwordHasher.Hash(request.Password);
        var user = User.Create(request.Name, request.Email, passwordHash);
        await userRepository.AddAsync(user, cancellationToken);

        var rawKey = GenerateRawKey();
        var keyHash = ComputeSha256(rawKey);
        var apiKey = ApiKey.Create(user.Id, keyHash, "Default");
        await apiKeyRepository.AddAsync(apiKey, cancellationToken);

        return new CreateUserResponse(user.Id, user.Name, user.Email, user.CreatedAt, rawKey);
    }

    private static string GenerateRawKey()
    {
        var bytes = RandomNumberGenerator.GetBytes(32);
        return Convert.ToBase64String(bytes).Replace("+", "").Replace("/", "").Replace("=", "");
    }

    private static string ComputeSha256(string key)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(key));
        return Convert.ToHexString(bytes).ToLowerInvariant();
    }
}
