#!/bin/bash
set -e
case $1 in
  "start")
    if [ -e /var/run/sqlservr.pid ]; then
      echo "sqlservr is already running ? (`cat /var/run/sqlservr.pid`)" 
      exit 1
    fi
    touch /var/log/sqlservr.log
    /opt/mssql/bin/sqlservr > /var/log/sqlservr.log &
    echo "Waiting for sqlservr to start"
    tail -n +1 -f /var/log/sqlservr.log | tee /dev/stderr | sed '/SQL Server is now ready for client connections./ q'
    echo "sqlservr successfully started"
    echo $! > /var/run/sqlservr.pid
    if [ ! -z "${MSSQL_AGENT_ENABLED+x}" -a "${MSSQL_AGENT_ENABLED,,}" = "true" ]; then
      echo "Waiting for SQL Server Agent to start"
      # For an unknown reason the message is printed twice to the file, and only the second time seems accurate.
      # Also this file is UTF-16 encoded, which probably is why iconv was installed on this docker image.
      match=0
      while read line; do
        echo "$line" | iconv -f gbk -t utf8 | tee /dev/stderr | (grep -c 'SQLServerAgent service successfully started' > /dev/null) && match=$((match + 1))
        [ $match -eq 2 ] && break
      done < <(tail -n +1 -f /var/opt/mssql/log/sqlagent.out)
      echo "SQL Server Agent is ready"
    else
      echo "SQL Server Agent is not enabled"
    fi
    ;;

  "stop")
    pid=`cat /var/run/sqlservr.pid`
    kill $pid
    rm /var/run/sqlservr.pid
    rm /var/log/sqlservr.log
    ;;

  *)
    echo "Unknown command"
    echo "$0 (start|stop)"
    exit 1
esac

