using Domain.Contracts;
using Domain.Contracts.Auth;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Resend;
using ServiceAbstraction.Contracts;
using ServiceAbstraction.Contracts.Auth;
using ServiceAbstraction.Contracts.Caching;
using ServiceAbstraction.Contracts.Incident;
using ServiceAbstraction.Contracts.Logs;
using ServiceAbstraction.Contracts.ServiceDeployments;
using ServiceAbstraction.Contracts.ServiceMangment;
using ServiceLayer.Services.Auth;
using ServiceLayer.Services.Caching;
using ServiceLayer.Services.Incidents;
using ServiceLayer.Services.Logs;
using ServiceLayer.Services.ServiceDeployments;
using ServiceLayer.Services.ServiceMangment;

namespace ServiceLayer.Services
{
    public class ServiceManger : IServiceManger
    {
        private readonly Lazy<IEmailService> _emailService;
        private readonly Lazy<ITokenService> _tokenService;
        private readonly Lazy<IAuthService> _authService;
        private readonly Lazy<IUserMangementService> _userMangementService;
        private readonly Lazy<IRediesCachingService> _rediesCachingService;
        private readonly Lazy<IIncidentEventService> _incidentEventService;
        private readonly Lazy<IIncidentService> _incidentService;
        private readonly Lazy<ILogsService> _logsService;
        //private readonly Lazy<ILogsTrackingService> _logsTrackingService;
        private readonly Lazy<IServiceDeploymentService> _serviceDeploymentService;
        private readonly Lazy<IAnalyzeServiceJob> _analyzeServiceJob;
        private readonly Lazy<IServiceMangementService> _serviceMangementService;

        public ServiceManger(
            IResend resend,
            IAuthRepo authRepo,
            IConfiguration configuration,
            IUserMangementRepo userMangementRepo,
            IDistributedCache cache,
            ILogger<RediesCachingService> logger,
            IUnitOfWork unitOfWork,
            IMediator mediator)
        {

            _emailService = new Lazy<IEmailService>(() => new EmailService(resend));
            _tokenService = new Lazy<ITokenService>(() => new TokenService(configuration));
            _rediesCachingService = new Lazy<IRediesCachingService>(() => new RediesCachingService(cache, logger));
            _userMangementService = new Lazy<IUserMangementService>(() => new UserMangmentService(userMangementRepo));


            _incidentEventService = new Lazy<IIncidentEventService>(() => new IncidentEventService(unitOfWork));
            _incidentService = new Lazy<IIncidentService>(() => new IncidentService(unitOfWork));
            _logsService = new Lazy<ILogsService>(() => new LogsService(unitOfWork, mediator));
            //_logsTrackingService = new Lazy<ILogsTrackingService>(() => new LogsTrackingService(unitOfWork)); 
            _serviceDeploymentService = new Lazy<IServiceDeploymentService>(() => new ServiceDeploymentService(unitOfWork));
            _analyzeServiceJob = new Lazy<IAnalyzeServiceJob>(() => new AnalyzeServicesJob(unitOfWork));


            _serviceMangementService = new Lazy<IServiceMangementService>(() => new ServiceManagementService(unitOfWork, RediesCachingService));
            _authService = new Lazy<IAuthService>(() => new AuthService(authRepo, TokenService, EmailService, RediesCachingService));
        }


        public IAuthService AuthService => _authService.Value;
        public IEmailService EmailService => _emailService.Value;
        public ITokenService TokenService => _tokenService.Value;
        public IUserMangementService UserMangementService => _userMangementService.Value;
        public IRediesCachingService RediesCachingService => _rediesCachingService.Value;
        public IIncidentEventService IncidentEventService => _incidentEventService.Value;
        public IIncidentService IncidentService => _incidentService.Value;
        public ILogsService LogsService => _logsService.Value;
        //public ILogsTrackingService logsTrackingService => _logsTrackingService.Value;
        public IServiceDeploymentService ServiceDeploymentService => _serviceDeploymentService.Value;
        public IAnalyzeServiceJob AnalyzeServiceJob => _analyzeServiceJob.Value;
        public IServiceMangementService ServiceMangementService => _serviceMangementService.Value;
    }
}