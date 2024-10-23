FROM debian:latest

# 安装 systemd 和 cron
RUN apt-get update && apt-get install -y systemd cron

# 启用 systemd
VOLUME [ "/sys/fs/cgroup" ]
CMD ["/lib/systemd/systemd"]
