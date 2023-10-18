using Domain.Interfaces;
using Entities.Entities.Models;
using Infrastructure.Configuration;
using Infrastructure.Repositories.Generic;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class TaskRepository : GenericRepository<TaskModel>, ITask
    {
        private readonly DbContextOptions<ContextBase> _OptionsBuilder;

        public TaskRepository()
        {
            _OptionsBuilder = new DbContextOptions<ContextBase>();
        }

        public async Task<List<TaskModel>> ListarTasks(Expression<Func<TaskModel, bool>> exTask)
        {
            using (var banco = new ContextBase(_OptionsBuilder))
            {
                return await banco.Tasks.Where(exTask).AsNoTracking().ToListAsync();    
            }
        }
    }
}