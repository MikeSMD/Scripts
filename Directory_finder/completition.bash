#!/bin/bash

# funkce pro bash automatické doplňování

_sp_completions() {
	dbfile="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)/dbfile.db"
    	current="${COMP_WORDS[COMP_CWORD]}"
	result=$(sqlite3 $dbfile "SELECT name FROM File WHERE current_dir = '$(pwd)' AND name LIKE '$current%'" 2> /dev/null )

	if [[ -n $result ]]; then
		COMPREPLY=($result)
	fi
    
}
complete -F _sp_completions sp

