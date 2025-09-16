using MediatR;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Inspection.Application.Dto;
using Inspection.Application.Abstractions;
using Inspection.DataAccessLayer.Repository;
using Inspection.Domain.Entities;

namespace Inspection.Application.Features.Inspectors.Commands
{
    public class CreateInspectorCommandHandler : IRequestHandler<CreateInspectorCommand>
    {
        private readonly IInspectorService _inspectorService;

        public CreateInspectorCommandHandler(IInspectorService inspectorService)
        {
            _inspectorService = inspectorService;
        }

        public async Task Handle(CreateInspectorCommand request, CancellationToken cancellationToken)
        {
           await _inspectorService.CreateAsync(new CreateInspectorDto()
           {
               Email = request.Email,
               FullName = request.FullName,
               Password = request.Password,
               Phone= request.Phone,    
           });
        }
    }
}
