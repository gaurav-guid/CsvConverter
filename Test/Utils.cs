using JsonDiffPatchDotNet;
using Newtonsoft.Json;

namespace Test;

public static class Utils
{
    public static bool ObjectsHaveDifferences(object left, object right)
    {
        string leftSerialized = JsonConvert.SerializeObject(left);
        string rightSerialized = JsonConvert.SerializeObject(right);

        var patch = new JsonDiffPatch().Diff(leftSerialized, rightSerialized);

        return !string.IsNullOrEmpty(patch);
    }
}
