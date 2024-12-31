using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Library.Application.Validators
{
    public class BookValidator: AbstractValidator<BookDTO>
    {
        public BookValidator()
        {
            RuleFor(b => b.ISBN)
            .NotEmpty().WithMessage("ISBN is required.")
            .Matches(@"^978-\d{1,5}-\d{1,7}-\d{1,7}-\d{1}$").WithMessage("ISBN must be in valid format (e.g. 978-3-16-148410-0).");

            RuleFor(b => b.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(255).WithMessage("Title can't be longer than 255 characters.");

            RuleFor(b => b.Genre)
                .NotEmpty().WithMessage("Genre is required.");

            RuleFor(b => b.Description)
                .MaximumLength(1000).WithMessage("Description can't be longer than 1000 characters.");

            RuleFor(b => b.AuthorId)
                .GreaterThan(0).WithMessage("AuthorId must be greater than 0.");

            RuleFor(b => b.ClientId)
                .NotEmpty().When(b => b.ClientId != null).WithMessage("ClientId can't be empty if provided.");

            RuleFor(b => b)
                .Must(b => b.ClientId == null && !b.TakenTime.HasValue && !b.ReturnBy.HasValue)
                .When(b => b.ClientId == null)
                .WithMessage("If ClientId is null, TakenTime and ReturnBy must also be null.");

            RuleFor(b => b)
                .Must(b => b.ClientId != null && b.TakenTime.HasValue && b.ReturnBy.HasValue)
                .When(b => b.ClientId != null)
                .WithMessage("If ClientId is not null, TakenTime and ReturnBy must not be null.");

            RuleFor(b => b.TakenTime)
                .LessThanOrEqualTo(DateTime.UtcNow).When(b => b.TakenTime.HasValue).WithMessage("TakenTime can't be in the future.");

            RuleFor(b => b.ReturnBy)
                .GreaterThanOrEqualTo(b => b.TakenTime)
                .When(b => b.ReturnBy.HasValue && b.TakenTime.HasValue)
                .WithMessage("ReturnBy must be greater than or equal to TakenTime.");

            RuleFor(b => b.ImageFile)
                .Must(IsValidImage).When(b => b.ImageFile != null).WithMessage("ImageFile must be a valid image (JPG, PNG, JPEG).");

        }
        private bool IsValidImage(IFormFile file)
        {
            var validExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var fileExtension = System.IO.Path.GetExtension(file.FileName)?.ToLower();
            return fileExtension != null && Array.Exists(validExtensions, ext => ext == fileExtension);
        }
    }
}
