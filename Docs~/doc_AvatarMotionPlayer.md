# TH-Utils-Avatar-Unity/TH.Utils.Avatar.AvatarMotionPlayer class<!-- omit in toc -->
<img src="https://img.shields.io/badge/Unity-2021 or Later-blue?&logo=Unity"> <img src="https://img.shields.io/badge/License-MIT-green">


# Table Of Contents <!-- omit in toc -->
<details>
<summary>Details</summary>

- [Usage](#usage)
- [Definition](#definition)
- [Properties](#properties)
  - [\_startPlayingKey / \_stopPlayingKey](#_startplayingkey--_stopplayingkey)
  - [\_loop](#_loop)
  - [\_playbackSpeed](#_playbackspeed)
  - [\_dataPath](#_datapath)
  - [\_fileName](#_filename)
  - [IsPlaying](#isplaying)
- [Methods](#methods)
  - [ApplyFrame(int)](#applyframeint)
    - [Parameters](#parameters)
  - [StartPlaying()](#startplaying)
  - [StopPlaying()](#stopplaying)
  - [LoadMotionData()](#loadmotiondata)
  - [GetMotionDataCopy()](#getmotiondatacopy)
  - [SetMotionData(AvatarMotionData)](#setmotiondataavatarmotiondata)
</details>


# Usage
![AvatarMotionPlayer](https://github.com/user-attachments/assets/ab627a7c-5128-4dce-82da-8a0e844bbbad)

**必ず記録データと同じ `Time.fixedDeltaTime` で実行してください。また、記録データの構造は `AvatarMotionRecorder.cs` で記録したものを同じにしてください。**

記録データと同じ`Time.fixedDeltaTime` にするには、Edit -> Project Settings -> TimeのFixed Timestepで同じ値を設定してください。

また、実行時に記録データを一度自動で読み込んだ後にパスやファイル名を変更しても、反映されません。
実行前に指定するか、コンテキストメニューの「Load Motion Data」を実行するか、LoadMotionData()をほかのスクリプトから呼び出してください。

1. アバターモデルのGameObjectにこのスクリプトをアタッチする
    - 自動でAvatarBoneListもアタッチされます
2. Data Pathに記録データの保存されているパス、File Nameに記録データのファイル名を指定する
3. Start Playing Keyに任意の再生キー、Stop Playing Keyに任意の停止キーを設定する
4. (Optional) ループさせる場合はLoopにチェックを入れる
    - デフォルトではデータの末尾まで再生すると、自動で停止します
5. (Optional) Playback Speedに任意の再生速度を指定することができます
    - 0を指定すると動きません
    - 1以外を指定した場合、正確にこの速度で再生される保証はありません
    - あくまで「XX倍速に見える」ようにする程度なので、より正確な制御が必要な場合は別途手法を検討してください


# Definition
Namespace: TH.Utils.Avatar

事前に `AvatarMotionRecorder` で記録したCSVデータを読み込み、アバターの動作に反映します。
以下の注意点をご確認ください。

- 記録データと同じ `Time.fixedDeltaTime` で実行する
- 記録データと同じ `AvatarBoneList.Bones` の個数にする
- 記録データの構造は `AvatarMotionRecorder.cs` で記録したCSVと同じにする
- 記録時と同じ階層構造のアバターを使用する


# Properties
<!-- -------------------------------------------------- -->
## _startPlayingKey / _stopPlayingKey
動作データの再生 / 停止を制御するキー。

## _loop
データをループ再生するかどうか。

## _playbackSpeed
再生速度。
1以外を指定した場合、正確にこの速度で再生される保証はありません。
あくまで「XX倍速に見える」ようにする程度なので、より正確な制御が必要な場合は別途手法を検討してください。

## _dataPath
データが保存されているフォルダのパス。

## _fileName
データのファイル名。

## IsPlaying
再生中かどうか。


# Methods
<!-- -------------------------------------------------- -->
## ApplyFrame(int)
アバターのLocalPositionとLocalRotationに指定したフレームの値を反映します。


```csharp
private void ApplyFrame(int frameIndex)
```

### Parameters
- `frameIndex`: int
  - 反映するフレーム番号。
  
<!-- -------------------------------------------------- -->
## StartPlaying()
再生を開始します。


```csharp
public void StartPlaying()
```

<!-- -------------------------------------------------- -->
## StopPlaying()
再生を停止します。


```csharp
public void StopPlaying()
```

<!-- -------------------------------------------------- -->
## LoadMotionData()
動作データを読み込みます。
実行中にパスやファイル名を変更した場合は、このメソッドを呼び出してください。


```csharp
public void LoadMotionData()
```

<!-- -------------------------------------------------- -->
## GetMotionDataCopy()
読み込んだモーションデータのコピーを返します。
外部スクリプトでモーションデータを編集したいときに使用します。

```csharp
public AvatarMotionData GetMotionDataCopy()
```

<!-- -------------------------------------------------- -->
## SetMotionData(AvatarMotionData)
再生するモーションデータを設定します。
外部スクリプトで編集したモーションデータを設定する際などに使用します。

```csharp
public void SetMotionData(AvatarMotionData motionData)
```