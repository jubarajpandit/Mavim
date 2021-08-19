using Mavim.Libraries.Middlewares.WopiValidator.Helpers;

namespace Mavim.Libraries.Middlewares.WopiValidator.Models
{
    public class WopiActionUrlMetadata : IWopiActionUrlMetaData
    {
        public string WopiTestViewerActionUrl { get; set; }
        public string WordViewerActionUrl { get; set; }
        public string WordEditorActionUrl { get; set; }
        public string WordNewEditorActionUrl { get; set; }
        public string VisioViewerActionUrl { get; set; }        
    }
}
