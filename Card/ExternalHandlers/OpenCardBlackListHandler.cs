using Camunda.Worker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Card.ExternalHandlers
{
    [HandlerTopics("OpenCardBlackList", LockDuration = 10000)]
    public class OpenCardBlackListHandler : IExternalTaskHandler
    {
        public async Task<IExecutionResult> HandleAsync(ExternalTask externalTask, CancellationToken cancellationToken)
        {
            var approved = Convert.ToBoolean(new Random().Next(0, 2));
            var securityCheck = Convert.ToBoolean(new Random().Next(0, 2));

            return new CompleteResult
            {
                Variables = new Dictionary<string, Variable>
                {
                    ["Approved"] = Variable.Boolean(approved),
                    ["SecurityCheck"] = Variable.Boolean(securityCheck)
                }
            };
        }
    }
}
