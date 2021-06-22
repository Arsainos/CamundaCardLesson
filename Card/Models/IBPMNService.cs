using Camunda.Api.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Card.Models
{
    public interface IBPMNService
    {
        CamundaClient Client();
    }
}
