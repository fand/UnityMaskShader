﻿using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class MaskControllerTest
{
    [Test]
    public void StartWithoutPlayerPrefs()
    {
        var o = new GameObject("object");
        o.AddComponent(typeof(MaskController));

        var mask = o.GetComponent<MaskController>();
        mask.shader = Shader.Find("Smatch/MaskShader");
        mask.prefsKey = "test";

        var blur = mask.blur;
        var squareness = mask.squareness;
        var width = mask.width;
        var height = mask.height;

        PlayerPrefs.DeleteAll();

        mask.Start();
        Assert.AreEqual(blur, mask.blur, "blurは初期値通り");
        Assert.AreEqual(squareness, mask.squareness, "squarenessは初期値通り");
        Assert.AreEqual(width, mask.width, "Start時にアスペクト比が計算される");
        Assert.AreEqual(height, mask.height, "Start時にアスペクト比が計算される");
    }

    [Test]
    public void StartWithPlayerPrefs()
    {
        var o = new GameObject("object");
        o.AddComponent(typeof(MaskController));

        var mask = o.GetComponent<MaskController>();
        mask.shader = Shader.Find("Smatch/MaskShader");
        mask.prefsKey = "test";

        PlayerPrefs.SetFloat("test:maskBlur", 1.23f);
        PlayerPrefs.SetFloat("test:masksquareness", 0.45f);
        PlayerPrefs.SetFloat("test:maskWidth", 1.23f);
        PlayerPrefs.SetFloat("test:maskHeight", 2.46f);

        mask.Start();
        Assert.AreEqual(1.23f, mask.blur, "blurがPlayerPrefsから復元される");
        Assert.AreEqual(0.45f, mask.squareness, "squarenessがPlayerPrefsから復元される");
        Assert.AreEqual(1.23f, mask.width, "widthがPlayerPrefsから復元される");
        Assert.AreEqual(2.46f, mask.height, "heightがPlayerPrefsから復元される");
    }

    [Test]
    public void StartFailsIfShaderIsNotSet()
    {
        var o = new GameObject("object");
        o.AddComponent(typeof(MaskController));

        var mask = o.GetComponent<MaskController>();
        mask.prefsKey = "test";

        Assert.That(() => {
            mask.Start();
        }, Throws.Exception);
    }

    [Test]
    public void UpdateMask()
    {
        var o = new GameObject("object");
        o.AddComponent(typeof(MaskController));

        var mask = o.GetComponent<MaskController>();
        mask.shader = Shader.Find("Smatch/MaskShader");
        mask.prefsKey = "test";
        mask.Start();

        var blur = mask.blur;
        var squareness = mask.squareness;
        var width = mask.width;
        var height = mask.width;

        mask.blur = blur + 0.01f;
        mask.squareness = squareness + 0.02f;
        mask.width = width + 0.03f;
        mask.height = height + 0.04f;
        mask.UpdateMask();

        Assert.AreEqual(blur + 0.01f, mask.material.GetFloat("_MaskBlur"), "materialの_MaskBlurが更新されている");
        Assert.AreEqual(squareness + 0.02f, mask.material.GetFloat("_Masksquareness"), "materialの_MaskSquarenessが更新されている");
        Assert.AreEqual(width + 0.03f, mask.material.GetFloat("_MaskWidth"), "materialの_MaskWidthが更新されている");
        Assert.AreEqual(height + 0.04f, mask.material.GetFloat("_MaskHeight"), "materialの_MaskHeightが更新されている");

        Assert.AreEqual(blur + 0.01f, PlayerPrefs.GetFloat("test:maskBlur"), "PlayerPrefsのmaskBlurが更新されている");
        Assert.AreEqual(squareness + 0.02f, PlayerPrefs.GetFloat("test:masksquareness"), "PlayerPrefsのmaskSquarenessが更新されている");
        Assert.AreEqual(width + 0.03f, PlayerPrefs.GetFloat("test:maskWidth"), "PlayerPrefsのmaskWidthが更新されている");
        Assert.AreEqual(height + 0.04f, PlayerPrefs.GetFloat("test:maskHeight"), "PlayerPrefsのmaskHeightが更新されている");
    }
}
