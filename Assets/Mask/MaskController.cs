using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class MaskController : MonoBehaviour {
    const float BLUR_INIT = 1.0f;
    const float BLUR_MIN = 0.001f;
    const float BLUR_MAX = 10.0f;
    const float SQUARENESS_INIT = 0.5f;
    const float SQUARENESS_MIN = 0.0f;
    const float SQUARENESS_MAX = 10.0f;
    const float WIDTH_INIT = 1.0f;
    const float WIDTH_MIN = 0.01f;
    const float WIDTH_MAX = 3.0f;
    const float HEIGHT_INIT = 1.0f;
    const float HEIGHT_MIN = 0.01f;
    const float HEIGHT_MAX = 3.0f;

    const float ROTATION_INIT = 0.0f;
    const float ROTATION_MIN = -180.0f;
    const float ROTATION_MAX = 180.0f;

    [SerializeField, Range(BLUR_MIN, BLUR_MAX)]
    public float blur = BLUR_INIT;

    [SerializeField, Range(SQUARENESS_MIN, SQUARENESS_MAX)]
    public float squareness = SQUARENESS_INIT;

    [SerializeField, Range(WIDTH_MIN, WIDTH_MAX)]
    public float width = WIDTH_INIT;

    [SerializeField, Range(HEIGHT_MIN, HEIGHT_MAX)]
    public float height = HEIGHT_INIT;

    [SerializeField, Range(ROTATION_MIN, ROTATION_MAX)]
    public float rotation = ROTATION_INIT;

    [SerializeField]
    public string prefsKey;

    private bool isAlive = false; // To avoid update after OnApplicationQuit

    public Shader shader;

    [System.NonSerialized]
    public Material material;

    public void Start()
    {
        if (shader == null) {
            throw new Exception("Property \"shader\" must be set");
        }

        material = new Material(shader);

        isAlive = true;
        blur = PlayerPrefs.GetFloat(prefsKey + ":maskBlur", blur);
        squareness = PlayerPrefs.GetFloat(prefsKey + ":maskSquareness", squareness);
        width = PlayerPrefs.GetFloat(prefsKey + ":maskWidth", width);
        height = PlayerPrefs.GetFloat(prefsKey + ":maskHeight", height);
        rotation = PlayerPrefs.GetFloat(prefsKey + ":maskRotation", rotation);

        Invoke("UpdateMask", 0);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                blur = Mathf.Min(blur + 0.003f, BLUR_MAX);
                UpdateMask();
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                blur = Mathf.Max(blur - 0.003f, BLUR_MIN);
                UpdateMask();
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                squareness = Mathf.Max(squareness - 0.003f, SQUARENESS_MIN);
                UpdateMask();
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                squareness = Mathf.Min(squareness + 0.003f, SQUARENESS_MAX);
                UpdateMask();
            }
        }

        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                height = Mathf.Min(height + 0.003f, HEIGHT_MAX);
                UpdateMask();
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                height = Mathf.Max(height - 0.003f, HEIGHT_MIN);
                UpdateMask();
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                width = Mathf.Max(width - 0.003f, WIDTH_MIN);
                UpdateMask();
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                width = Mathf.Min(width + 0.003f, WIDTH_MAX);
                UpdateMask();
            }
        }

        if (Input.GetKey(KeyCode.R))
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                rotation = Mathf.Min(rotation + 0.5f, ROTATION_MAX);
                UpdateMask();
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                rotation = Mathf.Max(rotation - 0.5f, ROTATION_MIN);
                UpdateMask();
            }
        }
    }

    private void OnApplicationQuit()
    {
        isAlive = false;
    }

    // Called when values are changed in the inspector
    private void OnValidate()
    {
        if (isAlive)
        {
            UpdateMask();
        }
    }

    public void UpdateMask()
    {
        material.SetFloat("_MaskBlur", blur);
        material.SetFloat("_MaskSquareness", squareness);
        material.SetFloat("_MaskWidth", width);
        material.SetFloat("_MaskHeight", height);
        material.SetFloat("_MaskRotation", rotation);

        PlayerPrefs.SetFloat(prefsKey + ":maskBlur", blur);
        PlayerPrefs.SetFloat(prefsKey + ":maskSquareness", squareness);
        PlayerPrefs.SetFloat(prefsKey + ":maskWidth", width);
        PlayerPrefs.SetFloat(prefsKey + ":maskHeight", height);
        PlayerPrefs.SetFloat(prefsKey + ":maskRotation", rotation);
        PlayerPrefs.Save();
    }

    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        Graphics.Blit(src, dest, material);
    }

    private void Reset()
    {
        blur = BLUR_INIT;
        squareness = SQUARENESS_INIT;
        width = WIDTH_INIT;
        height = HEIGHT_INIT;
        rotation = ROTATION_INIT;
    }
}
