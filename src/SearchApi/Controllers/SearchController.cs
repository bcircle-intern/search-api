using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.IO;

namespace SearchApi.Controllers {

    [Route("/api/[controller]/[action]")]
    public class SearchController : ControllerBase {

        ILogger<SearchController> logger;

        public SearchController(ILogger<SearchController> logger) {
            this.logger = logger;
        }

        private (bool, string) Validate(SearchRequest request) {
            if (string.IsNullOrEmpty(request.Path) || !Directory.Exists(request.Path)) {
                return (false, "Invalid path");
            } else if (string.IsNullOrEmpty(request.Pattern)) {
                return (false, "Invalid pattern");
            }
            return (true, "");
        }

        private string[] GetFile(string path, string pattern) =>
            Directory.GetFiles(path, pattern, SearchOption.AllDirectories);

        [HttpPost]
        public IActionResult SearchFile([FromBody] SearchRequest request) {
            logger.LogInformation("Search file {@Resut}", request);
            var (ok, message) = Validate(request);
            if (ok) {
                var files = GetFile(request.Path, request.Pattern);
                return Ok(files);
            } else {
                return BadRequest(message);
            }
        }
    }
}