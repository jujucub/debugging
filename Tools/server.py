# -*- coding : UTF-8 -*-

# 0.ライブラリのインポートと変数定義
import socket

server_ip = "localhost"
server_port = 5601
listen_num = 5
buffer_size = 1024

# 1.ソケットオブジェクトの作成
tcp_server = socket.socket(socket.AF_INET, socket.SOCK_STREAM)

# 2.作成したソケットオブジェクトにIPアドレスとポートを紐づける
tcp_server.bind((server_ip, server_port))

# 3.作成したオブジェクトを接続可能状態にする
tcp_server.listen(listen_num)


# 5.クライアントと接続する
client,address = tcp_server.accept()
print("[*] Connected!! [ Source : {}]".format(address))

# 4.ループして接続を待ち続ける
while True:

    # 6.データを受信する
    data = client.recv(buffer_size)
    print("[*] Received Data : {}".format(data))


# 8.接続を終了させる
client.close()
