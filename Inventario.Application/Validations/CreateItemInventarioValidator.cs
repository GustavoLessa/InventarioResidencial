using FluentValidation;
using Inventario.Application.DTOs;

namespace Inventario.Application.Validations;

public class CreateItemInventarioValidator : AbstractValidator<CreateItemInventarioDTO>
{
    public CreateItemInventarioValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("O nome do item é obrigatório.")
            .MinimumLength(3).WithMessage("O nome deve ter pelo menos 3 caracteres.")
            .MaximumLength(100).WithMessage("O nome não pode exceder 100 caracteres.");

        RuleFor(x => x.ValorCompra)
            .GreaterThan(0).WithMessage("O valor de compra deve ser maior que zero.");

        RuleFor(x => x.CategoriaId)
            .NotEmpty().WithMessage("A categoria é obrigatória.");

        RuleFor(x => x.LocalId)
            .NotEmpty().WithMessage("O local de armazenamento é obrigatório.");

        RuleFor(x => x.DataAquisicao)
            .LessThanOrEqualTo(DateTime.Now).WithMessage("A data de aquisição não pode ser no futuro.");
    }
}