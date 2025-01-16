---
title: Dockerエラー「docker.appを開くとコンピュータが破損します」[MAC]
categories: [Docker]
tags: [docker,mac]
pin: true
---

MACを再起動したら「docker.appを開くとコンピュータが破損します」エラーが出た。



## エラー内容

```bash
docker.appを開くとコンピュータが破損します
```

## 解決方法

1. `Command + Space`キーを押してSpotlightを開き、「アクティビティモニター」を起動
2. アクティビティモニターで`docker.app`プロセスを見つけて強制終了
3. 既存の`docker.app`を削除
4. ゴミ箱を空にする
5. MACを再起動
6. 新しい`docker.app`をダウンロードしてインストール
[Docker公式サイト](https://www.docker.com/ja-jp/get-started/)

## 他のエラー

```bash
com.docker.vmnetdを開くとコンピューターが破損します。ゴミ箱に入れる必要があります
```

もしdocker.appを強制終了してもこのエラーが出たら、次のコマンドを実行する。

```bash
echo "Stopping Docker..."
sudo pkill [dD]ocker

# Stop the vmnetd service
echo "Stopping com.docker.vmnetd service..."
sudo launchctl bootout system /Library/LaunchDaemons/com.docker.vmnetd.plist

# Stop the socket service
echo "Stopping com.docker.socket service..."
sudo launchctl bootout system /Library/LaunchDaemons/com.docker.socket.plist

# Remove vmnetd binary
echo "Removing com.docker.vmnetd binary..."
sudo rm -f /Library/PrivilegedHelperTools/com.docker.vmnetd

# Remove socket binary
echo "Removing com.docker.socket binary..."
sudo rm -f /Library/PrivilegedHelperTools/com.docker.socket
```

[Docker Status Page](https://www.dockerstatus.com/)
