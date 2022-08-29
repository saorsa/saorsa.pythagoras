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

read_env_setting_or_die

USE_PROCESS_SECURITY=$(echo "$PYTHAGORAS_POSTGRES_USE_PROCESS_SECURITY" | tr '[:upper:]' '[:lower:]')

if [ "$USE_PROCESS_SECURITY" == "true" ]; then
    echo -e "${FG_BLUE}Connecting to database using process security...${REMOVE_ALL_FORMATTING}"
    psql --host=$PYTHAGORAS_POSTGRES_SERVER --port=$PYTHAGORAS_POSTGRES_PORT $PYTHAGORAS_POSTGRES_DATABASE
else
    echo -e "${FG_BLUE}Connecting to database with user '$PYTHAGORAS_POSTGRES_USER'...${REMOVE_ALL_FORMATTING}"
    export PGPASSWORD=$PYTHAGORAS_POSTGRES_PASSWORD
    psql --host=$PYTHAGORAS_POSTGRES_SERVER --port=$PYTHAGORAS_POSTGRES_PORT --user=$PYTHAGORAS_POSTGRES_USER $PYTHAGORAS_POSTGRES_DATABASE
fi
