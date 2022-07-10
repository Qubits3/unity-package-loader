using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public static class PackageLoader
{
    [MenuItem("Tools/Qubits/Import Packages")]
    static async void LoadNewManifest()
    {
        var url = GetGistUrl("93abf285726058bb46d9d30a333418ec");
        var contents = await GetContents(url);

        ReplacePackageFile(contents);
    }

    static string GetGistUrl(string id, string user = "Qubits3") => $"https://gist.github.com/{user}/{id}/raw";

    static async Task<string> GetContents(string url)
    {
        using var client = new HttpClient();
        var response = await client.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();

        return content;
    }

    static void ReplacePackageFile(string contents)
    {
        var existing = Path.Combine(Application.dataPath, "../Packages/manifest.json");
        File.WriteAllText(existing, contents);

        UnityEditor.PackageManager.Client.Resolve();
    }
}