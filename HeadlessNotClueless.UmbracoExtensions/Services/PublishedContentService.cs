using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Services.Navigation;
using Umbraco.Cms.Core.Web;
using Umbraco.Extensions;

namespace HeadlessNotClueless.UmbracoExtensions.Services;

/// <summary>
/// Simple service that helps us with a few IPublishedContent operations
/// </summary>
internal class PublishedContentService(
    IUmbracoContextFactory _contextFactory,
    IDocumentNavigationQueryService _documentNavigationService,
    IPublishedContentStatusFilteringService _publishedStatusFilteringService) : IPublishedContentService
{
    /// <summary>
    /// Takes Backoffice IContent and converts that to IPublishedContent
    /// </summary>
    public IPublishedContent? ToPublishedContent(IContent content)
    {
        return GetContext()?.Content.GetById(content.Id);
    }
    
    /// <summary>
    /// Gets the first item in the root matching a certain type of PublishedContentModel
    /// </summary>
    public T? GetFirstRootItem<T>() where T : PublishedContentModel
    {
        _documentNavigationService.TryGetRootKeys(out var allRoots);
        foreach (var rootKey in allRoots)
        {
            if (GetById(rootKey) is T publishedItem)
            {
                return  publishedItem;
            }
        }
        
        //No match
        return null;
    }

    private IPublishedContent? GetById(Guid id)
    {
        return GetContext()?.Content.GetById(id);
    }

      private IUmbracoContext? GetContext()
    {
        return _contextFactory.EnsureUmbracoContext().UmbracoContext;
    }
}