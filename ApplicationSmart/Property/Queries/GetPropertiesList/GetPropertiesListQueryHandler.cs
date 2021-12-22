using ApplicationSmart.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationSmart.Property.Queries.GetPropertiesList
{
    public class GetPropertiesListQueryHandler : IRequestHandler<GetPropertiesListQuery, PropertiesListVm>
    {
        private readonly ISmartSearchDataContext _context;
        //private readonly IElasticClient _elasticClient;
        private readonly IMapper _mapper;

        public GetPropertiesListQueryHandler(ISmartSearchDataContext context, IMapper mapper)
        {
            //_context = context;
            _context = context;
            _mapper = mapper;
        }

        public async Task<PropertiesListVm> Handle(GetPropertiesListQuery request, CancellationToken cancellationToken)
        {
            var properties = await _context.Properties
                .ProjectTo<PropertyLookUpDto>(_mapper.ConfigurationProvider)
                .OrderBy(i => i.Name)
                .ToListAsync(cancellationToken);

            var vm = new PropertiesListVm
            {
                Properties = properties,
            };
            return vm;
        }
    }
}
