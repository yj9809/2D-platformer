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

        EditorUtility.DisplayDialog("���� ���� ����", "���� ���� ���带 �Ϸ��߽��ϴ�.", "�Ϸ�");
    }
}
