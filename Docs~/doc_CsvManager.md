# TH-Utils-Avatar-Unity/TH.Utils.Avatar.CsvManager class<!-- omit in toc -->
<img src="https://img.shields.io/badge/Unity-2021 or Later-blue?&logo=Unity"> <img src="https://img.shields.io/badge/License-MIT-green">


# Table Of Contents <!-- omit in toc -->
<details>
<summary>Details</summary>

- [Definition](#definition)
- [Properties](#properties)
  - [MotionDataHeader (private)](#motiondataheader-private)
- [Methods](#methods)
  - [WriteMotionData(AvatarMotionData, string, string)](#writemotiondataavatarmotiondata-string-string)
    - [Parameters](#parameters)
    - [Returns](#returns)
  - [WriteMotionDataAsync(AvatarMotionData, string, string, CancellationToken)](#writemotiondataasyncavatarmotiondata-string-string-cancellationtoken)
    - [Parameters](#parameters-1)
    - [Returns](#returns-1)
  - [ReadMotionData(IReadOnlyList, string, string)](#readmotiondataireadonlylist-string-string)
    - [Parameters](#parameters-2)
    - [Returns](#returns-2)
</details>


# Definition
Namespace: TH.Utils.Avatar

アバターの動作データのCSVを読み書きするための静的メソッドを提供します。


# Properties
<!-- -------------------------------------------------- -->
## MotionDataHeader (private)
CSVのヘッダーを指定します。
将来的に出力するデータ形式を変更する場合、この値を修正することになります。


# Methods
<!-- -------------------------------------------------- -->
## WriteMotionData(AvatarMotionData, string, string)
`AvatarMotionData` 型のデータをCSVファイルに出力します。


```csharp
public static string WriteMotionData(AvatarMotionData motionData, string dataPath, string fileName)
```

### Parameters
- `motionData`: AvatarMotionData
  - 動作データ。

- `dataPath`: string
  - 動作データを出力するフォルダのパス。
  - 絶対パスではなく、`Application.dataPath` からのパスを指定します。

- `fileName`: string
  - ファイル名。
  - `.csv` などの拡張子は付いていてもいなくても、どちらでも大丈夫です。
    

### Returns
- string
  - 書き出したCSVファイルの絶対パス。

<!-- -------------------------------------------------- -->
## WriteMotionDataAsync(AvatarMotionData, string, string, CancellationToken)
`AvatarMotionData` 型のデータをCSVファイルに出力します。
このメソッドはメインスレッドをブロックしません。

```csharp
public async static ValueTask<string> WriteMotionDataAsync(AvatarMotionData motionData, string dataPath, string fileName, CancellationToken token)
```

### Parameters
- `motionData`: AvatarMotionData
  - 動作データ。

- `dataPath`: string
  - 動作データを出力するフォルダのパス。
  - 絶対パスではなく、`Application.dataPath` からのパスを指定します。

- `fileName`: string
  - ファイル名。
  - `.csv` などの拡張子は付いていてもいなくても、どちらでも大丈夫です。

- `token`: CancellationToken
  - 操作を取り消す際に使用します。

### Returns
- string
  - 書き出したCSVファイルの絶対パス。

<!-- -------------------------------------------------- -->
## ReadMotionData(IReadOnlyList<Transform>, string, string)
指定したCSVファイルを `AvatarMotionData` 型のデータとして読み込みます。


```csharp
public static AvatarMotionData ReadMotionData(IReadOnlyList<Transform> bones, string dataPath, string fileName)
```

### Parameters
- `bones`: IReadOnlyList\<Transform\>
  - アバターのボーンリスト。
  - 読み込むデータと同じ構造にしてください。

- `dataPath`: string
  - 動作データを読み込むフォルダのパス。
  - 絶対パスではなく、`Application.dataPath` からのパスを指定します。

- `fileName`: string
  - ファイル名。
  - `.csv` などの拡張子は付いていてもいなくても、どちらでも大丈夫です。
    

### Returns
- AvatarMotionData
  - `AvatarMotionData` 型として読み込んだデータ。