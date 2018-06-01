#!/bin/bash
a=`df -h |grep root | awk -F "%| +" '{print $5}'`
if [ $a -gt 50 ]
then
echo "您的根分区使用率已经达到$a%"
else
echo "您的根分区使用率正常"
fi

