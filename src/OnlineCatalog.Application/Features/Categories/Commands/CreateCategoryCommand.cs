using AutoMapper;
using FluentValidation;
using MediatR;
using OnlineCatalog.Application.DTOs;
using OnlineCatalog.Domain.Entities;
using OnlineCatalog.Domain.Exceptions;
using OnlineCatalog.Domain.Interfaces.Repositories;

namespace OnlineCatalog.Application.Features.Categories.Commands;

public record CreateCategoryCommand(string Name, string? Description) : IRequest<CategoryDto>;

public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Description).MaximumLength(500).When(x => x.Description is not null);
    }
}

public class CreateCategoryCommandHandler(
    ICategoryRepository categoryRepository,
    IMapper mapper) : IRequestHandler<CreateCategoryCommand, CategoryDto>
{
    public async Task<CategoryDto> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        if (await categoryRepository.ExistsByNameAsync(request.Name, cancellationToken))
            throw new ConflictException($"Category '{request.Name}' already exists.");

        var category = Category.Create(request.Name, request.Description);
        await categoryRepository.AddAsync(category, cancellationToken);
        return mapper.Map<CategoryDto>(category);
    }
}
