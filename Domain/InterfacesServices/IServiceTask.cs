using Entities.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.InterfacesServices
{
    public interface IServiceTask
    {
        Task AdicionarTask(TaskModel taskmodel);

        Task AtualizarTask(TaskModel taskmodel);

        Task<List<TaskModel>> ListarNoticiasAtiva();
    }
}