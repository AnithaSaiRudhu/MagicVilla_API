using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace MagicVilla_VillaAPI.Repository
{
    public class VillaRepository : Repository<Villa>, IVillaRepository
    {
        private readonly ApplicationDBContext _db;
        public VillaRepository(ApplicationDBContext db):base (db)
        {
            _db = db;
        }       

        public async Task UpdateAsync(Villa entity)
        {
            entity.UpdateDate = DateTime.Now;
            _db.Update(entity);
            await _db.SaveChangesAsync();
        }       
    }
}
