using FinPlanner360.Business.Interfaces.Repositories;
using FinPlanner360.Business.Interfaces.Services;
﻿using FinPlanner360.Busines.Interfaces.Repositories;
using FinPlanner360.Business.Models;
using FinPlanner360.Repositories.Contexts;
using Microsoft.EntityFrameworkCore;

namespace FinPlanner360.Repositories.Repositories;

public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
{
    public CategoryRepository(FinPlanner360DbContext context, IAppIdentityUser appIdentityUser)
        : base(context, appIdentityUser)
    {
    }

    public override async Task<ICollection<Category>> GetAllAsync()
    {
        Guid? userId = _appIdentityUser != null ? _appIdentityUser.GetUserId() : null;

        if (userId != null)
        {
            return await _dbSet.Where(x => x.UserId == null || x.UserId == userId.Value).ToListAsync();
        }
        else
        {
            return await _dbSet.ToListAsync();
        }
    }

    public async Task<Category> GetCategoryById(Guid id)
    {
        //return await _context.Categories
        //    .AsNoTracking()
        //    .Include(c => c.Transactions.Take(1)) // Include only one Transaction
        //    .Where(c => c.CategoryId == id)
        //    .FirstOrDefaultAsync();

        return await _context.Categories
            .AsNoTracking()
            .Where(c => c.CategoryId == id)
            .Select(c => new Category
            {
                CategoryId = c.CategoryId,
                Description = c.Description,
                Type = c.Type,
                UserId = c.UserId,
                Transactions = c.Transactions.Take(1).ToList() // Include only one Transaction
            })
            .FirstOrDefaultAsync();
    }
}