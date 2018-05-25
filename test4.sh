#!/bin/bash
cd /root/desk/shell练习脚本
arry=(`cat new-domain.list |sort|uniq|tr "\n" " "`)
for i in ${arry[*]}
do
   sed  "s/goo212.com/$i/"  temp.conf  >> master.conf  
done
