using AutoMapper;
using FluentValidation;
using MediatR;
using OnlineCatalog.Application.Common;
using OnlineCatalog.Application.DTOs;
using OnlineCatalog.Domain.Exceptions;
using OnlineCatalog.Domain.Interfaces.Repositories;

namespace OnlineCatalog.Application.Features.Users.Commands;

public record UpdateUserCommand(Guid Id, string? Name, string? Email) : IRequest<UserDto>;

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(x => x.Name).MaximumLength(100).When(x => x.Name is not null);
        RuleFor(x => x.Email).EmailAddress().MaximumLength(255).When(x => x.Email is not null);
    }
}

public class UpdateUserCommandHandler(
    IUserRepository userRepository,
    ICurrentUserService currentUserService,
    IMapper mapper) : IRequestHandler<UpdateUserCommand, UserDto>
{
    public async Task<UserDto> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        if (currentUserService.UserId != request.Id)
            throw new ForbiddenException();

        var user = await userRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException($"User '{request.Id}' not found.");

        if (request.Email is not null && request.Email != user.Email)
        {
            if (await userRepository.ExistsByEmailAsync(request.Email, cancellationToken))
                throw new ConflictException($"Email '{request.Email}' is already in use.");
        }

        user.Update(request.Name, request.Email);
        await userRepository.UpdateAsync(user, cancellationToken);
        return mapper.Map<UserDto>(user);
    }
}
