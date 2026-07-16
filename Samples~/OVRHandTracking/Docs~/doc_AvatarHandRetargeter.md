# TH-Utils-Avatar-Unity/TH.Utils.Avatar.AvatarHandRetargeter class<!-- omit in toc -->
<img src="https://img.shields.io/badge/Unity-2021 or Later-blue?&logo=Unity"> <img src="https://img.shields.io/badge/License-MIT-green">


# Table Of Contents <!-- omit in toc -->
<details>
<summary>Details</summary>

- [Usage](#usage)
- [Definition](#definition)
- [Properties](#properties)
  - [\_leftOVRSkeleton / \_rightOVRSkeleton](#_leftovrskeleton--_rightovrskeleton)
  - [\_avatarAnimator](#_avataranimator)
  - [\_leftHandOffsets / \_rightHandOffsets](#_lefthandoffsets--_righthandoffsets)
  - [\_applyInLateUpdate](#_applyinlateupdate)
  - [\_logInitialization](#_loginitialization)
- [Methods](#methods)
</details>


# Usage
![AvatarHandRetargeter](https://github.com/user-attachments/assets/90f454a9-e0e4-489d-a8e8-0b330c872a93)

1. 任意のGameObjectにこのスクリプトをアタッチする
2. `Left OVR Skeleton` と `Right OVR Skeleton` に対応する左右の手の `OVRSkeleton` をアタッチする
3. `Avatar Animator` に指の動きをリターゲットするアバターのAnimatorをアタッチする
4. Hand Offsetsで各指のオフセットを調整する


# Definition
Namespace: TH.Utils.Avatar

Meta Quest 3などのハンドトラッキングで取得した指の動きを、任意のアバターの指の動きにリターゲットします。

**デフォルトのままだと回転軸が対応しないため、アバターに合わせてHand Offsetsを調整する必要があります。**
**正確なリターゲットではなく、あくまで見た目がそれっぽくなるようにするためのスクリプトです。**


# Properties
<!-- -------------------------------------------------- -->
## _leftOVRSkeleton / _rightOVRSkeleton
左右の手の `OVRSkeleton` 。

## _avatarAnimator
指の動きをリターゲットするアバターのAnimator

## _leftHandOffsets / _rightHandOffsets
各指のオフセット。
すべて0の場合はオフセットをかけませんが、任意のアバターから別の任意のアバターにリターゲットすると、往々にして軸が異なるため、オフセットをかける必要があります。

Standard AssetsのEthanアバターの場合、上記画像のようにオフセットを入れると「それっぽく」見えます。

## _applyInLateUpdate
LateUpdate内で更新するかどうか。
IKなどの計算上、指の計算が確実に完了した後でこのスクリプトを実行する必要がある場合に使用します。

## _logInitialization
初期化時のログを出力するかどうか。


# Methods
外部からアクセス可能なメソッドはありません。