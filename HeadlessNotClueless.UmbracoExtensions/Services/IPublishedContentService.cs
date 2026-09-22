using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace HeadlessNotClueless.UmbracoExtensions.Services;

/// <summary>
/// Simple service that helps us with a few IPublishedContent operations
/// </summary>
public interface IPublishedContentService
{
    /// <summary>
    /// Takes Backoffice IContent and converts that to IPublishedContent
    /// </summary>
    IPublishedContent? ToPublishedContent(IContent content);

    /// <summary>
    /// Gets the first item in the root matching a certain type of PublishedContentModel
    /// </summary>
    T? GetFirstRootItem<T>() where T : PublishedContentModel;
}