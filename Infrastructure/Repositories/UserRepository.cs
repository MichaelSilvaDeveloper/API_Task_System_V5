using Domain.Interfaces;
using Entities.Enums;
using Entities.Models;
using Infrastructure.Configuration;
using Infrastructure.Repositories.Generic;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class UserRepository : GenericRepository<ApplicationUser>, IUsuario
    {
        private readonly DbContextOptions<ContextBase> _OptionsBuilder;

        public UserRepository()
        {
            _OptionsBuilder = new DbContextOptions<ContextBase>();
        }

        public async Task AdicionarUsuario(string email, string senha, int idade, string celular)
        {
            try
            {
                using (var data = new ContextBase(_OptionsBuilder))
                {
                    await data.ApplicationUser.AddAsync(
                        new ApplicationUser
                        {
                            Email = email,
                            PasswordHash = senha,
                            Idade = idade,
                            Celular = celular,
                            UserType = UserType.Common
                        });
                    await data.SaveChangesAsync();
                }
            }
            catch (Exception)
            {
                throw new Exception("Erro ao adicionar");
            }
        }

        public async Task<bool> ExisteUsuario(string email, string senha)
        {
            try
            {
                using (var data = new ContextBase(_OptionsBuilder))
                {
                    return await data.ApplicationUser
                        .Where(u => u.Email.Equals(email) && u.PasswordHash.Equals(senha))
                        .AsNoTracking()
                        .AnyAsync();
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<string> RetornaIdUsuario(string email)
        {
            try
            {
                using (var data = new ContextBase(_OptionsBuilder))
                {
                    var getIdUser = await data.ApplicationUser
                        .Where(u => u.Email.Equals(email))
                        .AsNoTracking()
                        .FirstOrDefaultAsync();
                    return getIdUser.Id;
                }
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }
}