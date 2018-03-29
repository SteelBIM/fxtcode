#!/bin/bash
#
# General purpose functions.
#

#
# Translates the given relative path into an absolute path and
# returns it.
#
absPath() {
    local PARENT_DIR=$(dirname "$1")
    cd "$PARENT_DIR"
    local ABS_PATH="$(pwd)"/"$(basename $1)"
    cd - >/dev/null
    echo $ABS_PATH
}

#
# Returns "True" if the specified process is running, the empty
# string otherwise.
#
isActiveProcess() {
	if ps ax | grep -v grep | grep $1 > /dev/null
	then echo "True"
	else echo ""
	fi
}
