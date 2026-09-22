using HeadlessNotClueless.UmbracoExtensions.Services;
using Umbraco.Cms.Core.DeliveryApi;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Services.Navigation;
using Umbraco.Cms.Web.Common.PublishedModels;
using Umbraco.Extensions;

namespace HeadlessNotClueless.UmbracoExtensions.Resolvers;

public class CustomPathResolver(
    IPublishedContentService _publishedContentService,
    IDocumentNavigationQueryService _navigationQueryService,
    IPublishedContentStatusFilteringService _publishedStatusFilteringService,
    IRequestRoutingService requestRoutingService,
    IApiPublishedContentCache apiPublishedContentCache)
    : ApiContentPathResolver(requestRoutingService, apiPublishedContentCache)
{
    /// <summary>
    /// Resolver for items that can't be found in the current startnode.
    /// check "data" folder(s) based on type, currently for news
    /// </summary>
    /// <param name="path">The path to inspect</param>
    public override IPublishedContent? ResolveContentPath(string path)
    {
        var content = base.ResolveContentPath(path);
        
        //Already found return
        if (content != null) return content;
        
        //Try finding in news folder
        // Extract the slug from the path: /news/our-awesome-article/ → our-awesome-article
        var slug = path.Trim('/').Split('/').Last();

        // Get news folder
        var newsFolder = _publishedContentService.GetFirstRootItem<NewsFolder>();

        //No news folder in this site, nothing to resolve
        if (newsFolder == null)
        {
            return null;
        }

        // Find news item  by UrlSegment  in the news folder
        // We should include the check for website in here to.
        content = newsFolder.Children<NewsPage>(_navigationQueryService, _publishedStatusFilteringService, null).FirstOrDefault(n=>n.UrlSegment == slug);

        return content;
    }

}