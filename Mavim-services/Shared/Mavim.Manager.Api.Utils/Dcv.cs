using System.Text.RegularExpressions;

namespace Mavim.Manager.Api.Utils
{
    public static class DcvUtils
    {
        public static bool IsValid(string dcvId) =>
            !string.IsNullOrWhiteSpace(dcvId)
            && Regex.IsMatch(dcvId, RegexUtils.Dcv, RegexOptions.IgnoreCase);
    }
}