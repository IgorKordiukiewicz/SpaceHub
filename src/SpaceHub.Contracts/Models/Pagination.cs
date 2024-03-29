﻿using FluentValidation;

namespace SpaceHub.Contracts.Models;

public class Pagination
{
    public Pagination(int pageNumber = 1, int itemsPerPage = 10)
    {
        PageNumber = pageNumber;
        ItemsPerPage = itemsPerPage;
    }

    public int PageNumber { get; set; }
    public int ItemsPerPage { get; set; }

    public int Offset => (PageNumber - 1) * ItemsPerPage;
    public int GetPagesCount(int itemsCount) => (itemsCount - 1) / ItemsPerPage + 1;
}

public class PaginationValidator : AbstractValidator<Pagination>
{
    public PaginationValidator()
    {
        RuleFor(x => x.PageNumber).NotNull().GreaterThanOrEqualTo(1);
        RuleFor(x => x.ItemsPerPage).NotNull().GreaterThanOrEqualTo(1);
    }
}