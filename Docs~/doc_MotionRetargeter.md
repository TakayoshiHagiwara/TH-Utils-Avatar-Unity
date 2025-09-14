# TH-Utils-Avatar-Unity/TH.Utils.Avatar.MotionRetargeter class<!-- omit in toc -->
<img src="https://img.shields.io/badge/Unity-2021 or Later-blue?&logo=Unity"> <img src="https://img.shields.io/badge/License-MIT-green">


# Table Of Contents <!-- omit in toc -->
<details>
<summary>Details</summary>

- [Definition](#definition)
- [Methods](#methods)
  - [RetargetAll(Transform, Transform, string)](#retargetalltransform-transform-string)
    - [Parameters](#parameters)
</details>


# Definition
Namespace: TH.Utils.Avatar

アバターの動作を別のアバターにリターゲットします。

# Methods
<!-- -------------------------------------------------- -->
## RetargetAll(Transform, Transform, string)
指定したオブジェクトとその全ての子オブジェクトの位置と回転を別のオブジェクトに反映します。


```csharp
private void RetargetAll(Transform original, Transform target, string endName = "")
```

### Parameters
- `original`: Transform
  - 動作の元になるオブジェクト。
- `target`: Transform
  - 動作を反映するオブジェクト。
- `endName`: string
  - この名前のオブジェクトとそれ以降の子オブジェクトには動作を反映しません。
