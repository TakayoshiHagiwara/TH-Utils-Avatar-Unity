# TH-Utils-Avatar-Unity/TH.Utils.Avatar.AvatarMotionSender class<!-- omit in toc -->
<img src="https://img.shields.io/badge/Unity-2021 or Later-blue?&logo=Unity"> <img src="https://img.shields.io/badge/License-MIT-green">


# Table Of Contents <!-- omit in toc -->
<details>
<summary>Details</summary>

- [Usage](#usage)
- [Definition](#definition)
- [Properties](#properties)
  - [\_avatarId](#_avatarid)
  - [\_sendRate](#_sendrate)
</details>


# Usage
![AvatarMotionSender](https://github.com/user-attachments/assets/8ad233ae-a77c-4145-b5b7-0501cccd35d7)

1. アバターモデルのGameObjectにこのスクリプトをアタッチする
    - 自動でAvatarBoneListもアタッチされます
2. `Avatar Id` に任意の値を入力する
    - アバターが複数存在する場合は、アバターごとに一意の値を入力してください
3. `Send Rate` に任意の送信レートを入力する
4. (まだの場合) `AvatarNetworkManager` を任意のGameObjectにアタッチする
    - Send Rateを3で入力した値にします
    - Player Prefabには `Network Identity` がアタッチされた任意のPrefabをアタッチします (適当なGameObejctで問題ありません)


# Definition
Namespace: TH.Utils.Avatar

Unityのアセットである[Mirror](https://assetstore.unity.com/packages/tools/network/mirror-129321?locale=ja-JP&srsltid=AfmBOopF9b_Dqx55tNFr4P9S1kDw24lvzoEW0-XcG128nu_WW3pRDlru)の機能を利用して、任意のアバターの動作を別のPCにネットワーク経由で送信します。


# Properties
<!-- -------------------------------------------------- -->
## _avatarId
アバターのID。
アバターが複数存在する場合は、アバターごとに一意の値を入力してください

## _sendRate
1秒間に送信する最大回数。
Unityのフレームレートは超えません。