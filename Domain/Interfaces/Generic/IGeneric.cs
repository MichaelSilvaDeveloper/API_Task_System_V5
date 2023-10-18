using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Generic
{
    public interface IGeneric<T> where T : class
    {
        Task Adicionar(T Object);

        Task Atualizar(T Object);

        Task Excluir(T Object);

        Task<T> BuscarPorId(int id);

        Task<List<T>> Listar();
    }
}