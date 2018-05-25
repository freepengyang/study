#!/bin/bash
dir=/smallju
[ ! -d $dir ] && mkdir $dir
cd /smallju
STRING(){
arry=( {a..z} )
for i in {1..10}; do
index=$[RANDOM%10]
echo -n ${arry[index]}
done
}

for j in {1..10}
do
touch `STRING`_smallju.html
done
