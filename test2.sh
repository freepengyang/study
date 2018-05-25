#!/bin/bash
cd /smallju
for i in `ls`
do
a=`echo $i |cut -c 1-10`
mv ${a}_smallju.html ${a}_bigju.HTML
done
