# TH-Utils-Avatar-Unity/TH.Utils.Avatar.AvatarMotionReceiver class<!-- omit in toc -->
<img src="https://img.shields.io/badge/Unity-2021 or Later-blue?&logo=Unity"> <img src="https://img.shields.io/badge/License-MIT-green">


# Table Of Contents <!-- omit in toc -->
<details>
<summary>Details</summary>

- [Usage](#usage)
- [Definition](#definition)
- [Properties](#properties)
  - [\_targetAvatarId](#_targetavatarid)
</details>


# Usage
![AvatarMotionReceiver](https://github.com/user-attachments/assets/ac6bedd1-e395-4280-9baa-32315bcaee8d)

1. アバターモデルのGameObjectにこのスクリプトをアタッチする
    - 自動でAvatarBoneListもアタッチされます
2. `Avatar Id` に任意の値を入力する
    - アバターが複数存在する場合は、アバターごとに一意の値を入力してください
3. (まだの場合) `AvatarNetworkManager` を任意のGameObjectにアタッチする
    - Send Rateを `AvatarMotionSender` で入力した値にします
    - Player Prefabには `Network Identity` がアタッチされた任意のPrefabをアタッチします (適当なGameObejctで問題ありません)


# Definition
Namespace: TH.Utils.Avatar

Unityのアセットである[Mirror](https://assetstore.unity.com/packages/tools/network/mirror-129321?locale=ja-JP&srsltid=AfmBOopF9b_Dqx55tNFr4P9S1kDw24lvzoEW0-XcG128nu_WW3pRDlru)の機能を利用して、任意のアバターの動作を別のPCからネットワーク経由で受信します。


# Properties
<!-- -------------------------------------------------- -->
## _targetAvatarId
アバターのID。
アバターが複数存在する場合は、アバターごとに一意の値を入力してください。