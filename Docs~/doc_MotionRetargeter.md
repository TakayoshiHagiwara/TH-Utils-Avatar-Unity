# TH-Utils-Avatar-Unity/TH.Utils.Avatar.MotionRetargeter class<!-- omit in toc -->
<img src="https://img.shields.io/badge/Unity-2021 or Later-blue?&logo=Unity"> <img src="https://img.shields.io/badge/License-MIT-green">


# Table Of Contents <!-- omit in toc -->
<details>
<summary>Details</summary>

- [Usage](#usage)
- [Definition](#definition)
- [Enum](#enum)
  - [RetargetMode](#retargetmode)
    - [AllBones](#allbones)
    - [SelectedBones](#selectedbones)
- [Methods](#methods)
  - [RetargetAll(Transform, Transform, string)](#retargetalltransform-transform-string)
    - [Parameters](#parameters)
  - [AddPositionConstraint(Transform, Transform, Vector3)](#addpositionconstrainttransform-transform-vector3)
    - [Parameters](#parameters-1)
  - [AddRotationConstraint(Transform, Transform)](#addrotationconstrainttransform-transform)
    - [Parameters](#parameters-2)
  - [AddRotationConstraint(Transform, Transform, List)](#addrotationconstrainttransform-transform-list)
    - [Parameters](#parameters-3)
</details>


# Usage
![Component](https://github.com/user-attachments/assets/16d7dc5c-8db7-4a57-a9c0-e950cca1264c)

1. 任意のGameObjectにMotionRetargeterをアタッチしてください
2. Retarget Modeを選択してください
3. Original RootとTarget Rootに、それぞれ動きの元となるアバターのroot、動きを反映するアバターのrootを指定してください
4. (AllBonesモードのみ) End Bone Nameに指定した名前のGameObject以降には動きが反映されません
5. (SelectedBonesモードのみ) Position Offsetに任意の値を入れることで、位置を調整します
6. (SelectedBonesモードのみ) Retarget Human Body Bonesリストで指定したボーンにのみ動きを反映します


# Definition
Namespace: TH.Utils.Avatar

アバターの動作を別のアバターにリターゲットします。
任意のGameObjectにアタッチして使用します。


# Enum
## RetargetMode
AllBones, SelectedBones

リターゲットモード。
AllBonesは階層構造になっている全てのオブジェクトを対象とします。
SelectedBonesは後述の指定したオブジェクトのみを対象とします。
それぞれでリターゲットの方法が異なります。

### AllBones
階層構造となっているオブジェクトを再帰で取得していき、そのすべての位置と回転をリターゲットします。
複雑な階層となるほど、計算コストが増大します。

### SelectedBones
指定したオブジェクトにPositionConstraintまたはRotationConstraintをアタッチすることで、リターゲットを実現します。
オブジェクトに直接位置や回転を与えるのではなく、上記のConstraintをアタッチするという方法を用いています。
変数`RetargetHumanBodyBones`で指定したHumanoidボーンにのみアタッチします。
そのため、リターゲット元と先の両方のオブジェクトがHumanoid Avatarになっていることを確認してください。


# Methods
<!-- -------------------------------------------------- -->
## RetargetAll(Transform, Transform, string)
指定したオブジェクトとその全ての子オブジェクトの位置と回転を別のオブジェクトに反映します。
位置と回転はローカル座標 (localPosition, localRotation) を使用します。
**originalとtargetが同じ階層構造になっていることを想定しています**


```csharp
private void RetargetAll(Transform original, Transform target, string endName = "")
```

### Parameters
- `original`: Transform
  - 動作の元になるオブジェクト。
- `target`: Transform
  - 動作を反映するオブジェクト。
- `endName`: string
  - この名前のオブジェクトとそれ以降の子オブジェクトには動作を反映しません。


<!-- -------------------------------------------------- -->
## AddPositionConstraint(Transform, Transform, Vector3)
targetオブジェクトにPositionConstraintコンポーネントをアタッチします。
rootとなるオブジェクトにのみアタッチします。引数で渡されたtargetオブジェクトにのみアタッチするため、階層構造は考慮しません。
ソースとなるオブジェクトにはoriginalを使用します。

```csharp
private void AddPositionConstraint(Transform original, Transform target, Vector3 offset)
```

### Parameters
- `original`: Transform
  - PositionConstraintコンポーネントのソースとなるオブジェクトのroot。
- `target`: Transform
  - PositionConstraintコンポーネントをアタッチするオブジェクトのroot。
- `offset`: Vector3
  - ポジションのオフセット。originalの位置からのオフセットを指定します。


<!-- -------------------------------------------------- -->
## AddRotationConstraint(Transform, Transform)
targetオブジェクトにRotationConstraintコンポーネントをアタッチします。
引数で渡されたtargetオブジェクトにのみアタッチするため、階層構造は考慮しません。
ソースとなるオブジェクトにはoriginalを使用します。

```csharp
private void AddRotationConstraint(Transform original, Transform target)
```

### Parameters
- `original`: Transform
  - RotationConstraintコンポーネントのソースとなるオブジェクト。
- `target`: Transform
  - RotationConstraintコンポーネントをアタッチするオブジェクト。


<!-- -------------------------------------------------- -->
## AddRotationConstraint(Transform, Transform, List<HumanBodyBones>)
targetオブジェクトにRotationConstraintコンポーネントをアタッチします。
bonesで指定されたボーンのオブジェクトに対してアタッチします。
ソースとなるオブジェクトにはoriginalを使用します。

```csharp
private void AddRotationConstraint(Animator original, Animator target, List<HumanBodyBones> bones)
```

### Parameters
- `original`: Transform
  - RotationConstraintコンポーネントのソースとなるオブジェクトのroot。
- `target`: Transform
  - RotationConstraintコンポーネントをアタッチするオブジェクトのroot。
- `bones`: List of HumanBodyBones
  - RotationConstraintコンポーネントをアタッチするボーンのリスト。
