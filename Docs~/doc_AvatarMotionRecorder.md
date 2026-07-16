# TH-Utils-Avatar-Unity/TH.Utils.Avatar.AvatarMotionRecorder class<!-- omit in toc -->
<img src="https://img.shields.io/badge/Unity-2021 or Later-blue?&logo=Unity"> <img src="https://img.shields.io/badge/License-MIT-green">


# Table Of Contents <!-- omit in toc -->
<details>
<summary>Details</summary>

- [Usage](#usage)
- [Definition](#definition)
  - [記録レートについて](#記録レートについて)
- [Properties](#properties)
  - [\_startRecordingKey / \_stopRecordingKey](#_startrecordingkey--_stoprecordingkey)
  - [\_maximumRecordingSeconds](#_maximumrecordingseconds)
  - [\_dataPath](#_datapath)
  - [\_fileName](#_filename)
  - [IsRecording](#isrecording)
- [Methods](#methods)
  - [StartRecording()](#startrecording)
  - [StopRecording()](#stoprecording)
  - [SaveMotionData()](#savemotiondata)
  - [SaveMotionDataAsync()](#savemotiondataasync)
  - [InitializeMotionData()](#initializemotiondata)
</details>


# Usage
![AvatarMotionRecorder](https://github.com/user-attachments/assets/b13db2c2-621b-4bff-b8e1-58f259e7c372)

1. アバターモデルのGameObjectにこのスクリプトをアタッチする
    - 自動でAvatarBoneListもアタッチされます
2. Data Pathに記録データを保存するパス、File Nameに記録データのファイル名を指定する
3. Start Recording Keyに任意の記録開始キー、Stop Recording Keyに任意の記録停止キーを設定する
4. (Optional) Maximum Recording Secondsに記録する最大時間を指定する
    - デフォルトでは300秒です
    - この値を超える時間記録をしても問題はありませんが、パフォーマンスが低下する可能性があります
    - 事前に記録時間がわかっていれば指定することをおすすめします


# Definition
Namespace: TH.Utils.Avatar

AvatarBoneListのBonesに指定されたアバターのボーンの動作を記録します。

## 記録レートについて
Edit -> Project Settings -> TimeのFixed Timestepに依存します。

またはスクリプトで `Time.fixedDeltaTime` を指定することも可能です。


# Properties
<!-- -------------------------------------------------- -->
## _startRecordingKey / _stopRecordingKey
動作データの記録開始 / 停止を制御するキー。

## _maximumRecordingSeconds
記録する最大時間。デフォルトでは300秒です。
この値を超える時間記録をしても問題はありませんが、パフォーマンスが低下する可能性があります。
事前に記録時間がわかっていれば指定することをおすすめします。

## _dataPath
データが保存されているフォルダのパス。

## _fileName
データのファイル名。

## IsRecording
記録中かどうか。


# Methods
<!-- -------------------------------------------------- -->
## StartRecording()
動作記録を開始します。


```csharp
public void StartRecording()
```
  
<!-- -------------------------------------------------- -->
## StopRecording()
動作記録を停止します。


```csharp
public void StopRecording()
```

<!-- -------------------------------------------------- -->
## SaveMotionData()
記録したデータを書き出します。
メインスレッドをブロックするため、記録時間が長い場合は一時的にUnityがフリーズします。


```csharp
public void SaveMotionData()
```

<!-- -------------------------------------------------- -->
## SaveMotionDataAsync()
記録したデータを書き出します。
このメソッドはメインスレッドをブロックしません。


```csharp
public async ValueTask SaveMotionDataAsync()
```

<!-- -------------------------------------------------- -->
## InitializeMotionData()
動作データのインスタンスを初期化します。
このメソッドは初期化時に一度呼ぶだけで問題ないです。
記録されているデータのみ初期化する場合には、 `AvatarMotionData` クラスの `Clear` メソッドを呼びます。


```csharp
private void InitializeMotionData()
```