using System;
using Microsoft.ContentManagement.Publishing;

namespace Escc.Cms.WebServices
{
    /// <summary>
    /// Serialisable object which acts as a proxy for <see cref="Resource"/>
    /// </summary>
    public class CmsResource
    {
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
        /// Gets or sets the display name.
        /// </summary>
        /// <value>The display name.</value>

        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the path.
        /// </summary>
        /// <value>The path.</value>
        public string Path { get; set; }
    }
}