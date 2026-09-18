using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Services.Navigation;
using Umbraco.Cms.Core.Web;
using Umbraco.Extensions;

namespace HeadlessNotClueless.UmbracoExtensions.Services;

internal class PublishedContentService(
    IUmbracoContextFactory _contextFactory,
    IDocumentNavigationQueryService _documentNavigationService,
    IPublishedContentStatusFilteringService _publishedStatusFilteringService) : IPublishedContentService
{
    public IPublishedContent? ToPublishedContent(IContent content)
    {
        return GetContext()?.Content.GetById(content.Id);
    }

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