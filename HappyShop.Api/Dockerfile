FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
#拷贝当前文件夹下的文件到容器中系统的工作空间
COPY . /app
#设置Docker容器端口
EXPOSE 5002
ENTRYPOINT ["dotnet", "HappyShop.Api.dll"]