using Card.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Card.Controllers
{
    public class OpenCard : Controller
    {
        private readonly IOpenCardService _openCardService;

        public OpenCard(ILogger<OpenCard> logger, IOpenCardService openCardService)
        {
            _openCardService = openCardService;
        }

        // POST: OpenCard/Open
        [HttpPost]
        [Produces("application/json")]
        public async Task<ActionResult> Open([FromBody] CardInfo cardInfo)
        {
            await _openCardService.CreateInstance(cardInfo);

            return Ok();
        }
        
        // POST: OpenCard/Stop
        [HttpPost]
        [Produces("application/json")]
        public async Task<ActionResult> Stop()
        {
            await _openCardService.SignalCloseProcessing();

            return Ok();
        }
    }
}
