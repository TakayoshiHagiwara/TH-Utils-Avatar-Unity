# TH-Utils-Avatar-Unity/TH.Utils.Avatar.AvatarMotionMessage class<!-- omit in toc -->
<img src="https://img.shields.io/badge/Unity-2021 or Later-blue?&logo=Unity"> <img src="https://img.shields.io/badge/License-MIT-green">


# Table Of Contents <!-- omit in toc -->
<details>
<summary>Details</summary>

- [Definition](#definition)
- [Properties](#properties)
  - [AvatarId](#avatarid)
  - [Sequence](#sequence)
  - [LocalPositions / LocalRotations](#localpositions--localrotations)
</details>


# Definition
Namespace: TH.Utils.Avatar

Mirrorによる通信でやりとりするデータの構造を定義します。



# Properties
<!-- -------------------------------------------------- -->
## AvatarId
複数のアバターが存在する場合、一意に決定するためのIDです。

## Sequence
送信される各モーションメッセージに割り当てられるシーケンス番号です。
送信されるたびに1ずつ増加します。
受信側はこれを利用して、古いメッセージや順序が乱れたメッセージを検出します。

## LocalPositions / LocalRotations
送信するローカル座標。