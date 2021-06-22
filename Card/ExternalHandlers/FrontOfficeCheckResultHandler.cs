using Camunda.Worker;
using Card.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Card.ExternalHandlers
{
    [HandlerTopics("FrontOfficeCheckResult", LockDuration = 10000)]
    [HandlerVariables(new string[] { "Approved", "SecurityCheck", "guid" })]
    public class FrontOfficeCheckResultHandler : IExternalTaskHandler
    {
        private readonly IOpenCardService _openCardService;

        public FrontOfficeCheckResultHandler(IOpenCardService openCardService)
        {
            _openCardService = openCardService;
        }

        public async Task<IExecutionResult> HandleAsync(ExternalTask externalTask, CancellationToken cancellationToken)
        {
            await _openCardService.MessageFrontOfficeResult(
                (bool)externalTask.Variables["Approved"].Value, 
                (bool)externalTask.Variables["SecurityCheck"].Value,
                (string)externalTask.Variables["guid"].Value   
            );

            return new CompleteResult();
        }
    }
}
