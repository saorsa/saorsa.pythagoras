#!/usr/bin/env bash

CURRENT_DIR=$(pwd)
SCRIPT_DIR="$( cd -- "$( dirname -- "${BASH_SOURCE[0]}" )" &> /dev/null && pwd )"

. "$SCRIPT_DIR/shell.properties"

MIGRATION_NAME=${1:-Init}
MIGRATION_FOLDER="${SCRIPT_DIR}/../src/Pythagoras.Persistence.Npgsql/"
ENV_NAME="development"

function configure_or_die {

    local CONFIG_SCRIPT="${SCRIPT_DIR}/run-env-config.sh"

    echo -e "${FG_BLUE}Configuring with '$CONFIG_SCRIPT'...${REMOVE_ALL_FORMATTING}"
    source "$CONFIG_SCRIPT" "development"

    local CONFIG_EXIT_CODE=$?

    if [ "$CONFIG_EXIT_CODE" -ne "0" ]; then
        echo -e "${FG_RED}Fatal ($CONFIG_EXIT_CODE): Configuration has failed.${REMOVE_ALL_FORMATTING}"
        exit $CONFIG_EXIT_CODE
    fi
}

function migration_remove_or_die {

    if [ ! -d "$MIGRATION_FOLDER" ]; then
        echo -e "${FG_RED}Fatal (21): Migrations root '$MIGRATION_FOLDER' does not exist.${REMOVE_ALL_FORMATTING}"
        exit 21
    fi

    cd "$MIGRATION_FOLDER"
    dotnet ef migrations remove \
        --context="Saorsa.Pythagoras.Persistence.PythagorasDbContext" \
        --verbose

    local EF_EXIT_CODE=$?
    cd "$CURRENT_DIR"

    if [ "$EF_EXIT_CODE" -ne "0" ]; then
        echo -e "${FG_RED}Fatal ($EF_EXIT_CODE): dotnet ef migrations has failed.${REMOVE_ALL_FORMATTING}"
        exit $EF_EXIT_CODE
    else
        echo -e "${FG_BLUE}Latest migration has been removed.${REMOVE_ALL_FORMATTING}"
    fi
}

configure_or_die

migration_remove_or_die
