namespace Mavim.Libraries.Middlewares.WopiValidator.Helpers
{
    public interface IWopiActionUrlMetaData
    {
        string WopiTestViewerActionUrl { get; set; }

        string WordViewerActionUrl { get; set; }

        string WordEditorActionUrl { get; set; }

        string WordNewEditorActionUrl { get; set; }

        string VisioViewerActionUrl { get; set; }
    }
}
