using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Library.Api.Requests;

namespace Library.Validators
{
    public class ArticleRequestValidator : AbstractValidator<ArticleRequest>
    {
        public ArticleRequestValidator()
        {
            RuleFor(request => request.PageNumber).GreaterThanOrEqualTo(1).WithMessage("Page number has to be greater or equal to 1");
        }
    }
}
