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
  - [AvatarBoneList class](#avatarbonelist-class)
    - [Definition](#definition-2)
    - [Methods](#methods-2)
  - [AvatarMotionData class](#avatarmotiondata-class)
    - [Definition](#definition-3)
    - [Methods](#methods-3)
  - [AvatarMotionPlayer class](#avatarmotionplayer-class)
    - [Definition](#definition-4)
    - [Methods](#methods-4)
  - [AvatarMotionRecorder class](#avatarmotionrecorder-class)
    - [Definition](#definition-5)
    - [Methods](#methods-5)
  - [CsvManager class](#csvmanager-class)
    - [Definition](#definition-6)
    - [Methods](#methods-6)
- [Description (Samples)](#description-samples)
  - [OVR Hand Tracking Sample](#ovr-hand-tracking-sample)
    - [AvatarHandRetargeter class](#avatarhandretargeter-class)
  - [Mirror Sample](#mirror-sample)
    - [AvatarMotionMessage class](#avatarmotionmessage-class)
    - [AvatarMotionReceiver class](#avatarmotionreceiver-class)
    - [AvatarMotionSender class](#avatarmotionsender-class)
    - [AvatarNetworkManager class](#avatarnetworkmanager-class)
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


<!-- -------------------------------------------------- -->
## AvatarBoneList class
### Definition
- Namespace: TH.Utils.Avatar

Humanoidアバターのボーン情報を保持します。

[詳細](Docs~/doc_AvatarBoneList.md)

### Methods
| Name | Summary |
| ---- | ---- |
| [RefreshBones()](/Docs~/doc_AvatarBoneList.md#refreshbones) | _bonesリストをリフレッシュします。 |


<!-- -------------------------------------------------- -->
## AvatarMotionData class
### Definition
- Namespace: TH.Utils.Avatar

アバターの動作データを記録するためのクラスです。

[詳細](Docs~/doc_AvatarMotionData.md)

### Methods
| Name | Summary |
| ---- | ---- |
| [Clear()](/Docs~/doc_AvatarMotionData.md#clear) | Times、Positions、Rotationsのリストを初期化します。 |


<!-- -------------------------------------------------- -->
## AvatarMotionPlayer class
### Definition
- Namespace: TH.Utils.Avatar

事前に `AvatarMotionRecorder` で記録したCSVデータを読み込み、アバターの動作に反映します。
以下の注意点をご確認ください。

- 記録データと同じ `Time.fixedDeltaTime` で実行する
- 記録データと同じ `AvatarBoneList.Bones` の個数にする
- 記録データの構造は `AvatarMotionRecorder.cs` で記録したCSVと同じにする
- 記録時と同じ階層構造のアバターを使用する

[詳細](Docs~/doc_AvatarMotionPlayer.md)

### Methods
| Name | Summary |
| ---- | ---- |
| [ApplyFrame(int)](/Docs~/doc_AvatarMotionPlayer.md#applyframeint) | アバターのLocalPositionとLocalRotationに指定したフレームの値を反映します。 |
| [StartPlaying()](/Docs~/doc_AvatarMotionPlayer.md#startplaying) | 再生を開始します。 |
| [StopPlaying()](/Docs~/doc_AvatarMotionPlayer.md#stopplaying) | 再生を停止します。 |
| [LoadMotionData()](/Docs~/doc_AvatarMotionPlayer.md#loadmotiondata) | 動作データを読み込みます。 実行中にパスやファイル名を変更した場合は、このメソッドを呼び出してください。 |


<!-- -------------------------------------------------- -->
## AvatarMotionRecorder class
### Definition
- Namespace: TH.Utils.Avatar

AvatarBoneListのBonesに指定されたアバターのボーンの動作を記録します。

[詳細](Docs~/doc_AvatarMotionRecorder.md)

### Methods
| Name | Summary |
| ---- | ---- |
| [StartRecording()](/Docs~/doc_AvatarMotionRecorder.md#startrecording) | 動作記録を開始します。 |
| [StopRecording()](/Docs~/doc_AvatarMotionRecorder.md#stoprecording) | 動作記録を停止します。 |
| [SaveMotionData()](/Docs~/doc_AvatarMotionRecorder.md#savemotiondata) | 記録したデータを書き出します。 メインスレッドをブロックするため、記録時間が長い場合は一時的にUnityがフリーズします。 |
| [SaveMotionDataAsync()](/Docs~/doc_AvatarMotionRecorder.md#savemotiondataasync) | 記録したデータを書き出します。 このメソッドはメインスレッドをブロックしません。 |
| [InitializeMotionData()](/Docs~/doc_AvatarMotionRecorder.md#initializemotiondata) | 動作データのインスタンスを初期化します。 |


<!-- -------------------------------------------------- -->
## CsvManager class
### Definition
- Namespace: TH.Utils.Avatar

アバターの動作データのCSVを読み書きするための静的メソッドを提供します。

[詳細](Docs~/doc_CsvManager.md)

### Methods
| Name | Summary |
| ---- | ---- |
| [WriteMotionData(AvatarMotionData, string, string)](/Docs~/doc_CsvManager.md#writemotiondataavatarmotiondata-string-string) | `AvatarMotionData` 型のデータをCSVファイルに出力します。 |
| [WriteMotionDataAsync(AvatarMotionData, string, string, CancellationToken)](/Docs~/doc_CsvManager.md#writemotiondataasyncavatarmotiondata-string-string-cancellationtoken) | `AvatarMotionData` 型のデータをCSVファイルに出力します。このメソッドはメインスレッドをブロックしません。 |
| [ReadMotionData(IReadOnlyList, string, string)](/Docs~/doc_CsvManager.md#readmotiondataireadonlylist-string-string) | 指定したCSVファイルを `AvatarMotionData` 型のデータとして読み込みます。 |






# Description (Samples)
<!-- -------------------------------------------------- -->
## OVR Hand Tracking Sample
Meta Quest 3などのハンドトラッキングで取得した手のデータに対するサンプルを提供します。

### AvatarHandRetargeter class
- Namespace: TH.Utils.Avatar

Meta Quest 3などのハンドトラッキングで取得した指の動きを、任意のアバターの指の動きにリターゲットするサンプルです。

[詳細](Samples~/OVRHandTracking/Docs~/doc_AvatarHandRetargeter.md)


<!-- -------------------------------------------------- -->
## Mirror Sample
Unityのアセットである[Mirror](https://assetstore.unity.com/packages/tools/network/mirror-129321?locale=ja-JP&srsltid=AfmBOopF9b_Dqx55tNFr4P9S1kDw24lvzoEW0-XcG128nu_WW3pRDlru)の機能を利用して、任意のアバターの動作をPC間で同期するサンプルを提供します。

### AvatarMotionMessage class
- Namespace: TH.Utils.Avatar

Mirrorによる通信でやりとりするデータの構造を定義します。

[詳細](Samples~/Mirror/Docs~/doc_AvatarMotionMessage.md)


<!-- -------------------------------------------------- -->
### AvatarMotionReceiver class
- Namespace: TH.Utils.Avatar

任意のアバターの動作を別のPCからネットワーク経由で受信します。

[詳細](Samples~/Mirror/Docs~/doc_AvatarMotionReceiver.md)


<!-- -------------------------------------------------- -->
### AvatarMotionSender class
- Namespace: TH.Utils.Avatar

任意のアバターの動作を別のPCにネットワーク経由で送信します。

[詳細](Samples~/Mirror/Docs~/doc_AvatarMotionSender.md)


<!-- -------------------------------------------------- -->
### AvatarNetworkManager class
- Namespace: TH.Utils.Avatar

Mirrorの `NetworkManager` を継承し、アバター動作の送受信用に修正したスクリプトです。

[詳細](Samples~/Mirror/Docs~/doc_AvatarNetworkManager.md)


# References


# Troubleshooting


# Versions
- [CHANGELOG](/CHANGELOG.md)


# Author
- Takayoshi Hagiwara
    - Nagoya Institute of Technology


# License
- MIT License