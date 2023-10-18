using API_Task_System_V5.Models;
using Domain.Interfaces;
using Domain.InterfacesServices;
using Entities.Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API_Task_System_V5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly IServiceTask _iServiceTask;
        private readonly ITask _iTask;

        public TaskController(IServiceTask iServiceTask, ITask iTask)
        {
            _iServiceTask = iServiceTask;
            _iTask = iTask;
        }

        [Authorize]
        [Produces("application/json")]
        [HttpPost("/api/ListarNoticias")]
        public async Task<List<TaskModel>> ListarNoticias()
        {
            return await _iServiceTask.ListarNoticiasAtiva();
        }

        [Authorize]
        [Produces("application/json")]
        [HttpPost("/api/BuscarNoticiaById")]
        public async Task<TaskModel> BuscarNoticiaById(TaskViewModel taskViewModel)
        {
            return await _iTask.BuscarPorId(taskViewModel.IdTask);

        }

        [Authorize]
        [Produces("application/json")]
        [HttpPost("/api/ExcluirTask")]
        public async Task ExcluirTask(TaskViewModel taskViewModel)
        {
            var removeTask = await _iTask.BuscarPorId(taskViewModel.IdTask);
            await _iTask.Excluir(removeTask);
        }

        [Authorize]
        [Produces("application/json")]
        [HttpPost("/api/AtualizarTask")]
        public async Task AtualizarTask(TaskViewModel taskViewModel)
        {
            var task = await _iTask.BuscarPorId(taskViewModel.IdTask);
            task.Titulo = taskViewModel.Titulo;
            task.Informacao = taskViewModel.Informacao;
            task.UserId = await RetornarIdUsuarioLogado();
            await _iServiceTask.AtualizarTask(task);
        }

        [Authorize]
        [Produces("application/json")]
        [HttpPost("/api/Add")]
        public async Task Add(TaskViewModel taskViewModel)
        {
            var taskNova = new TaskModel
            {
                Titulo = taskViewModel.Titulo,
                Informacao = taskViewModel.Informacao,
                UserId = await RetornarIdUsuarioLogado()
            };  
            await _iServiceTask.AdicionarTask(taskNova);
        }

        private async Task<string> RetornarIdUsuarioLogado()
        {
            if(User != null)
            {
                var idUsuario = User.FindFirst("idUsuario");
                return idUsuario.Value;
            }
            else
            {
                return string.Empty;
            }
        }
    }
}