#!/bin/sh
echo make sure VS is closed before running!
read -p "Enter a description of your changes: " desc
read -p "Really commit changes? (Y/n) [n]: " yn
if [ ! -n "$yn" ]
then
    yn=n
fi
if [ "$yn" == "n" ] || [ "$yn" == "N" ] || [ "$yn" == "no" ] || [ "$yn" == "No" ]
then
    exit 0
fi
echo cleaning files..
# remove build stuff 
# we only want to commit source
rm -R _ReSharper.*
rm -R */bin
rm -R */obj
rm *.suo
rm *.user
rm */*.user
echo commiting changes..
git add *
if [ ! -n "$desc" ]
then
    git commit
else
    git commit -m \'"$desc"\'
fi
git push -u origin master