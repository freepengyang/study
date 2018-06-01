#!/bin/bash
sed -i '/^IPVS_MODULES_UNLOAD/s/yes/no/' /etc/sysconfig/ipvsadm-config
 sed -i '/^IPVS_SAVE_ON_STOP/s/no/yes/' /etc/sysconfig/ipvsadm-config
sed -i '/^IPVS_SAVE_ON_RESTART/s/no/yes/' /etc/sysconfig/ipvsadm-config
systemctl	restart ipvsadm
ipvsadm -S > /etc/sysconfig/ipvsadm
systemctl	enable ipvsadm
