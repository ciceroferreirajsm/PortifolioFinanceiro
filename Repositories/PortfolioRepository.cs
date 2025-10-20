using Microsoft.EntityFrameworkCore;
using PortifolioFinanceiro.Infrastructure;
using PortifolioFinanceiro.Models;

namespace PortifolioFinanceiro.Repositories
{
    public class PortfolioRepository : IPortfolioRepository
    {
        private readonly AppDbContext _context;

        public PortfolioRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Portfolio>> GetAllAsync()
        {
            return await _context.Portfolios
                .Include(p => p.User)
                .Include(p => p.Positions)
                    .ThenInclude(pos => pos.Asset)
                .OrderBy(p => p.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<Portfolio>> GetByUserIdAsync(string userId)
        {
            return await _context.Portfolios
                .Include(p => p.User)
                .Include(p => p.Positions)
                    .ThenInclude(pos => pos.Asset)
                .Where(p => p.UserId == userId)
                .OrderBy(p => p.Name)
                .ToListAsync();
        }

        public async Task<Portfolio?> GetByIdAsync(int id)
        {
            return await _context.Portfolios
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Portfolio?> GetByIdWithPositionsAsync(int id)
        {
            return await _context.Portfolios
                .Include(p => p.User)
                .Include(p => p.Positions)
                    .ThenInclude(pos => pos.Asset)
                        .ThenInclude(a => a.PriceHistories)
                .Include(p => p.Transactions)
                    .ThenInclude(t => t.Asset)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Portfolio> CreateAsync(Portfolio portfolio)
        {
            _context.Portfolios.Add(portfolio);
            await _context.SaveChangesAsync();
            return portfolio;
        }

        public async Task<Portfolio> UpdateAsync(Portfolio portfolio)
        {
            _context.Portfolios.Update(portfolio);
            await _context.SaveChangesAsync();
            return portfolio;
        }

        public async Task DeleteAsync(int id)
        {
            var portfolio = await _context.Portfolios.FindAsync(id);
            if (portfolio != null)
            {
                _context.Portfolios.Remove(portfolio);
                await _context.SaveChangesAsync();
            }
        }
    }
}
