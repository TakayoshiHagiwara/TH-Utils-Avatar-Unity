# TH-Utils-Avatar-Unity/TH.Utils.Avatar.AvatarMotionData class<!-- omit in toc -->
<img src="https://img.shields.io/badge/Unity-2021 or Later-blue?&logo=Unity"> <img src="https://img.shields.io/badge/License-MIT-green">


# Table Of Contents <!-- omit in toc -->
<details>
<summary>Details</summary>

- [Definition](#definition)
- [Properties](#properties)
  - [Times](#times)
  - [FrameCount](#framecount)
  - [Positions](#positions)
  - [Rotations](#rotations)
- [Constructor](#constructor)
- [Methods](#methods)
  - [Clear()](#clear)
</details>


# Definition
Namespace: TH.Utils.Avatar

アバターの動作データを記録するためのクラスです。


# Properties
<!-- -------------------------------------------------- -->
## Times
動作データの時間リスト。

## FrameCount
記録データの全フレーム数。

## Positions
座標データの辞書。ボーンの名前で参照する。

## Rotations
回転データの辞書。ボーンの名前で参照する。


# Constructor
<!-- -------------------------------------------------- -->
```csharp
public AvatarMotionData(IReadOnlyList<Transform> bones, int initialCapacity)
```
- bones: 記録対象のボーンリスト
- initialCapacity: リストの初期容量。記録するフレームレートと時間に応じて適宜設定してください。記録中に超える場合は自動的にリストが再生成されます。


# Methods
<!-- -------------------------------------------------- -->
## Clear()
Times、Positions、Rotationsのリストを初期化します。


```csharp
public void Clear()
```
