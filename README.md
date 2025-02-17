# Portfolio

このポートフォリオでは、私の開発スキルとプロジェクト経験を紹介しています。
主にUnityを活用したゲーム開発や機械学習を取り入れた研究プロジェクトを通じて学んだ内容を公開しています。

## 内容
- **ビルド済みアプリケーション (.exe)**: Windowsで動作するアプリケーション
- **アプリケーション解説動画**: 各プロジェクトの特徴や機能を説明
- **C#スクリプト**: 自作したコードの一部を公開
- **解説テキスト**: プロジェクトの目的や技術的背景の詳細
- **制作期間**: Tetris, Hockey3D 合計1か月
---

## プロジェクト一覧

### プロジェクト1: **Tetris**
- **目的**: ゲームの動作やロジック構築を練習するため、テトリスを制作
- **使用技術**: 
  - 16bitのビット演算でブロック情報を管理
  - UI設計とスクリプトによるゲームロジック制御
- **主な機能**:
  - テトリミノの操作、回転、削除処理
  - ゲーム進行を管理するシステム
- **困難だった点と解決策**:
  - 回転時の衝突判定の実装が難しかった
    → ブロックの衝突処理を予測し、回転時の補正計算を適用することで解決
- **今後の計画**:
  - Tスピン対応の回転補正
  - UIの改善と得点システムの拡張

---

### プロジェクト2: **Hockey3D**
- **目的**: Unityの基礎技術を学ぶ
- **説明**: カプセル型フィールドでエアホッケーを行うe-Sports風ゲーム
- **使用技術**: Unity API、RigidBody制御、アニメーション管理
- **主な機能**:
  - ゴール判定や得点計算
  - ボールの挙動制御（反射や衝突のロジック）
  - プレイヤーキャラ操作とアニメーション
- **困難だった点と解決策**:
  - ボールの物理挙動の調整
  → 重力を停止、反発係数を調節
  - フィールドのカプセル型オブジェクトの内側に衝突可能コリジョンを実装
  → コリジョンの表裏を逆にする機能を追加することで解決
- **今後の計画**:
  - NPCの導入やルール追加
  - VR対応と観戦システムの構築
  - フィールド表面上で移動可能（表面に重力空間オブジェクトを配置）
- **備考**: アプリケーションは大容量でアップロードが不可能なため非公開

---

### プロジェクト3: **AircraftTrain**
- **目的**: 航空機の飛行シミュレーションを研究で活用
- **説明**: 
  - 成功例と失敗例
    - 成功例（識別制度78%）: Unityで飛行挙動を再現し、撮影した映像から機械学習用画像データを生成
    - 失敗例（識別制度26%）: Unityで多種多様な飛行角度、機体色、背景色などの条件から機械学習用画像データを生成
- **使用技術**:
  - Unityでの環境再現
  - 魚眼レンズを活用した視覚効果
  - Unityでの航空機の機動制御
- **主な機能**:
  - 厚木飛行場での飛行再現
  - 離着陸の挙動再現
- **困難だった点と解決策**:
  - 機種識別精度の向上
  → 多角的な航空機の画像生成から現実の識別方法に則した飛行シミュレーションに変更
  - 識別時の効率化
  → 識別対象の航空機映像の雲等のノイズ要素の除去をセマンティックセグメンテーションによる画像加工によって実現
- **備考**: Unityの実験プロジェクト(.exe)、Pythonプログラム（動体物検知、機械学習）は研究上の理由で非公開

---

## インストール方法

1. リポジトリをクローン:
   ```bash
   git clone https://github.com/NoriCreator/Portfolio.git
   cd Portfolio

2. アプリケーションの実行:
    Windows: build/ディレクトリ内の .exe を実行

お問い合わせ
メール: itowokashi.nori@gmail.com
GitHub: [NoriCreator](https://github.com/NoriCreator)