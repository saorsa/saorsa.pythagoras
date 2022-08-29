#!/usr/bin/env bash

SCRIPT_DIR="$( cd -- "$( dirname -- "${BASH_SOURCE[0]}" )" &> /dev/null && pwd )"

. "$SCRIPT_DIR/shell.properties"

ENV_NAME=${1:-development}
ENV_FILE="$SCRIPT_DIR/compose/$ENV_NAME/env.properties"

function read_env_setting_or_die {

    if [ ! -f "$ENV_FILE" ]; then
        echo -e "${FG_RED}Fatal (17): Environment file '$ENV_FILE' does not exist.${REMOVE_ALL_FORMATTING}"
        exit 17
    fi
    
    echo -e "${FG_BLUE}Reading settings from '$ENV_FILE'...${REMOVE_ALL_FORMATTING}"
        
    . $ENV_FILE
    source $ENV_FILE
    cut -d= -f1 $ENV_FILE
    export $(cut -d= -f1 $ENV_FILE)
    cat $ENV_FILE
}

function run_docker_compose_or_die {

    local COMPOSE_SCRIPT="${SCRIPT_DIR}/compose/${ENV_NAME}/compose-up.sh"

    if [ ! -f "$COMPOSE_SCRIPT" ]; then
        echo -e "${FG_RED}Fatal (18): Compose file '$COMPOSE_SCRIPT' does not exist.${REMOVE_ALL_FORMATTING}"
        exit 18
    fi


    echo -e "${FG_BLUE}Composing with '$COMPOSE_SCRIPT'...${REMOVE_ALL_FORMATTING}"
    eval "${COMPOSE_SCRIPT} -d"

    local COMPOSE_EXIT_CODE=$?

    if [ "$COMPOSE_EXIT_CODE" -ne "0" ]; then
        echo -e "${FG_RED}Fatal ($COMPOSE_EXIT_CODE): Compose has failed.${REMOVE_ALL_FORMATTING}"
        exit $COMPOSE_EXIT_CODE
    fi
}

read_env_setting_or_die

run_docker_compose_or_die
