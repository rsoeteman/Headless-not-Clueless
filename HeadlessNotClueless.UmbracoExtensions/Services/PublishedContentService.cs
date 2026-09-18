using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Services.Navigation;
using Umbraco.Cms.Core.Web;

namespace HeadlessNotClueless.UmbracoExtensions.Services;

public class PublishedContentService(IUmbracoContextFactory _contextFactory) : IPublishedContentService
{
    public IPublishedContent? ToPublishedContent(IContent content)
    {
        return GetContext()?.Content.GetById(content.Id);
    }

    private IUmbracoContext? GetContext()
    {
        return _contextFactory.EnsureUmbracoContext().UmbracoContext;
    }
}