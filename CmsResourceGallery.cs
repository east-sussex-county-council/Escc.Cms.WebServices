using System;
using System.Collections.ObjectModel;
using Microsoft.ContentManagement.Publishing;

namespace Escc.Cms.WebServices
{
    /// <summary>
    /// Serialisable object which acts as a proxy for <see cref="ResourceGallery"/>
    /// </summary>
    public class CmsResourceGallery
    {
        private Collection<CmsResourceGallery> galleries = new Collection<CmsResourceGallery>();
        private Collection<CmsResource> resources = new Collection<CmsResource>();

        /// <summary>
        /// Gets or sets the GUID.
        /// </summary>
        /// <value>The GUID.</value>
        public Guid Guid { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the path.
        /// </summary>
        /// <value>The path.</value>
        public string Path { get; set; }

        /// <summary>
        /// Gets the galleries.
        /// </summary>
        /// <value>The galleries.</value>
        public Collection<CmsResourceGallery> Galleries { get { return this.galleries; } }

        /// <summary>
        /// Gets the resources.
        /// </summary>
        /// <value>The resources.</value>
        public Collection<CmsResource> Resources { get { return this.resources; } }
    }
}