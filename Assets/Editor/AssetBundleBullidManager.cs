using System.IO;
using UnityEditor;

public class AssetBundleBuildManager
{
    [MenuItem("MyTool/Asset Bundle")]
    public static void AssetBundleBullid()
    {
        string directory = "./AssetBundle";

        if(!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        BuildPipeline.BuildAssetBundles(directory, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);

        EditorUtility.DisplayDialog("에셋 번들 빌드", "에셋 번들 빌드를 완료했습니다.", "완료");
    }
}
