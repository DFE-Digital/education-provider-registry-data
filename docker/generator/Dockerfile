FROM mcr.microsoft.com/dotnet/sdk:10.0

# RUN addgroup --gid 1001 app \
#  && adduser \
#       --uid 1001 \
#       --gid 1001 \
#       --disabled-password \
#       app

# USER app

# ENV HOME=/home/app
# ENV DOTNET_CLI_HOME=/home/app

RUN apt-get update \
    && apt-get install -y postgresql-client

WORKDIR /src

