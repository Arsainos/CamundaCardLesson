using Camunda.Api.Client;
using Camunda.Api.Client.Message;
using Camunda.Api.Client.ProcessDefinition;
using Camunda.Api.Client.Signal;
using Card.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Card.Services
{
    public class OpenCardService : IOpenCardService
    {
        private readonly ILogger<OpenCardService> _logger;
        private readonly IBPMNService _service;

        public OpenCardService(ILogger<OpenCardService> logger, IBPMNService service)
        {
            _logger = logger;
            _service = service;
        }

        public async Task CreateInstance(CardInfo cardInfo)
        {
            var guid = Guid.NewGuid();

            var processVariables = new StartProcessInstance()
                .SetVariable("Name", VariableValue.FromObject(cardInfo.Name))
                .SetVariable("Inn", VariableValue.FromObject(cardInfo.Inn))
                .SetVariable("CardType", VariableValue.FromObject(cardInfo.CardType))
                .SetVariable("guid", VariableValue.FromObject(guid.ToString()));

            processVariables.BusinessKey = guid.ToString();

            try
            {
                var processStartResult = await _service.Client().ProcessDefinitions.ByKey("OpenCardProcess").StartProcessInstance(processVariables);

                _logger.LogInformation($"created instance with id {processStartResult.Id}");
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }

        public async Task MessageFrontOfficeRequest(string guid)
        {
            try
            {
                await _service.Client().Messages.DeliverMessage(new CorrelationMessage
                {
                    MessageName = "FrontOfficeCheck",
                    ProcessVariables = new Dictionary<string, VariableValue>
                    {
                        ["guid"] = VariableValue.FromObject(guid)
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }            
        }

        public async Task MessageFrontOfficeResult(bool approved, bool securityCheck, string guid)
        {
            try
            {
                await _service.Client().Messages.DeliverMessage(new CorrelationMessage
                {
                    MessageName = "FrontOfficeCheckResult",
                    BusinessKey = guid,
                    ProcessVariables = new Dictionary<string, VariableValue>
                    {
                        ["FrontOfficeApproved"] = VariableValue.FromObject(approved),
                        ["FrontOfficeSecurityCheck"] = VariableValue.FromObject(securityCheck)
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }

        public async Task MessageSecurityCheckRequest(string guid)
        {
            try
            {
                await _service.Client().Messages.DeliverMessage(new CorrelationMessage
                {
                    MessageName = "BlacklistRequested",
                    ProcessVariables = new Dictionary<string, VariableValue>
                    {
                        ["guid"] = VariableValue.FromObject(guid)
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }

        public async Task MessageSecurityCheckResult(bool approved, bool securityCheck, string guid)
        {
            try
            {
                await _service.Client().Messages.DeliverMessage(new CorrelationMessage
                {
                    MessageName = "BlacklistResult",
                    BusinessKey = guid,
                    ProcessVariables = new Dictionary<string, VariableValue>
                    {
                        ["Blacklisted"] = VariableValue.FromObject(approved),
                        ["BlacklistedSecurityCheck"] = VariableValue.FromObject(securityCheck)
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }

        public async Task SignalCloseProcessing()
        {
            try
            {
                await _service.Client().Signals.ThrowSignal(new Signal
                {
                    Name = "CardOpenStopProcessingSignal"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }

        public async Task SignalAdditionalSecurityCheck()
        {
            try
            {
                await _service.Client().Signals.ThrowSignal(new Signal
                {
                    Name = "AdditionalSecurity"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }

        public async Task SignalStopProcessing()
        {
            try
            {
                await _service.Client().Signals.ThrowSignal(new Signal
                {
                    Name = "StopProcessing"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }
    }
}
