FROM alpine:3.7

RUN apk update && apk upgrade && \
    apk add --no-cache bash git openssh

RUN git clone https://github.com/carlpaton/SQLStatements.git git_raw_sql && \
    mkdir -p flyway/sql && \
    cp git_raw_sql/mssql/VulnusCloud/*.sql flyway/sql/