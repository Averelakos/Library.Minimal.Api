using FluentValidation;
using Library.Api.Entities;

namespace Library.Api.Validators
{
    public class BookValidator : AbstractValidator<Book>
    {
        public BookValidator()
        {
            RuleFor(book => book.Isbn)
                .Matches(@"^(?=(?:\\D*\\d){10}(?:(?:\\D*\\d){3})?$)[\\d-]+$")
                .WithMessage("Value was not a valid ISBN-13");
        }
    }
}
