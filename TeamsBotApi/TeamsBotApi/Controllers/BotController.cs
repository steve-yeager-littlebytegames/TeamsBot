using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Extensions.Logging;

namespace TeamsBotApi.Controllers
{
    [ApiController]
    [Route("api/messages")]
    public class BotController : ControllerBase
    {
        private readonly IBotFrameworkHttpAdapter adapter;
        private readonly IBot bot;
        private readonly ILogger<BotController> logger;

        public BotController(IBotFrameworkHttpAdapter adapter, IBot bot, ILogger<BotController> logger)
        {
            this.adapter = adapter;
            this.bot = bot;
            this.logger = logger;
        }

        [HttpPost]
        public async Task PostAsync()
        {
            logger.LogInformation("Recieved Teams message.");
            await adapter.ProcessAsync(Request, Response, bot);
        }
    }
}