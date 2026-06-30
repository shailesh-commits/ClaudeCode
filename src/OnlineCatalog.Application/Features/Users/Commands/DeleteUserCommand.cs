using MediatR;
using OnlineCatalog.Application.Common;
using OnlineCatalog.Domain.Exceptions;
using OnlineCatalog.Domain.Interfaces.Repositories;

namespace OnlineCatalog.Application.Features.Users.Commands;

public record DeleteUserCommand(Guid Id) : IRequest;

public class DeleteUserCommandHandler(
    IUserRepository userRepository,
    ICurrentUserService currentUserService) : IRequestHandler<DeleteUserCommand>
{
    public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        if (currentUserService.UserId != request.Id)
            throw new ForbiddenException();

        var user = await userRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException($"User '{request.Id}' not found.");

        await userRepository.DeleteAsync(user, cancellationToken);
    }
}
