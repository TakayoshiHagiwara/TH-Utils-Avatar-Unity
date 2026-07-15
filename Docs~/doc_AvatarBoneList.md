# TH-Utils-Avatar-Unity/TH.Utils.Avatar.AvatarBoneList class<!-- omit in toc -->
<img src="https://img.shields.io/badge/Unity-2021 or Later-blue?&logo=Unity"> <img src="https://img.shields.io/badge/License-MIT-green">


# Table Of Contents <!-- omit in toc -->
<details>
<summary>Details</summary>

- [Usage](#usage)
- [Definition](#definition)
- [Methods](#methods)
  - [RefreshBones()](#refreshbones)
</details>


# Usage
![AvatarBoneList](https://github.com/user-attachments/assets/3adb3f12-6793-4927-81d2-b65ab1bb61ab)

このスクリプト単体では使用しません。
AvatarMotionRecorderやAvatarMotionPlayerから、このスクリプトを参照し、アバターのボーン情報を取得します。
Humanoidアバターにアタッチされることを想定しています。

1. AvatarMotionRecorderやAvatarMotionPlayerなどのスクリプトをアタッチすると、自動でこのスクリプトがアタッチされます
2. _bonesをほかのスクリプトから参照します
    - Humanoidアバターを参照して、自動でボーンをアタッチします
3. _bonesToExcludeに設定したボーンはほかのスクリプトの参照から除外します
4. _bonesToExcludeにボーンを設定した後は、コンテキストメニューの「Refresh Bones」を実行することで_bonesに反映します


# Definition
Namespace: TH.Utils.Avatar

Humanoidアバターのボーン情報を保持します。
このスクリプト単体では使用しません。


# Methods
<!-- -------------------------------------------------- -->
## RefreshBones()
_bonesリストをリフレッシュします。


```csharp
public void RefreshBones()
```
  
