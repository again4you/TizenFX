using System;
using System.ComponentModel;
using System.Xml;

namespace Tizen.NUI.Binding.Internals
{
    /// <summary>
    /// The interface INameScope.
    /// </summary>
    /// <since_tizen> 5 </since_tizen>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface INameScope
    {
        /// This will be public opened in tizen_5.0 after ACR done. Before ACR, need to be hidden as inhouse API.
        [EditorBrowsable(EditorBrowsableState.Never)]
        object FindByName(string name);

        /// This will be public opened in tizen_5.0 after ACR done. Before ACR, need to be hidden as inhouse API.
        [EditorBrowsable(EditorBrowsableState.Never)]
        void RegisterName(string name, object scopedElement);

        /// This will be public opened in tizen_5.0 after ACR done. Before ACR, need to be hidden as inhouse API.
        [EditorBrowsable(EditorBrowsableState.Never)]
        void UnregisterName(string name);

        /// This will be public opened in tizen_5.0 after ACR done. Before ACR, need to be hidden as inhouse API.
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete]void RegisterName(string name, object scopedElement, IXmlLineInfo xmlLineInfo);
    }
}
