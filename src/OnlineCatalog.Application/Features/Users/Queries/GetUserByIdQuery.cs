using AutoMapper;
using MediatR;
using OnlineCatalog.Application.Common;
using OnlineCatalog.Application.DTOs;
using OnlineCatalog.Domain.Exceptions;
using OnlineCatalog.Domain.Interfaces.Repositories;

namespace OnlineCatalog.Application.Features.Users.Queries;

public record GetUserByIdQuery(Guid Id) : IRequest<UserDto>;

public class GetUserByIdQueryHandler(
    IUserRepository userRepository,
    ICurrentUserService currentUserService,
    IMapper mapper) : IRequestHandler<GetUserByIdQuery, UserDto>
{
    public async Task<UserDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        if (currentUserService.UserId != request.Id)
            throw new ForbiddenException();

        var user = await userRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException($"User '{request.Id}' not found.");

        return mapper.Map<UserDto>(user);
    }
}
