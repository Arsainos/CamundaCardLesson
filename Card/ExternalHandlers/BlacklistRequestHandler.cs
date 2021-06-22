using Camunda.Worker;
using Card.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Card.ExternalHandlers
{
    [HandlerTopics("BlacklistRequest", LockDuration = 10000)]
    [HandlerVariables("guid")]
    public class BlacklistRequestHandler : IExternalTaskHandler
    {
        private readonly IOpenCardService _openCardService;

        public BlacklistRequestHandler(IOpenCardService openCardService)
        {
            _openCardService = openCardService;
        }

        public async Task<IExecutionResult> HandleAsync(ExternalTask externalTask, CancellationToken cancellationToken)
        {
            await _openCardService.MessageSecurityCheckRequest((string)externalTask.Variables["guid"].Value);

            return new CompleteResult();
        }
    }
}
