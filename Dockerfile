FROM mcr.microsoft.com/dotnet/core/sdk:3.0

# https://docs.microsoft.com/en-us/azure/app-service/containers/configure-custom-container#enable-ssh
RUN apt-get update -y \
    && apt-get install -y --no-install-recommends dialog \ 
    && apt-get install -y --no-install-recommends openssh-server \
    && mkdir -p /run/sshd \ 
    && echo "root:Docker!" | chpasswd 

COPY sshd_config /etc/ssh/

EXPOSE 80 2222

ENTRYPOINT ["/bin/bash", "-c", "/usr/sbin/sshd"]