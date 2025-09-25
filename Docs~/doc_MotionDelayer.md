# TH-Utils-Avatar-Unity/TH.Utils.Avatar.MotionDelayer class<!-- omit in toc -->
<img src="https://img.shields.io/badge/Unity-2021 or Later-blue?&logo=Unity"> <img src="https://img.shields.io/badge/License-MIT-green">


# Table Of Contents <!-- omit in toc -->
<details>
<summary>Details</summary>

- [Usage](#usage)
- [Definition](#definition)
- [Methods](#methods)
  - [RecordParameter(Transform)](#recordparametertransform)
    - [Parameters](#parameters)
  - [ApplyParameter(Transform)](#applyparametertransform)
    - [Parameters](#parameters-1)
  - [InitializeDictionary(Transform)](#initializedictionarytransform)
    - [Parameters](#parameters-2)
  - [InitializeDictionaryKeyValue(Transform)](#initializedictionarykeyvaluetransform)
    - [Parameters](#parameters-3)
  - [CheckCurrentTime()](#checkcurrenttime)
  - [ResetAll(Transform)](#resetalltransform)
    - [Parameters](#parameters-4)
</details>


# Usage
1. 任意のGameobjectに、MotionDelayer.csをアタッチする
2. InspectorのOriginal Rootに動きの元になるオブジェクトを、Target Rootに遅延させた動きを反映させたいオブジェクトをアタッチする (2つのオブジェクトの名前、階層構造は一致させる)
3. Delayに遅延時間 (秒) を入力する
4. 実行し、Original Rootに指定したオブジェクトを動かしたときに、遅れてTarget Rootに指定したオブジェクトが動く


# Definition
Namespace: TH.Utils.Avatar

あるGameObjectの動きを遅延させて、別のGameObjectに反映させるサンプルです。 アバターなどの階層構造を持つGameObjectも対応可能です。
任意のGameObjectにアタッチして使用します。


# Methods
<!-- -------------------------------------------------- -->
## RecordParameter(Transform)
OriginalのTransformをDictionaryに一時保存します。
**originalとtargetが同じ階層構造になっていることを想定しています**


```csharp
public void RecordParameter(Transform original)
```

### Parameters
- `original`: Transform
  - 動作の元になるオブジェクト。


<!-- -------------------------------------------------- -->
## ApplyParameter(Transform)
記録されたTransformを指定したオブジェクトに反映させます。
**originalとtargetが同じ階層構造になっていることを想定しています**


```csharp
public void ApplyParameter(Transform target)
```

### Parameters
- `target`: Transform
  - 動作を反映するオブジェクト。


<!-- -------------------------------------------------- -->
## InitializeDictionary(Transform)
動きを記録する用のDictionaryを初期化します。


```csharp
private void InitializeDictionary(Transform original)
```

### Parameters
- `original`: Transform
  - 動作の元になるオブジェクト。
  

<!-- -------------------------------------------------- -->
## InitializeDictionaryKeyValue(Transform)
動きを記録する用のDictionaryを初期化します。
引数オブジェクトの階層構造と同じ名前のKeyを持つように初期化します。


```csharp
private void InitializeDictionaryKeyValue(Transform original)
```

### Parameters
- `original`: Transform
  - 動作の元になるオブジェクト。このオブジェクトの階層構造と同じ名前のKeyを持つように初期化します。


<!-- -------------------------------------------------- -->
## CheckCurrentTime()
実行時からの経過時間を計測し、指定した遅延時間を超えた場合、遅延動作開始のフラグをtrueにします。


```csharp
private void CheckCurrentTime()
```

<!-- -------------------------------------------------- -->
## ResetAll(Transform)
経過時間、遅延動作開始のフラグ、動きを記録する用のDictionaryを初期化します。
遅延時間を実行途中で変更した場合は、このメソッドを使用して初期化します。


```csharp
public void ResetAll(Transform original)
```

### Parameters
- `original`: Transform
  - 動作の元になるオブジェクト。このオブジェクトの階層構造と同じ名前のKeyを持つようにDictionaryを初期化します。