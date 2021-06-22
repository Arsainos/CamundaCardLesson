using Camunda.Worker;
using Card.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Card.ExternalHandlers
{
    [HandlerTopics("FrontOfficeCheckRequest", LockDuration = 10000)]
    [HandlerVariables("guid")]
    public class FrontOfficeCheckRequestHandler : IExternalTaskHandler
    {
        private readonly IOpenCardService _openCardService;

        public FrontOfficeCheckRequestHandler(IOpenCardService openCardService)
        {
            _openCardService = openCardService;
        }

        public async Task<IExecutionResult> HandleAsync(ExternalTask externalTask, CancellationToken cancellationToken)
        {
            await _openCardService.MessageFrontOfficeRequest((string)externalTask.Variables["guid"].Value);

            return new CompleteResult();            
        }
    }
}
