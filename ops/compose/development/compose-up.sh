#!/usr/bin/env bash

SCRIPT_DIR=$( cd -- "$( dirname -- "${BASH_SOURCE[0]}" )" &> /dev/null && pwd )
ENV_FILE="$SCRIPT_DIR/env.properties"

. "$SCRIPT_DIR/../../shell.properties"

for i in "$@"
do
case $i in
    -d)
    DETACHED_MODE=1
    shift # past argument with no value
    ;;
    *)
          # unknown option
    ;;
esac
done

STACK_NAME='pythagoras-dev'
DETACHED_MODE=${DETACHED_MODE:-0}

if [ $DETACHED_MODE -eq 0 ]; then
    echo -e "${FG_BLUE}Starting in non-detached mode..${REMOVE_ALL_FORMATTING}."
    docker-compose --env-file=$ENV_FILE --file "$SCRIPT_DIR/docker-compose.yml" -p $STACK_NAME up
else
    echo -e "${FG_BLUE}Starting in detached mode..${REMOVE_ALL_FORMATTING}."
    docker-compose --env-file=$ENV_FILE --file  "$SCRIPT_DIR/docker-compose.yml" -p $STACK_NAME up -d
fi
