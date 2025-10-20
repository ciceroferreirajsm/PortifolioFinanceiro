using Microsoft.EntityFrameworkCore;
using PortifolioFinanceiro.Infrastructure;
using PortifolioFinanceiro.Models;

namespace PortifolioFinanceiro.Repositories
{
    public class PositionRepository : IPositionRepository
    {
        private readonly AppDbContext _context;

        public PositionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Position>> GetByPortfolioIdAsync(int portfolioId)
        {
            return await _context.Positions
                .Include(p => p.Asset)
                .Include(p => p.Portfolio)
                .Where(p => p.PortfolioId == portfolioId)
                .OrderBy(p => p.Asset.Symbol)
                .ToListAsync();
        }

        public async Task<Position?> GetByIdAsync(int id)
        {
            return await _context.Positions
                .Include(p => p.Asset)
                .Include(p => p.Portfolio)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Position> CreateAsync(Position position)
        {
            _context.Positions.Add(position);
            await _context.SaveChangesAsync();
            return position;
        }

        public async Task<Position> UpdateAsync(Position position)
        {
            _context.Positions.Update(position);
            await _context.SaveChangesAsync();
            return position;
        }

        public async Task DeleteAsync(int id)
        {
            var position = await _context.Positions.FindAsync(id);
            if (position != null)
            {
                _context.Positions.Remove(position);
                await _context.SaveChangesAsync();
            }
        }
    }
}
