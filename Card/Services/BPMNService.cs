using Camunda.Api.Client;
using Card.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Card.Services
{
    public class BPMNService : IBPMNService
    {
        private readonly CamundaClient _camunda;

        public BPMNService(string camundaRestApiUri)
        {
            this._camunda = CamundaClient.Create(camundaRestApiUri);
        }

        public CamundaClient Client()
        {
            return this._camunda;
        }
    }
}
