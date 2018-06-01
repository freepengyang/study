#!/bin/bash
iptables -F
iptables -X
iptables -t nat -F
iptables -t nat -X
iptables -A INPUT -m state --state ESTABLISHED,RELATED -j ACCEPT
iptables -A INPUT -i lo -j ACCEPT
iptables -A INPUT -s 192.168.2.0/24 -j ACCEPT
iptables -A INPUT -s 192.168.3.0/24 -j ACCEPT
iptables -A INPUT -p tcp --dport 80 -j ACCEPT
iptables -A INPUT -p tcp --dport 25 -j ACCEPT
iptables -A INPUT -p tcp --dport 110 -j ACCEPT
iptables -A INPUT -p tcp --dport 22 -j ACCEPT
iptables -A INPUT -p tcp --dport 20 -j ACCEPT					iptables -A INPUT -p tcp --dport 21 -j ACCEPT	
iptables -A INPUT -p tcp --dport 50000:60000 -j ACCEPT	
systemctl restart iptables
iptables-save > /etc/sysconfig/iptables
systemc	enable iptables
