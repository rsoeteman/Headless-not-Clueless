using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace HeadlessNotClueless.UmbracoExtensions.Services;

public interface IPublishedContentService
{
    IPublishedContent? ToPublishedContent(IContent content);

}