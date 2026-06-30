using AutoMapper;
using MediatR;
using OnlineCatalog.Application.DTOs;
using OnlineCatalog.Domain.Exceptions;
using OnlineCatalog.Domain.Interfaces.Repositories;

namespace OnlineCatalog.Application.Features.Categories.Queries;

public record GetCategoryByIdQuery(Guid Id) : IRequest<CategoryDto>;

public class GetCategoryByIdQueryHandler(
    ICategoryRepository categoryRepository,
    IMapper mapper) : IRequestHandler<GetCategoryByIdQuery, CategoryDto>
{
    public async Task<CategoryDto> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var category = await categoryRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException($"Category '{request.Id}' not found.");

        return mapper.Map<CategoryDto>(category);
    }
}
