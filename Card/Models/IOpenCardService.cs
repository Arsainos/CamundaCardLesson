using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Card.Models
{
    public interface IOpenCardService
    {
        Task CreateInstance(CardInfo cardInfo);
        Task MessageFrontOfficeRequest(string guid);
        Task MessageFrontOfficeResult(bool approved, bool securityCheck, string guid);
        Task MessageSecurityCheckRequest(string guid);
        Task MessageSecurityCheckResult(bool approved, bool securityCheck, string guid);
        Task SignalCloseProcessing();
        Task SignalStopProcessing();
        Task SignalAdditionalSecurityCheck();
    }
}
