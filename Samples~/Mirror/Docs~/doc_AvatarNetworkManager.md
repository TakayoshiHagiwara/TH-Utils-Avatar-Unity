# TH-Utils-Avatar-Unity/TH.Utils.Avatar.AvatarNetworkManager class<!-- omit in toc -->
<img src="https://img.shields.io/badge/Unity-2021 or Later-blue?&logo=Unity"> <img src="https://img.shields.io/badge/License-MIT-green">


# Table Of Contents <!-- omit in toc -->
<details>
<summary>Details</summary>

- [Usage](#usage)
- [Definition](#definition)
</details>


# Usage

1. 任意のGameObjectにこのスクリプトをアタッチする
    - 自動でAvatarBoneListもアタッチされます
2. `Send Rate` に任意の送信レートを入力する
3. Player Prefabに `Network Identity` がアタッチされた任意のPrefabをアタッチする (適当なGameObejctで問題ありません)
4. (まだの場合) `Kcp Transport` を任意のGameObjectにアタッチする
5. (まだの場合、Optional) `Network Manager HUD` を任意のGameObjectにアタッチする
    - GUIからIPアドレスとポートの指定や、接続の開始/停止を制御できます

# Definition
Namespace: TH.Utils.Avatar

Mirrorの `NetworkManager` を継承し、アバター動作の送受信用に修正したスクリプトです。