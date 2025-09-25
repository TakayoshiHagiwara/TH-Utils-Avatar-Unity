# TH-Utils-Avatar-Unity<!-- omit in toc -->
<img src="https://img.shields.io/badge/Unity-2021 or Later-blue?&logo=Unity"> <img src="https://img.shields.io/badge/License-MIT-green">

Unityにおけるアバター操作関連のメソッドを扱います。

# Table Of Contents <!-- omit in toc -->
<details>
<summary>Details</summary>

- [Environment](#environment)
- [Installation](#installation)
  - [Unity Package Manager経由での導入](#unity-package-manager経由での導入)
- [Description](#description)
  - [MotionRetargeter class](#motionretargeter-class)
    - [Definition](#definition)
    - [Methods](#methods)
  - [MotionDelayer class](#motiondelayer-class)
    - [Definition](#definition-1)
    - [Methods](#methods-1)
- [References](#references)
- [Troubleshooting](#troubleshooting)
- [Versions](#versions)
- [Author](#author)
- [License](#license)
</details>


# Environment
- Unity 2021 or Later

# Installation
## Unity Package Manager経由での導入
1. Window -> Package Managerを開きます
2. 左上のプラスアイコンをクリックし、「Add package from git URL...」をクリックします
3. このリポジトリURLを入力し、addをクリックします


# Description
<!-- -------------------------------------------------- -->
## MotionRetargeter class
### Definition
- Namespace: TH.Utils.Avatar

アバターの動作を別のアバターにリターゲットするメソッドを提供します。

### Methods
| Name | Summary |
| ---- | ---- |
| [RetargetAll(Transform, Transform, string)](/Docs~/doc_MotionRetargeter.md#retargetalltransform-transform-string) | 指定したオブジェクトとその全ての子オブジェクトの位置と回転を別のオブジェクトに反映します。 |
| [AddPositionConstraint(Transform, Transform, Vector3)](/Docs~/doc_MotionRetargeter.md#addpositionconstrainttransform-transform-vector3) | targetオブジェクトにPositionConstraintコンポーネントをアタッチします。 |
| [AddRotationConstraint(Transform, Transform)](/Docs~/doc_MotionRetargeter.md#addrotationconstrainttransform-transform) | targetオブジェクトにRotationConstraintコンポーネントをアタッチします。 |
| [AddRotationConstraint(Transform, Transform, List<HumanBodyBones>)](/Docs~/doc_MotionRetargeter.md#addrotationconstrainttransform-transform-list) | targetオブジェクトにRotationConstraintコンポーネントをアタッチします。 |



<!-- -------------------------------------------------- -->
## MotionDelayer class
### Definition
- Namespace: TH.Utils.Avatar

アバターの動きに遅延をかけるメソッドを提供します。
[こちら](https://github.com/TakayoshiHagiwara/Delay-Object-cs.git)と同様です (一部変数名が異なりますが、動作は同じです)。

### Methods
| Name | Summary |
| ---- | ---- |
| [RecordParameter(Transform)](/Docs~/doc_MotionDelayer.md#recordparametertransform) | OriginalのTransformをDictionaryに一時保存します。 |
| [ApplyParameter(Transform)](/Docs~/doc_MotionDelayer.md#applyparametertransform) | 記録されたTransformを指定したオブジェクトに反映させます。 |
| [InitializeDictionary(Transform)](/Docs~/doc_MotionDelayer.md#initializedictionarytransform) | 動きを記録する用のDictionaryを初期化します。 |
| [InitializeDictionaryKeyValue(Transform)](/Docs~/doc_MotionDelayer.md#initializedictionarykeyvaluetransform) | 動きを記録する用のDictionaryを初期化します。 |
| [CheckCurrentTime()](/Docs~/doc_MotionDelayer.md#checkcurrenttime) | 実行時からの経過時間を計測し、指定した遅延時間を超えた場合、遅延動作開始のフラグをtrueにします。 |
| [ResetAll(Transform)](/Docs~/doc_MotionDelayer.md#resetalltransform) | 経過時間、遅延動作開始のフラグ、動きを記録する用のDictionaryを初期化します。 |


# References


# Troubleshooting


# Versions
- [CHANGELOG](/CHANGELOG.md)


# Author
- Takayoshi Hagiwara
    - National Institute of Technology (KOSEN), Nagano College


# License
- MIT License