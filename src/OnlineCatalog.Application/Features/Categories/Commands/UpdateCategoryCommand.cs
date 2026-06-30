using AutoMapper;
using FluentValidation;
using MediatR;
using OnlineCatalog.Application.DTOs;
using OnlineCatalog.Domain.Exceptions;
using OnlineCatalog.Domain.Interfaces.Repositories;

namespace OnlineCatalog.Application.Features.Categories.Commands;

public record UpdateCategoryCommand(Guid Id, string? Name, string? Description) : IRequest<CategoryDto>;

public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
{
    public UpdateCategoryCommandValidator()
    {
        RuleFor(x => x.Name).MaximumLength(100).When(x => x.Name is not null);
        RuleFor(x => x.Description).MaximumLength(500).When(x => x.Description is not null);
    }
}

public class UpdateCategoryCommandHandler(
    ICategoryRepository categoryRepository,
    IMapper mapper) : IRequestHandler<UpdateCategoryCommand, CategoryDto>
{
    public async Task<CategoryDto> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await categoryRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException($"Category '{request.Id}' not found.");

        if (request.Name is not null && request.Name != category.Name)
        {
            if (await categoryRepository.ExistsByNameAsync(request.Name, cancellationToken))
                throw new ConflictException($"Category '{request.Name}' already exists.");
        }

        category.Update(request.Name, request.Description);
        await categoryRepository.UpdateAsync(category, cancellationToken);
        return mapper.Map<CategoryDto>(category);
    }
}
