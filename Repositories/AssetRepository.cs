using Microsoft.EntityFrameworkCore;
using PortifolioFinanceiro.Infrastructure;
using PortifolioFinanceiro.Models;

namespace PortifolioFinanceiro.Repositories
{
    public class AssetRepository : IAssetRepository
    {
        private readonly AppDbContext _context;

        public AssetRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Asset>> GetAllAsync()
        {
            return await _context.Assets
                .Include(a => a.PriceHistories)
                .OrderBy(a => a.Symbol)
                .ToListAsync();
        }

        public async Task<Asset?> GetByIdAsync(int id)
        {
            return await _context.Assets
                .Include(a => a.PriceHistories)
                .Include(a => a.Positions)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Asset?> GetBySymbolAsync(string symbol)
        {
            return await _context.Assets
                .Include(a => a.PriceHistories)
                .Include(a => a.Positions)
                .FirstOrDefaultAsync(a => a.Symbol == symbol);
        }

        public async Task<Asset> CreateAsync(Asset asset)
        {
            _context.Assets.Add(asset);
            await _context.SaveChangesAsync();
            return asset;
        }

        public async Task<Asset> UpdateAsync(Asset asset)
        {
            _context.Assets.Update(asset);
            await _context.SaveChangesAsync();
            return asset;
        }

        public async Task DeleteAsync(int id)
        {
            var asset = await _context.Assets.FindAsync(id);
            if (asset != null)
            {
                _context.Assets.Remove(asset);
                await _context.SaveChangesAsync();
            }
        }
    }
}
