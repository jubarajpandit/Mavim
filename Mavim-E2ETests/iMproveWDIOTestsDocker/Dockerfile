FROM node:12-alpine
LABEL maintainer="Labs42"

RUN apk update && apk add bash

WORKDIR /app
COPY . /app/

RUN echo '---------------------working-directory---------------------'
RUN ls -lha /app
RUN echo '---------------------------------------------------------------'

RUN chmod +x ./run_full.sh

ENTRYPOINT [ "./run_full.sh" ]