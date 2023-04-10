using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class TestBundles : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string path = "C:\\Program Files (x86)\\Steam\\steamapps\\common\\Captain of Industry\\AssetBundles\\zippers_86df";
        var bundle = AssetBundle.LoadFromFile(path);
        var assets = bundle.AllAssetNames();
        foreach (var asset in assets)
        {
            Debug.Log(asset);
        }

        var prefab = bundle.LoadAsset<GameObject>("balancerfluid");
        Debug.Log(prefab);
        GameObject.Instantiate(prefab);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
