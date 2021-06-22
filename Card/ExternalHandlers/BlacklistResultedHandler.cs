using Camunda.Worker;
using Card.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Card.ExternalHandlers
{
    [HandlerTopics("BlacklistResulted", LockDuration = 10000)]
    [HandlerVariables(new string[] { "Approved", "SecurityCheck", "guid" })]
    public class BlacklistResultedHandler : IExternalTaskHandler
    {
        private readonly IOpenCardService _openCardService;

        public BlacklistResultedHandler(IOpenCardService openCardService)
        {
            _openCardService = openCardService;
        }

        public async Task<IExecutionResult> HandleAsync(ExternalTask externalTask, CancellationToken cancellationToken)
        {
            await _openCardService.MessageSecurityCheckResult(
                (bool)externalTask.Variables["Approved"].Value, 
                (bool)externalTask.Variables["SecurityCheck"].Value,
                (string)externalTask.Variables["guid"].Value
            );

            return new CompleteResult();
        }
    }
}
