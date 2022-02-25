using System;
using System.Net;
using System.Threading.Tasks;
using Sieve.Models;
using UIM.BAL.Services.Interfaces;
using UIM.Common;
using UIM.Common.ResponseMessages;
using UIM.DAL.Repositories.Interfaces;
using UIM.Model.Dtos.Common;
using UIM.Model.Entities;

namespace UIM.BAL.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository;

        public DepartmentService(IDepartmentRepository departmentRepository) =>
            _departmentRepository = departmentRepository;

        public async Task AddAsync(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(string.Empty);

            if (await _departmentRepository.GetByNameAsync(name) != null)
                throw new HttpException(HttpStatusCode.BadRequest,
                                        ErrorResponseMessages.BadRequest);

            var added = await _departmentRepository.AddAsync(name);
            if (!added)
                throw new HttpException(HttpStatusCode.InternalServerError,
                                        ErrorResponseMessages.UnexpectedError);
        }

        public Department Edit(int id, string newName)
        {
            if (string.IsNullOrEmpty(newName))
                throw new ArgumentNullException(string.Empty);

            throw new System.NotImplementedException();
        }

        public TableResponse GetDepartments(SieveModel model)
        {
            throw new System.NotImplementedException();
        }

        public Task RemoveAsync(Department item)
        {
            throw new System.NotImplementedException();
        }
    }
}