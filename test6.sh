#!/bin/bash
echo  "恭喜"
names=(`cat /root/desk/name.txt`)
for i in {1..10};do
echo -en "${names[$[RANDOM%38]]}\r"
sleep 1
if [ $i -eq 10 ];then
echo -ne "\033[33m${names[$[RANDOM%38]]}\033[0m --->>中奖了"
fi
done
