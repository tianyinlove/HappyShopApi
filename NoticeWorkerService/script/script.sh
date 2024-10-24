#!/bin/bash

# 定义日志文件路径
LOG_FILE="/home/service/happyshop/log/logfile.log"

while true; do
    # 发送 HTTP GET 请求并捕获响应状态码
    HTTP_RESPONSE=$(curl -s -o /dev/null -w "%{http_code}" -X GET "http://192.168.3.124:9010/MyFollow/SendMessage")
    
    # 获取当前时间戳
    TIMESTAMP=$(date +"%Y-%m-%d %H:%M:%S")
    
    # 检查 HTTP 响应状态码
    if [ "$HTTP_RESPONSE" -eq 200 ]; then
        echo "$TIMESTAMP - Request successful" >> $LOG_FILE
    else
        echo "$TIMESTAMP - Request failed with status code: $HTTP_RESPONSE" >> $LOG_FILE
    fi
    
    # 等待 1 秒
    sleep 1
done