#!/bin/bash
a="please enter a sentence: welcome to xiaoju xi'an shanxi china"
echo $a
b=(`echo $a |awk '{print $1}'` `echo $a |awk '{print $2}'` `echo $a |awk '{print $3}'` `echo $a |awk '{print $4}'` `echo $a |awk '{print $5}'` `echo $a |awk '{print $6}'` `echo $a |awk '{print $7}'` `echo $a |awk '{print $8}'` `echo $a| awk '{print $9}'` `echo $a |awk '{print $10}'`)
for ((i=0;i<${#b[@]};i++))
do
if [ `echo ${b[i]} |tr -d "\n"|wc -c` -lt 2 -o `echo ${b[i]} |tr -d "\n" |wc -c` -ge 6 ];then
echo ${b[i]}
fi
done
