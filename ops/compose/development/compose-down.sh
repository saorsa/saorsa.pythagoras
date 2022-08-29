#!/usr/bin/env bash

SCRIPT_DIR=$( cd -- "$( dirname -- "${BASH_SOURCE[0]}" )" &> /dev/null && pwd )
ENV_FILE="$SCRIPT_DIR/env.properties"

. "$SCRIPT_DIR/../../shell.properties"

STACK_NAME='pythagoras-dev'
DETACHED_MODE=${DETACHED_MODE:-0}

echo -e "${FG_YELLOW}Composing down..${REMOVE_ALL_FORMATTING}."
docker-compose --env-file=$ENV_FILE --file ./docker-compose.yml -p $STACK_NAME down
