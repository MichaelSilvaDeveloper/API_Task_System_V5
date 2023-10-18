using Domain.Interfaces;
using Domain.InterfacesServices;
using Entities.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Service
{
    public class ServiceTask : IServiceTask
    {
        private readonly ITask _iTask;

        public ServiceTask(ITask iTask)
        {
            _iTask = iTask;
        }

        public async Task AdicionarTask(TaskModel taskmodel)
        {
            taskmodel.DataAlteracao = DateTime.Now;
            taskmodel.DataCadastro = DateTime.Now;
            taskmodel.Ativo = true;
            await _iTask.Adicionar(taskmodel);
        }

        public async Task AtualizarTask(TaskModel taskmodel)
        {
            taskmodel.DataAlteracao = DateTime.Now;
            taskmodel.Ativo = true;
            await _iTask.Atualizar(taskmodel);    
        }

        public async Task<List<TaskModel>> ListarNoticiasAtiva()
        {
            return await _iTask.ListarTasks(n => n.Ativo);
        }
    }
}
