FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
#拷贝当前文件夹下的文件到容器中系统的工作空间
COPY . /app
#设置Docker容器端口
EXPOSE 9010
ENTRYPOINT ["dotnet", "HappyShop.Api.dll"]


#FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
#WORKDIR /app
#EXPOSE 80
#EXPOSE 443
#
#FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
#WORKDIR /src
#COPY ["HappyShop.Api/HappyShop.Api.csproj", "HappyShop.Api/"]
#COPY ["HappyShop.Abstractions/HappyShop.Abstractions.csproj", "HappyShop.Abstractions/"]
#COPY ["HappyShop.Core/HappyShop.Core.csproj", "HappyShop.Core/"]
#COPY ["HappyShop.Data/HappyShop.Data.csproj", "HappyShop.Data/"]
#COPY ["HappyShop.Service/HappyShop.Service.csproj", "HappyShop.Service/"]
#COPY ["HappyShop.SQLData/HappyShop.SQLData.csproj", "HappyShop.SQLData/"]
#RUN dotnet nuget add source --name appdev https://appdev.emoney.cn/nexusport/repository/nuget/
#RUN dotnet restore "HappyShop.Api/HappyShop.Api.csproj"
#COPY . .
#WORKDIR "/src/HappyShop.Api/HappyShop.Api"
#RUN dotnet build "HappyShop.Api.csproj" -c Release -o /app/build
#
#FROM build AS publish
#RUN dotnet publish "HappyShop.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false
#
#FROM base AS final
#ENV TZ=Asia/Shanghai
#ENV ASPNETCORE_HTTP_PORTS 80
#RUN sed -i '/\[openssl_init\]/a ssl_conf = ssl_sect' /etc/ssl/openssl.cnf
#RUN printf "\n[ssl_sect]\nsystem_default = system_default_sect\n" >> /etc/ssl/openssl.cnf
#RUN printf "\n[system_default_sect]\nMinProtocol = TLSv1.2\nCipherString = DEFAULT@SECLEVEL=0" >> /etc/ssl/openssl.cnf
#WORKDIR /app
#COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "HappyShop.Api.dll"]