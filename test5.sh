#!/bin/bash
while :
do
read -p "请输入你的服务名称" a
[ -z $a  ] && continue
[ "$a" =  "exit"  ] && exit
 ldd `which $a` | grep wrap
if [ `echo $?` -eq 0  ];then
echo "$a服务支持"；
else
echo  "$a服务不支持"；

fi
done
