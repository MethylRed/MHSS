# 使い方
## この記述は発売前(2/27)時点での情報に基づいて記述しています。

## 概要
1. 検索条件を指定
2. 検索する
3. 追加スキル検索

## 1. 検索条件を指定
### 検索回数
検索する回数を指定してください。上限は`./Models/Config/Config.csv`に記述されています(デフォルトは50)。変更したい場合は書き換えてください。

### 属性耐性
指定値以上になるように検索します。何も入力されていない場合は、属性耐性による条件はありません。

### スキル選択
理想とするスキルの組み合わせを指定します。
- スキルレベルの指定  
各スキルのコンボボックスのドロップダウンリストから要求するレベルを指定してください。
- レベル固定  
デフォルトでは、各スキルの指定値以上(上限以上にもなる)を満たす装備の組み合わせを検索します。チェックボックスにチェックを入れるとそのスキルのレベルが指定値で固定されます。

### 武器選択
装備したい武器を指定します。特定の武器を固定することも、条件に合致するものを指定することも可能です。
1. 武器種選択  
14武器種の内1つを指定します。その武器種の武器がリスト表示され、その武器種のみが検索対象になります。  
指定しない場合はすべての武器が検索対象になります。

2. 属性選択  
武器種選択と同様に、指定した属性を持つ武器が表示され、その属性を持つ武器のみが検索対象になります。  
指定しない場合は属性によるフィルタリングは解除されます。

3. スキルによるフィルタリング    
指定した武器種・属性の武器が持つスキルがリストに表示されます。  
スキルを指定するとそのスキルを持つ武器がリストに表示されます。  
スキル指定は検索対象に影響しません。

4. 武器を固定する場合  
リストに表示されている武器をクリックするとその武器が固定されます。  
`固定を解除`ボタンで固定を解除することが可能です。  
別の武器を選択することで固定を上書きできます。

### 装備固定・除外
防具・護石の固定または除外を行います。  
未実装です。

### 装飾品登録
所持している装飾品の個数を入力してください。  
アプリケーション終了時に、`./Models/Data/save/DecoCount.json`に情報が記録されます。

### 武器登録
アーティア武器は、ムフェト・ジーヴァの覚醒武器のようにステータスをある程度カスタマイズすることが可能なようです。自由にステータスを指定して武器を登録することが可能です。アプリケーション終了時に、`./Models/Data/save/CsvAddWeapon.csv`に情報が記録されます。
##### 注意点
- 名前  
検索結果を表示するにあたって、既存の装備と被らない名前の登録が必要です。

- 武器倍率  
攻撃力です。MHWでは武器倍率に、各武器種で定められた武器係数を乗じて表示しています。MHRでは武器倍率が表示されています。入力値を間違えても検索上の問題はありませんが、注意してください。

- 会心率  
%を書く必要はありません。  
  
大変申し訳ありませんが、一度登録した武器をアプリケーション上で削除する機能を実装できていません。削除したい場合は`./Models/Data/save/CsvAddWeapon.csv`内の該当する行を削除してください。

## 2. 検索する
条件を指定したら、`🔍検索`ボタンを押して検索を実行します。  
検索が終了すると、検索結果が一覧表示されます。クリックして展開すると詳細が確認可能です。`📋`ボタンを押すと表示画面がクリップボードにコピーされます。検索結果の共有や保存に活用してください。
- 装飾品の並び  
ゲーム内の装飾品装備画面のように、どの装飾品を℃のスロットに装着すればよいかガイドしています。ご活用ください。

## 3. 追加スキル検索
指定した条件に加えて追加でどのスキルをつけることができるかを検索します。  
`🔍追加スキル検索`ボタンを押すと検索を実行します。検索は、マルチスレッドで行います。環境によりますが、10秒以下、長くても30秒以内には終了します。
