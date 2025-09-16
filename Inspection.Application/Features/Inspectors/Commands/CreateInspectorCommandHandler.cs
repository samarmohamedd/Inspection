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
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public CreateInspectorCommandHandler(IInspectorService inspectorService)
        {
            _inspectorService = inspectorService;
        }

        public async Task Handle(CreateInspectorCommand request, CancellationToken cancellationToken)
        {
            //_inspectorService.CreateAsync(request);
        }
    }
}
