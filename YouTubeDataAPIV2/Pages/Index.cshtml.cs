using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;

namespace YouTubeDataAPIV2.Pages
{
    public class IndexModel : PageModel
    {
        public List<YouTubeVideo> Videos { get; private set; } = new List<YouTubeVideo>();

        private readonly YouTubeService youTubeService;

        public IndexModel(YouTubeService youTubeService)
        {
            this.youTubeService = youTubeService;
        }
        public async Task OnGet(string data)
        {
            var searchListRequest = youTubeService.Search.List("snippet");
            searchListRequest.Q = data;
            searchListRequest.Type = "video";
            searchListRequest.Order = SearchResource.ListRequest.OrderEnum.Relevance;
            searchListRequest.MaxResults = 12;

            var searchListResponse = await searchListRequest.ExecuteAsync();
            Videos.AddRange(searchListResponse.Items.Select(video => new YouTubeVideo
            {
                Thumbnail = video.Snippet.Thumbnails.High.Url,
                Title = video.Snippet.Title,
                VideoId = video.Id.VideoId,
                Description = video.Snippet.Description
            }));
        }
    }
    public class YouTubeVideo
    {
        public string Thumbnail { get; internal set; }
        public string Title { get; internal set; }
        public string VideoId { get; internal set; }
        public string Description { get; internal set; }
        public string VideoUrl { get; internal set; }
    }
}