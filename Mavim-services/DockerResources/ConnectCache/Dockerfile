FROM redis
COPY Mavim-services/DockerResources/ConnectCache/redis.conf /usr/local/etc/redis/redis.conf
RUN adduser --disabled-password -u 1001 serviceuser
CMD [ "redis-server", "/usr/local/etc/redis/redis.conf" ]
USER 1001