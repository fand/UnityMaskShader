# UnityMaskShader

![2018-07-06 13_55_01](https://user-images.githubusercontent.com/1403842/42360534-5743042c-8124-11e8-940f-9ddc4eb040b0.gif)

Mask component for installation apps built with Unity.

You can adjust mask shape, size and blur with keyboard at runtime.
Once you've done the parameters will be saved automatically.
After that the parameters will be loaded everytime the app launches.

## Install

1. [Download latest release](https://github.com/fand/UnityMaskShader/releases).
2. Import `maskshader.unitypackage`.
3. Add `MaskController` to the main camera of the scene.
4. Set `PrefsKey` in `MaskController` inspector.

`PrefsKey` is a key for PlayerPrefs.
Put any string which doesn't conflict with other scenes in the project.

## Key bindings

- `Shift` + `↑` / `Shift` + `↓`: Blur
- `Shift` + `←` / `Shift` + `→`: Squareness
- `Ctrl` + `↑` `Ctrl` + `↓`: Height
- `Ctrl` + `←` `Ctrl` + `→`: Width
- `R` + `↑` / `R` + `↓`: Rotation

## License

MIT
