#!/bin/bash

if ! command -v sqlite3 >/dev/null 2>&1; then
    echo "For this script, it is necessary to have SQLite installed"
    exit 1
fi
message="$1"
dbfile="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)/dbfile.db"
if [[ $message == "--clear" ]]; then
	sqlite3 $dbfile "DELETE FROM File"
	exit 0
fi

result=$(sqlite3 $dbfile "SELECT path, counter FROM File WHERE current_dir = '$(pwd)' AND name='$message'" )

if [[  -n $result ]]; then
	path=$(echo "$result" | cut -d '|' -f 1)
	counter=$(echo "$result" | cut -d '|' -f 2)

	sqlite3 $dbfile "UPDATE File SET counter = $((counter+1)) WHERE name='$message' AND current_dir='$(pwd)'"
else
	path=$(find . -name "$message" -print -quit 2>/dev/null)
	if [[ -z "$path" ]]; then
  		echo "File not found"
  		exit 1
	fi
	sqlite3 $dbfile "INSERT INTO File(current_dir, name, path, date_added, counter) VALUES('$(pwd)', '$message', '$path', datetime('now'), 1)"
fi

if [ -d "$path" ]; then
	echo "changing directory to $path"
	cd "$path"

elif [ -f "$path" ]; then
	echo "changing directory and running neovim editor to $path"
	cd "${path%/*}"
	nvim "${path##*/}"
else
	echo "directory or file not found"
fi
